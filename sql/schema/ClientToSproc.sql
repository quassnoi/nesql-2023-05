CREATE PROCEDURE dbo.ClientToSproc
        @json NVARCHAR(MAX)
AS
-- Do some stuff with the tabular data
INSERT
INTO    Target
SELECT  COUNT(*),
        SUM(NumericValue)
FROM    OPENJSON(@json) WITH
        (
        id BIGINT,
        numericValue NUMERIC,
        textValue NVARCHAR(500)
        )
WHERE   NumericValue > 0
        AND TextValue = 'foo';
GO