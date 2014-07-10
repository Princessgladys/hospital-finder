-- TAKE PAIRS OF LETTERS OF EACH WORD IN A LIST OF TOKENS
-- SONNX
IF OBJECT_ID('FU_TAKE_PAIRS_OF_LETTER_IN_WORD', 'F') IS NOT NULL
	DROP FUNCTION FU_TAKE_PAIRS_OF_LETTER_IN_WORD
GO
CREATE FUNCTION [dbo].[FU_TAKE_PAIRS_OF_LETTER_IN_WORD]
(
	@InputStr NVARCHAR(MAX)
)
RETURNS @PairList TABLE (ID INT PRIMARY KEY, 
						 Pair NVARCHAR(2))
AS
BEGIN
	DECLARE @RowNumber INT = 1
	DECLARE @NumOfPairs INT
	DECLARE @RowNumber2 INT
	DECLARE @Token NVARCHAR(32)
	DECLARE @Pair NVARCHAR(2)
	DECLARE @Id INT = 1

	DECLARE @TotalToken INT
	SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
						  FROM [dbo].[FU_STRING_TOKENIZE] (@InputStr, ' ') TokenList)

	WHILE (@RowNumber <= @TotalToken)
	BEGIN
		SELECT @Token = (SELECT TokenList.Token
						FROM (SELECT ROW_NUMBER()
							  OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
							  FROM [dbo].[FU_STRING_TOKENIZE] (@InputStr, ' ') TokenList) AS TokenList
							  WHERE RowNumber = @RowNumber)

		SELECT @NumOfPairs = (SELECT COUNT(PairList.ID)
							  FROM [dbo].[FU_TAKE_PAIRS_OF_LETTER] (@Token) PairList)
		SET @RowNumber2 = 1

		WHILE(@RowNumber2 <= @NumOfPairs)
		BEGIN
			SELECT @Pair = (SELECT PairList.Pair
							FROM (SELECT ROW_NUMBER()
								  OVER (ORDER BY PairList.ID ASC) AS RowNumber, PairList.Pair
								  FROM [dbo].[FU_TAKE_PAIRS_OF_LETTER] (@Token) PairList) AS PairList
								  WHERE RowNumber = @RowNumber2)

			INSERT INTO @PairList 
			SELECT @Id, @Pair

			SET @RowNumber2 += 1
			SET @Id += 1
		END

		SET @RowNumber += 1
	END

	RETURN
END