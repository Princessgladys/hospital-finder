-- SCRIPT TO RATE HOSPITAL
-- VIETLP
IF OBJECT_ID('[SP_RATE_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_RATE_HOSPITAL
GO

CREATE PROCEDURE SP_RATE_HOSPITAL
(
	@User_ID INT,
	@Hospital_ID INT,
	@Score INT
)
AS
BEGIN
	IF (1 <= @Score AND @Score <= 5)
	BEGIN
		DECLARE @Check INT
		SET @Check = 0
		SELECT @Check = COUNT(r.Rating_ID)
		FROM Rating r
		WHERE r.Hospital_ID = @Hospital_ID AND r.Created_Person = @User_ID
		
		BEGIN TRAN
		BEGIN TRY
			IF (@Check > 0)
				BEGIN
					UPDATE Rating
					SET Score = @Score
					WHERE Hospital_ID = @Hospital_ID AND Created_Person = @User_ID
				END
			ELSE
				BEGIN
					INSERT INTO Rating (Score, Hospital_ID, Created_Person)
					VALUES (@Score, @Hospital_ID, @User_ID)
				END
			
			DECLARE @Average_Score FLOAT
			SET @Average_Score = [dbo].[FU_CALCULATE_AVERAGE_RATING] (@Hospital_ID)
			
			DECLARE @Rating_Count INT
			SELECT @Rating_Count = COUNT(r.Rating_ID)
			FROM Rating r
			WHERE r.Hospital_ID = @Hospital_ID
			
			UPDATE Hospital
			SET Rating = @Average_Score, Rating_Count = @Rating_Count
			WHERE Hospital_ID = @Hospital_ID
						
			COMMIT TRAN
			RETURN @Rating_Count
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
			RETURN -1
		END CATCH		
	END
END
GO


DECLARE	@return_value float
EXEC	@return_value = [dbo].[FU_CALCULATE_AVERAGE_RATING] 2
SELECT	'Return Value' = @return_value
GO

DECLARE	@return_value float
EXEC	@return_value = [SP_RATE_HOSPITAL] 3, 20, 1
SELECT	'Return Value' = @return_value
GO

EXEC [SP_RATE_HOSPITAL] 84, 450, 5