# What is it?

This is support code for the lightning talk I gave for New England SQL Server group in May, 2023.

It compares performance of JSON and Table Variables as a means to pass tabular data to SQL Server stored procedures.

# Prerequisites

* VSCode
  * [SQL Database Projects](https://marketplace.visualstudio.com/items?itemName=ms-mssql.sql-database-projects-vscode) extension
* Docker
* .NET 7.0 SDK

# Setup

## Install the database in a Docker container

1. In the VSCode side menu, locate Database Projects
1. Right-click on the project `sql`, select "Publish"
1. Select "Publish to new SQL server local development container"
1. Select the default port (1433)
1. For the admin password, select `Password123`. If you choose another password, you will have to change it in the [`appsettings.json`](./src/appsettings.json) as well.
1. Keep selecting default values
1. The publish process will start and complete in several seconds.
1. VSCode should show a toast message saying **"Database project published successfully"**

## Install the dotnet-script framework

```bash
cd src
dotnet tool restore
```

## Run the test scripts

Inside the `src` folder:

```bash
# Measure Table Variable performance
./runTv.sh
```

```bash
# Measure JSON performance
./runJson.sh
```

1. The test scripts start 8 parallel worker processes.
2. Each worker process runs the stored procedures sequentially in a loop. At any given point in time 8 procedures are running concurrently (less negligible time spent on running C# code between invocations for populating parameters, parsing results and measuring time).
3. Every 20 invocations of the stored procedure, a worker prints hash character (`#`) on the screen.
4. The test scripts run until stopped with a Ctrl-C.
5. If it doesn't work, check [`appsettings.json`](./src/appsettings.json). It assumes that you followed the steps in the section "Install the database" above.

Have fun!