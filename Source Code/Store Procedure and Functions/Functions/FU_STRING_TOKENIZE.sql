-- SPLIT WORDS IN A STRING INTO LIST OF TOKENS
-- RETURN TABLE THAT CONTAINS LIST OF TOKENS
-- SONNX
IF OBJECT_ID('[FU_STRING_TOKENIZE]', 'FN') IS NOT NULL
	DROP FUNCTION FU_STRING_TOKENIZE
GO
CREATE FUNCTION [dbo].[FU_STRING_TOKENIZE]
(
	@InputStr NVARCHAR(MAX),
	@Delimeter VARCHAR(1)
)
RETURNS @TokenList TABLE (ID INT PRIMARY KEY,
						  Token NVARCHAR(MAX))
AS
BEGIN
	DECLARE @Start INT
	DECLARE @End INT
	DECLARE @Id INT = 1
    SELECT @Start = 1, @End = CHARINDEX(@Delimeter, @InputStr) 

    WHILE @Start < LEN(@InputStr) + 1
	BEGIN 
        IF @End = 0  
            SET @End = LEN(@InputStr) + 1
       
        INSERT INTO @TokenList
        SELECT @Id, (SUBSTRING(@InputStr, @Start, @End - @Start))

        SET @Start = @End + 1 
        SET @End = CHARINDEX(@Delimeter, @InputStr, @Start)
        SET @Id += 1
    END 
    RETURN 
END