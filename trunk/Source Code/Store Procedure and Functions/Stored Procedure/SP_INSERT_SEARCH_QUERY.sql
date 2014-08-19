-- SCRIPT TO INSERT SEARCH QUERY
-- VIETLP
IF OBJECT_ID('[SP_INSERT_SEARCH_QUERY]', 'P') IS NOT NULL
	DROP PROCEDURE SP_INSERT_SEARCH_QUERY
GO

CREATE PROCEDURE SP_INSERT_SEARCH_QUERY
(
	@Search_Query NVARCHAR(100),
	@Result_Count INT,
	@Search_Date DATETIME
)
AS
BEGIN
	DECLARE @Sentece_ID INT
	SET @Sentece_ID = 0
	
	SELECT @Sentece_ID = sq.Sentence_ID
	FROM SearchQuery sq
	WHERE sq.Sentence LIKE @Search_Query
	
	IF (@Sentece_ID > 0)
	BEGIN
		UPDATE SearchQuery
		SET Search_Date = @Search_Date, Result_Count = @Result_Count, Search_Time_Count = Search_Time_Count + 1
		WHERE Sentence_ID = @Sentece_ID
		RETURN @Result_Count
	END
	ELSE
	BEGIN
		INSERT INTO SearchQuery(Sentence, Search_Date, Result_Count, Search_Time_Count)
		VALUES(@Search_Query, @Search_Date, @Result_Count, 1)
		RETURN @Result_Count
	END
END