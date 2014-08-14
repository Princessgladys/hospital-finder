-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO INSERT FEEDBACK
-- ANHDTH

IF OBJECT_ID ('[SP_INSERT_FEEDBACK]','P') IS NOT NULL
	DROP PROCEDURE [SP_INSERT_FEEDBACK]
GO
CREATE PROCEDURE [dbo].[SP_INSERT_FEEDBACK]
	@header nvarchar(64),
	@feedback_content nvarchar(256),
	@feedback_type int,
	@email varchar(64),
	@hospitalID int
AS
BEGIN
	BEGIN TRANSACTION
		DECLARE @created_date DATETIME = GETDATE()
		IF(@hospitalID=0)
		BEGIN
			SET @hospitalID=NULL;
		END
		INSERT INTO Feedback
		VALUES
		(@header
		,@feedback_content
		,@feedback_type
		,@email
		,@hospitalID
		,@created_date)
		IF @@ERROR <>0
		BEGIN
			ROLLBACK TRAN;
			RETURN 0;
		END
	COMMIT TRAN;
	RETURN 1;
END