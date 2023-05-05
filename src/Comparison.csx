#pragma warning disable IDE0002
#nullable enable

#r "nuget: System.CommandLine, 2.0.0-beta4.22272.1"
#r "nuget: System.CommandLine.NamingConventionBinder, 2.0.0-beta4.22272.1"
#r "nuget: Microsoft.Data.SqlClient, 5.1.1"
#r "nuget: Microsoft.Extensions.Configuration, 7.0.0"
#r "nuget: Microsoft.Extensions.Configuration.EnvironmentVariables, 7.0.0"
#r "nuget: Microsoft.Extensions.Configuration.Json, 7.0.0"
#r "nuget: System.Linq.Async, 6.0.1"
#r "nuget: System.Interactive.Async, 6.0.1"
#r "nuget: System.Text.Json, 7.0.2"

using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

var rowCountOption = new Option<int>("--rowCount", "Number of rows to pass to the stored procedure")
{
    IsRequired = true
};
var workersOption = new Option<int>("--workers", "Number of workers to run in parallel")
{
    IsRequired = true
};
var chunkSizeOption = new Option<int>("--chunkSize", "Number of iterations to run in a single batch")
{
    IsRequired = true
};
var runnerTypeOption = new Option<RunnerType>("--runnerType", "Type of runner to use")
{
    IsRequired = true
};

var rootCommand = new RootCommand("Comparison of different ways to pass data to SQL Server")
{
    rowCountOption,
    workersOption,
    chunkSizeOption,
    runnerTypeOption
};
rootCommand.SetHandler(Measure, rowCountOption, workersOption, chunkSizeOption, runnerTypeOption);

readonly record struct InputRecord(long Id, decimal NumericValue, string TextValue);

async Task<decimal?> SumValuesTv(IEnumerable<InputRecord> values)
{
    using var dataTable = new DataTable
    {
        Columns = {
            new DataColumn("id", typeof(long)),
            new DataColumn("numericValue", typeof(decimal)),
            new DataColumn("textValue", typeof(string))
        }
    };

    foreach (var (id, numericValue, textValue) in values)
    {
        dataTable.Rows.Add(id, numericValue, textValue);
    }

    await using var command = new SqlCommand("dbo.SumValuesTv", connection)
    {
        CommandType = CommandType.StoredProcedure,
        Parameters = {
            new("@values", SqlDbType.Structured) {
                Value = dataTable
            }
        }
    };
    return await command.ExecuteScalarAsync() is decimal sum ? sum : null;
}


async Task<decimal?> SumValuesJson(IEnumerable<InputRecord> values)
{
    var value = JsonSerializer.Serialize(values, new JsonSerializerOptions
    {
        IncludeFields = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
    await using var command = new SqlCommand("dbo.SumValuesJson", connection)
    {
        CommandType = CommandType.StoredProcedure,
        Parameters = {
            new("@values", SqlDbType.NVarChar) {
                Value = value
            }
        }
    };
    return (await command.ExecuteScalarAsync() is decimal sum) ? sum : null;
}

async IAsyncEnumerable<(int id, long timeMilliseconds, decimal? sum)> TimeExecution(int id, int chunkSize, Func<Task<decimal?>> executor)
{
    var stopwatch = new Stopwatch();
    while (true)
    {
        decimal sum = 0;
        stopwatch.Restart();
        for (var i = 0; i < chunkSize; i++)
        {
            sum += await executor() is decimal value ? value : 0;
        }
        stopwatch.Stop();
        yield return (id, stopwatch.ElapsedMilliseconds, sum);
    }
}

delegate Task<decimal?> Runner(IEnumerable<InputRecord> values);

async Task Measure(int rowCount, int workers, int chunkSize, Runner runner)
{
    Task<decimal?> addValues()
    {
        var random = new Random();
        var values = Enumerable.Range(0, rowCount)
            .Select(index => new InputRecord(index, random.Next(0, 1000), Guid.NewGuid().ToString()))
            .ToList();
        return runner(values);
    }

    var runners = Enumerable.Range(0, workers)
        .Select(id => TimeExecution(id, chunkSize, addValues))
        .ToArray();

    await foreach (var (id, timeMilliseconds, sum) in AsyncEnumerableEx.Merge(runners))
    {
        await Console.Out.WriteAsync('#');
        await Console.Out.FlushAsync();
    }
}

public enum RunnerType
{
    Tv,
    Json
}

async Task Measure(int rowCount, int workers, int chunkSize, RunnerType runnerType)
{
    Runner runner = runnerType switch
    {
        RunnerType.Tv => SumValuesTv,
        RunnerType.Json => SumValuesJson,
        _ => throw new ArgumentOutOfRangeException(nameof(runnerType))
    };
    await Measure(rowCount, workers, chunkSize, runner);
}

Environment.ExitCode = await rootCommand.InvokeAsync(Args.ToArray());