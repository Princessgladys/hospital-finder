-- FUNCTION THAT IMPLEMENTS BOYER MOORE STRING MATCHING ALGORITHM
-- RETURN INTEGER VALUE INDICATING POSITION INDEX OF MATCHED PATTERN IN TEXT
-- RETURN 9999: Not matched, #9999: Matched position
-- SONNX
IF OBJECT_ID('[FU_TAKE_MATCHED_STRING_POSITION]', 'FN') IS NOT NULL
	DROP FUNCTION FU_TAKE_MATCHED_STRING_POSITION
GO
CREATE FUNCTION [dbo].[FU_TAKE_MATCHED_STRING_POSITION]
(
	@Text NVARCHAR(MAX),
	@Pattern NVARCHAR(MAX)
)
RETURNS INT
AS
BEGIN
	SET @Text = LOWER(LTRIM(RTRIM(@Text)))
	SET @Pattern = LOWER(LTRIM(RTRIM(@Pattern)))

	DECLARE @Shift INT = 0
	DECLARE @TextLength INT = LEN(@Text)
	DECLARE @PatternLength INT = LEN(@Pattern)
	DECLARE @BadMatchTable NVARCHAR(256) = [dbo].[FU_CREATE_BAD_MATCH_TABLE] (@Pattern)
	DECLARE @Count1 INT = 0
	DECLARE @Count2 INT = 0

	DECLARE @CharInText NVARCHAR(1) = NULL
	DECLARE @CharInPattern NVARCHAR(1) = NULL
	DECLARE @Temp INT = 0
	
	WHILE (@Count1 <= (@TextLength - @PatternLength))
	BEGIN
		SET @Shift = 0
		SET @Count2 = @PatternLength - 1

		WHILE (@Count2 >= 0)
		BEGIN
			SET @CharInText =  SUBSTRING(@Text, @Count1 + @Count2 + 1, 1)
			SET @CharInPattern =  SUBSTRING(@Pattern, @Count2 + 1, 1)

			IF (@CharInPattern != @CharInText)
			BEGIN
				IF (CHARINDEX(@CharInText, @BadMatchTable) = 0)
				BEGIN
					SET @Temp = @Count2
					IF (@Temp > 1)
						SET @Shift = @Temp
					ELSE
						SET @Shift = 1
					BREAK
				END
				ELSE
				BEGIN
					DECLARE @A INT = CHARINDEX(@CharInText, @BadMatchTable) - 2
					DECLARE @B NVARCHAR(1)  = SUBSTRING(@BadMatchTable, CHARINDEX(@CharInText, @BadMatchTable) - 2, 1)

					SET @Temp = @Count2 - CONVERT(INT, SUBSTRING(@BadMatchTable, CHARINDEX(@CharInText, @BadMatchTable) - 2, 1))
					IF (@Temp > 1)
						SET @Shift = @Temp
					ELSE
						SET @Shift = 1
					BREAK
				END
			END

			SET @Count2 -= 1
		END

		IF (@Shift = 0)
			RETURN @Count1;

		SET @Count1 += @Shift
	END

	RETURN 9999
END