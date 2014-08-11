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
	INSERT INTO SentenceDictionary(Sentence, Result_Count, Search_Date)
	VALUES(@Search_Query, @Result_Count, @Search_Date)
	RETURN @Result_Count
END