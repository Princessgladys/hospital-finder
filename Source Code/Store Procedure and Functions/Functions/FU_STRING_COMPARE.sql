-- CALCULATE THE PERCENTEAGE OF SIMILARITY BETWEEN 2 STRINGS
-- SONNX
IF OBJECT_ID('FU_STRING_COMPARE', 'F') IS NOT NULL
	DROP FUNCTION FU_STRING_COMPARE
GO
CREATE FUNCTION [dbo].[FU_STRING_COMPARE]
(
	@InputStr1 NVARCHAR(MAX),
	@InputStr2 NVARCHAR(MAX)
)
RETURNS FLOAT
AS
BEGIN
	DECLARE @Result FLOAT
	DECLARE @Union INT = 0
	DECLARE @Intersection INT = 0

	DECLARE @RowNumber1 INT = 1
	DECLARE @RowNumber2 INT
	DECLARE @NumOfPairsStr1 INT
	DECLARE @NumOfPairsStr2 INT
	DECLARE @PairStr1 NVARCHAR(2)
	DECLARE @PairStr2 NVARCHAR(2)

	SELECT @NumOfPairsStr1 = (SELECT COUNT(TokenList.ID)
							  FROM [dbo].[FU_TAKE_PAIRS_OF_LETTER_IN_WORD] (@InputStr1) TokenList)
	SELECT @NumOfPairsStr2 = (SELECT COUNT(TokenList.ID)
							  FROM [dbo].[FU_TAKE_PAIRS_OF_LETTER_IN_WORD] (@InputStr2) TokenList)

	SET @Union = @NumOfPairsStr1 + @NumOfPairsStr2

	WHILE (@RowNumber1 <= @NumOfPairsStr1)
	BEGIN
		SELECT @PairStr1 = (SELECT PairList.Pair
							FROM (SELECT ROW_NUMBER()
								  OVER (ORDER BY PairList.ID ASC) AS RowNumber, PairList.Pair
								  FROM [dbo].[FU_TAKE_PAIRS_OF_LETTER_IN_WORD] (@InputStr1) PairList) AS PairList
							WHERE RowNumber = @RowNumber1)
		SET @RowNumber2 = 1

		WHILE (@RowNumber2 <= @NumOfPairsStr2)
		BEGIN
			SELECT @PairStr2 = (SELECT PairList.Pair
								FROM (SELECT ROW_NUMBER()
									  OVER (ORDER BY PairList.ID ASC) AS RowNumber, PairList.Pair
									  FROM [dbo].[FU_TAKE_PAIRS_OF_LETTER_IN_WORD] (@InputStr2) PairList) AS PairList
								WHERE RowNumber = @RowNumber2)

			IF (@PairStr1 = @PairStr2)
			BEGIN
				SET @Intersection += 1
			END

			SET @RowNumber2 += 1
		END

		SET @RowNumber1 += 1
	END

	SET @Result = CONVERT(FLOAT, 2 * @Intersection) / CONVERT(FLOAT, @Union)
	RETURN @Result
END