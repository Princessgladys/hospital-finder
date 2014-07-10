-- SPLIT WORDS IN A STRING INTO LIST OF TOKENS
-- RETURN TABLE THAT CONTAINS LIST OF TOKENS
-- SONNX
IF OBJECT_ID('[FU_STRING_TOKENIZE]', 'F') IS NOT NULL
	DROP FUNCTION FU_STRING_TOKENIZE
GO
CREATE FUNCTION [dbo].[FU_STRING_TOKENIZE]
(
	@InputStr NVARCHAR(MAX),
	@Delimeter VARCHAR(1)
)
RETURNS @TokenList TABLE ([Token] [nvarchar] (512))
AS
BEGIN
	DECLARE @Token NVARCHAR(128)
	DECLARE @Position INT

	WHILE CHARINDEX(@Delimeter, @InputStr) > 0
	BEGIN
		SELECT @Position = CHARINDEX(@Delimeter, @InputStr)
		SELECT @Token = SUBSTRING(@InputStr, 1, @Position - 1)

		INSERT INTO @TokenList 
		SELECT @Token

		SELECT @InputStr = SUBSTRING(@InputStr, @Position + 1, LEN(@InputStr) - @Position)
	END

	INSERT INTO @TokenList
	SELECT @InputStr

	RETURN
END