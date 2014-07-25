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
		BEGIN TRAN
		BEGIN TRY
			INSERT INTO Rating (Score, Hospital_ID, Created_Person)
			VALUES (@Score, @Hospital_ID, @User_ID)
			
			DECLARE @Average_Score FLOAT
			SET @Average_Score = [dbo].[FU_CALCULATE_AVERAGE_RATING] (@Hospital_ID)
			
			UPDATE Hospital
			SET Rating = @Average_Score
			WHERE Hospital_ID = @Hospital_ID
						
			COMMIT TRAN
			RETURN @Average_Score
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
		END CATCH		
	END
END
GO


DECLARE	@return_value float
EXEC	@return_value = [dbo].[FU_CALCULATE_AVERAGE_RATING] 2
SELECT	'Return Value' = @return_value
GO

DECLARE	@return_value float
EXEC	@return_value = [SP_RATE_HOSPITAL] 1, 2, 1
SELECT	'Return Value' = @return_value
GO