-- SCRIPT TO INSERT SEARCH QUERY
-- VIETLP
IF OBJECT_ID('[SP_LOAD_SEARCH_QUERY_STATISTIC]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_SEARCH_QUERY_STATISTIC
GO

CREATE PROCEDURE SP_LOAD_SEARCH_QUERY_STATISTIC
(
	@From_Date DATETIME,
	@To_Date DATETIME
)
AS
BEGIN
	SELECT sd.Sentence, COUNT(*) AS Search_Time_Count, sd.Result_Count
	FROM SentenceDictionary sd
	WHERE sd.Search_Date BETWEEN @From_Date AND @To_Date
	GROUP BY sd.Sentence, CAST(ISNULL(sd.Search_Date, '1900-01-01') AS DATE), sd.Result_Count
	ORDER BY Search_Time_Count DESC
END

EXEC SP_LOAD_SEARCH_QUERY_STATISTIC '1900-12-31', '2015-12-31'