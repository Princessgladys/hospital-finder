-- SCRIPT TO SEARCH HOSPITALS
-- USING LOCATION OPTION
-- VIETLP
IF OBJECT_ID('[SP_RATE_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_RATE_HOSPITAL
GO

CREATE PROCEDURE SP_RATE_HOSPITAL
	@User_ID INT,
	@Hospital_ID INT,
	@Score INT
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
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
		END CATCH		
	END
END
GO