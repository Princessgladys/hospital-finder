-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF AN USER HAVE BEEN EXISTED IN DATABSE
-- SONNX
IF OBJECT_ID('[SP_INSERT_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_INSERT_HOSPITAL
GO
CREATE PROCEDURE SP_INSERT_HOSPITAL
	@HospitalName NVARCHAR(64),
	@HospitalType INT,
	@Address NVARCHAR(128),
	@CityID INT,
	@DistrictID INT,
	@WardID INT,
	@PhoneNumber VARCHAR(64),
	@Fax VARCHAR(16),
	@Email VARCHAR(64),
	@Website VARCHAR(64),
	@HolidayStartTime NVARCHAR(7),
	@HolidayEndTime NVARCHAR(7),
	@OrdinaryStartTime NVARCHAR(7),
	@OrdinaryEndTime NVARCHAR(7),
	@Coordinate VARCHAR(26),
	@IsAllowAppointment BIT,
	@CreatedPerson INT,
	@FullDescription NVARCHAR(4000),
	@SpecialityList NVARCHAR(4000),
	@ServiceList NVARCHAR(4000),
	@FacilityList NVARCHAR(4000)
AS
BEGIN
	BEGIN TRANSACTION T

	-- INSERT TO HOSPITAL TABLE
	BEGIN
		INSERT INTO Hospital
		(
			Hospital_Name,
			Hospital_Type,
			[Address],
			City_ID,
			District_ID,
			Ward_ID,
			Phone_Number,
			Fax,
			Email,
			Website,
			Holiday_Start_Time,
			Holiday_End_Time,
			Ordinary_Start_Time,
			Ordinary_End_Time,
			Is_Allow_Appointment,
			Full_Description,
			Coordinate,
			Created_Person,
			Is_Active
		)
		VALUES
		(
			@HospitalName,
			@HospitalType,
			@Address,
			@CityID,
			@DistrictID,
			@WardID,
			@PhoneNumber,
			@Fax,
			@Email,
			@Website,
			@HolidayStartTime,
			@HolidayEndTime,
			@OrdinaryStartTime,
			@OrdinaryEndTime,
			@IsAllowAppointment,
			@FullDescription,
			@Coordinate,
			@CreatedPerson,
			'True'
		)

		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN T;
			RETURN 0;
		END

		-- INSERT TO HOSPITAL_SPECIALITY TABLE
		IF (@SpecialityList != '')
		BEGIN
			DECLARE @RowNumber INT = 1
			DECLARE @TotalToken  INT = 0;
			DECLARE @Token VARCHAR(4)
			DECLARE @HospitalID INT = (SELECT TOP 1 Hospital_ID
									   FROM Hospital
									   ORDER BY Hospital_ID DESC)

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)
				INSERT INTO Hospital_Speciality
				(
					Hospital_ID,
					Speciality_ID,
					Is_Main_Speciality,
					Is_Active
				)
				VALUES
				(
					@HospitalID,
					@Token,
					'False',
					'True'
				)

				IF @@ROWCOUNT = 0
				BEGIN
					ROLLBACK TRAN T;
					RETURN 0;
				END

				SET @RowNumber += 1
			END
		END

		-- INSERT TO SERVCE TABLE
		IF (@ServiceList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0;

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)
				INSERT INTO Hospital_Service
				(
					Hospital_ID,
					Service_ID,
					Is_Active
				)
				VALUES
				(
					@HospitalID,
					@Token,
					'True'
				)

				IF @@ROWCOUNT = 0
				BEGIN
					ROLLBACK TRAN T;
					RETURN 0;
				END

				SET @RowNumber += 1
			END
		END

		-- INSERT TO FACILITY TABLE
		IF (@FacilityList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0;

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)
				INSERT INTO Hospital_Facility
				(
					Hospital_ID,
					Facility_ID,
					Is_Active
				)
				VALUES
				(
					@HospitalID,
					@Token,
					'True'
				)

				IF @@ROWCOUNT = 0
				BEGIN
					ROLLBACK TRAN T;
					RETURN 0;
				END

				SET @RowNumber += 1
			END
		END	
	END
	COMMIT TRAN T;

	RETURN 1;
END