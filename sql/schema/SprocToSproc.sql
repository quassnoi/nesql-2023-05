CREATE PROCEDURE dbo.SprocToSproc
        @id1 INT,
        @numericValue1 NUMERIC(18, 2),
        @textValue1 NVARCHAR(500),
        @id2 INT,
        @numericValue2 NUMERIC(18, 2),
        @textValue2 NVARCHAR(500)
AS
DECLARE @json NVARCHAR(MAX) =
        (
        SELECT id, numericValue, textValue
        FROM    (
                VALUES
                (@id1, @numericValue1, @textValue1),
                (@id2, @numericValue2, @textValue2)
                ) AS t(id, numericValue, textValue)
        FOR JSON PATH
        );
EXEC dbo.ClientToSproc
        @json = @json;
GO