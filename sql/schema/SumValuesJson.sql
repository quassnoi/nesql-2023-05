CREATE PROCEDURE
        dbo.SumValuesJson
        @values NVARCHAR(MAX)
AS
SELECT SUM(NumericValue)
FROM OPENJSON(@values) WITH
        (
        id BIGINT,
        numericValue NUMERIC,
        textValue NVARCHAR(500)
        );
GO