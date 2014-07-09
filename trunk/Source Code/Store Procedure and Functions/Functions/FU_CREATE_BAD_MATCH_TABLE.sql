-- FUNCTION TO CREATE BAD MATCH TABLE FOR BOYER MOORE STRING MATCHING ALGORITHM
-- SONNX
IF OBJECT_ID('[FU_CREATE_BAD_MATCH_TABLE]', 'F') IS NOT NULL
	DROP FUNCTION FU_CREATE_BAD_MATCH_TABLE
GO
CREATE FUNCTION [dbo].[FU_CREATE_BAD_MATCH_TABLE]
(
	@Pattern NVARCHAR(MAX)
)
RETURNS NVARCHAR(256)
AS
BEGIN
	SET @Pattern = LOWER(@Pattern)
	DECLARE @BadMatchTable NVARCHAR(256) = NULL

	DECLARE @Count INT = 1
	DECLARE @PatternLength INT 
	SET @PatternLength = LEN(RTRIM(LTRIM(@Pattern)))

	DECLARE @CharInPattern CHAR = NULL

	WHILE (@Count <= @PatternLength)
	BEGIN
		SET @CharInPattern =  SUBSTRING(@Pattern, @Count, 1)

		IF (CHARINDEX(@CharInPattern, @BadMatchTable) > 0)
		BEGIN
			SET @BadMatchTable = REPLACE(@BadMatchTable,
				SUBSTRING(@BadMatchTable,
					CHARINDEX(@CharInPattern, @BadMatchTable) - 2, 1), @Count)
		END
		ELSE
		BEGIN
			SET @BadMatchTable = CONCAT(@BadMatchTable, '[', @Count, ',', @CharInPattern, ']|')
		END

		SET @Count += 1
	END

	RETURN @BadMatchTable
END