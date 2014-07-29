-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF AN USER HAVE BEEN EXISTED IN DATABSE
-- SONNX
IF OBJECT_ID('[SP_UPDATE_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_UPDATE_HOSPITAL
GO
CREATE PROCEDURE SP_UPDATE_HOSPITAL
	@HosptalID INT,
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
	BEGIN TRY
		-- UPDATE HOSPITAL TABLE
		BEGIN
			UPDATE Hospital
			SET Hospital_Name = @HospitalName,
				Hospital_Type = @HospitalType,
				[Address] = @Address,
				City_ID = @CityID,
				District_ID = @DistrictID,
				Ward_ID = @WardID,
				Phone_Number = @PhoneNumber,
				Fax = @Fax,
				Email = @Email,
				Website = @Website,
				Holiday_Start_Time = @HolidayStartTime,
				Holiday_End_Time = @HolidayEndTime,
				Ordinary_Start_Time = @OrdinaryStartTime,
				Ordinary_End_Time = @OrdinaryEndTime,
				Is_Allow_Appointment = @IsAllowAppointment,
				Full_Description = @FullDescription,
				Coordinate = @Coordinate
			WHERE Hospital_ID = @HosptalID

			IF @@ROWCOUNT = 0
			BEGIN
				ROLLBACK TRAN T;
				RETURN 0;
			END
		END

		-- SELECT HOSPITAL ID
		DECLARE @HospitalID INT = (SELECT TOP 1 Hospital_ID
									FROM Hospital
									ORDER BY Hospital_ID DESC)

		-- UPDATE SPECIALITY TABLE
		IF (@SpecialityList != '')
		BEGIN
			DECLARE @RowNumber INT = 1
			DECLARE @TotalToken  INT = 0;
			DECLARE @Token VARCHAR(4)

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									 OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									 FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)

				IF (EXISTS(SELECT hs.Hospital_ID
						   FROM Hospital_Speciality hs
						   WHERE hs.Speciality_ID = @Token AND
								 hs.Hospital_ID = @HospitalID))
				BEGIN
					-- CHECK STATUS OF SPECIALITY IN HOSPITAL
					IF (EXISTS(SELECT hs.Hospital_ID
							   FROM Hospital_Speciality hs
							   WHERE hs.Speciality_ID = @Token AND
									 hs.Hospital_ID = @HospitalID AND
									 hs.Is_Active = 'False'))
					BEGIN
						UPDATE Hospital_Speciality
						SET Is_Active = 'True'
						WHERE Speciality_ID = @Token AND
							  Hospital_ID = @HospitalID
					END
				END
				ELSE
				BEGIN
					-- INSERT NEW SPECIALITY
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
				END

				SET @RowNumber += 1
			END
		END

		-- UPDATE SERVICE TABLE
		IF (@ServiceList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
									FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									 OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									 FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)

				IF (EXISTS(SELECT hs.Hospital_ID
						   FROM Hospital_Service hs
						   WHERE hs.Service_ID = @Token AND
								 hs.Hospital_ID = @HospitalID))
				BEGIN
					-- CHECK STATUS OF SERVICE IN HOSPITAL
					IF (EXISTS(SELECT hs.Hospital_ID
							   FROM Hospital_Service hs
							   WHERE hs.Service_ID = @Token AND
									 hs.Hospital_ID = @HospitalID AND
									 hs.Is_Active = 'False'))
					BEGIN
						UPDATE Hospital_Service
						SET Is_Active = 'True'
						WHERE Service_ID = @Token AND
							  Hospital_ID = @HospitalID
					END
				END
				ELSE
				BEGIN
					-- INSERT NEW SERVICE
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
				END

				SET @RowNumber += 1
			END
		END

		-- UPDATE FACILITY TABLE
		IF (@FacilityList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
									FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									 OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									 FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)

				IF (EXISTS(SELECT hs.Hospital_ID
						   FROM Hospital_Facility hs
						   WHERE hs.Facility_ID = @Token AND
								 hs.Hospital_ID = @HospitalID))
				BEGIN
					-- CHECK STATUS OF SERVICE IN HOSPITAL
					IF (EXISTS(SELECT hs.Hospital_ID
							   FROM Hospital_Facility hs
							   WHERE hs.Facility_ID = @Token AND
									 hs.Hospital_ID = @HospitalID AND
									 hs.Is_Active = 'False'))
					BEGIN
						UPDATE Hospital_Facility
						SET Is_Active = 'True'
						WHERE Facility_ID = @Token AND
							  Hospital_ID = @HospitalID
					END
				END
				ELSE
				BEGIN
					-- INSERT NEW SERVICE
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
				END

				SET @RowNumber += 1
			END
		END
		COMMIT TRAN T;

		RETURN 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN T
		RETURN 0;
	END CATCH	
END