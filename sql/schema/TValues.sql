CREATE TYPE dbo.TValues
AS TABLE
        (
        Id BIGINT NOT NULL
                PRIMARY KEY NONCLUSTERED,
        NumericValue NUMERIC NOT NULL,
        TextValue NVARCHAR(500)
        )
WITH    (MEMORY_OPTIMIZED = ON)
GO