SELECT  *
FROM    OPENJSON(
        N'[{"id":1,"value":"some text"},
        {"id":2,"value":"another text"}]')
WITH    (id BIGINT, value NVARCHAR(MAX))