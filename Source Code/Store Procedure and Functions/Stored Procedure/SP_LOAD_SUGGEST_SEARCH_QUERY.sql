-- SCRIPT TO INSERT SEARCH QUERY
-- VIETLP
IF OBJECT_ID('[SP_LOAD_SUGGEST_SEARCH_QUERY]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_SUGGEST_SEARCH_QUERY
GO

CREATE PROCEDURE SP_LOAD_SUGGEST_SEARCH_QUERY
(
	@Search_Query NVARCHAR(100)
)
AS
BEGIN
	SELECT TOP 10 Sentence, Search_Time_Count, Result_Count
	FROM SearchQuery
	WHERE Result_Count > 0 AND Sentence LIKE N'%' + @Search_Query + '%'
	GROUP BY Sentence, Search_Time_Count, Result_Count
	ORDER BY Search_Time_Count DESC, Result_Count DESC
END

EXEC SP_LOAD_SUGGEST_SEARCH_QUERY N'bệnh viện mắt'
GO