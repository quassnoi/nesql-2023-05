CREATE PROCEDURE
        dbo.SumValuesTV
        @values dbo.TValues READONLY
AS
SELECT SUM(NumericValue)
FROM @values;
GO