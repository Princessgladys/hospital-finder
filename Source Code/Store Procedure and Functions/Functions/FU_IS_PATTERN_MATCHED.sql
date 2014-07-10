-- FUNCTION THAT IMPLEMENTS BOYER MOORE STRING MATCHING ALGORITHM
-- RETURN BOOLEAN VALUE INDICATING IF A PATTERN IS APPEARS IN A TEXT OR NOT
-- RETURN 1: True, 0: False
-- SONNX
IF OBJECT_ID('[FU_IS_PATTERN_MATCHED]', 'FN') IS NOT NULL
	DROP FUNCTION FU_IS_PATTERN_MATCHED
GO
CREATE FUNCTION [dbo].[FU_IS_PATTERN_MATCHED]
(
	@Text NVARCHAR(MAX),
	@Pattern NVARCHAR(MAX)
)
RETURNS BIT
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
			RETURN 1;

		SET @Count1 += @Shift
	END

	RETURN 0
END