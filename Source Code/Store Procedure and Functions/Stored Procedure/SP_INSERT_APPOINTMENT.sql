-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO INSERT APPOINTMENT
-- ANHDTH

--ALTER TABLE Appointment
--ADD HealthInsuranceCode VARCHAR(15),
--	SymptomDescription nvarchar(200)

IF OBJECT_ID('[SP_INSERT_APPOINTMENT]') IS NOT NULL
	DROP PROCEDURE [SP_INSERT_APPOINTMENT]
GO
CREATE PROCEDURE [dbo].[SP_INSERT_APPOINTMENT]
	@FullName nvarchar(32),
	@Gender bit,
	@Birthday date,
	@PhoneNo varchar(13),
	@Email varchar(64),
	@Date date,
	@Start_time time,
	@End_time time,
	@Doctor_ID int,
	@Hospital_ID int,
	@Confirm_Code varchar(8),
	@HealthInsuranceCode varchar(15),
	@SymptomDescription nvarchar(200)
AS
BEGIN
	BEGIN TRANSACTION
	INSERT INTO Appointment
		([Patient_Full_Name]
		,[Patient_Gender]
		,[Patient_Birthday]
		,[Patient_Phone_Number]
		,[Patient_Email]
		,[Appointment_Date]
		,[Start_Time]
		,[End_Time]
		,[In_Charge_Doctor]
		,[Curing_Hospital]
		,[Confirm_Code]
		,[Health_Insurance_Code]
		,[Symptom_Description]
		,[Is_Confirm]
		,[Is_Active])
	VALUES
		(@FullName
		,@Gender
		,@Birthday
		,@PhoneNo
		,@Email
		,@Date
		,@Start_time
		,@End_time
		,@Doctor_ID
		,@Hospital_ID
		,@Confirm_Code
		,@HealthInsuranceCode
		,@SymptomDescription
		,'False'
		,'False')
	IF @@ERROR <>0
	BEGIN
		ROLLBACK TRAN;
		RETURN 0;
	END
	COMMIT TRAN T;

	RETURN 1;
END