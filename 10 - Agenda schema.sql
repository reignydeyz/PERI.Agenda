IF NOT EXISTS (SELECT schema_name 
    FROM information_schema.schemata 
    WHERE schema_name = 'prompt' )
BEGIN
    EXEC sp_executesql N'CREATE SCHEMA prompt;';
END