-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF AN USER HAVE BEEN EXISTED IN DATABSE
-- SONNX
IF OBJECT_ID('[SP_INSERT_HOSPITAL_EXCEL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_INSERT_HOSPITAL_EXCEL
GO
CREATE PROCEDURE SP_INSERT_HOSPITAL_EXCEL
	@HospitalName NVARCHAR(64),
	@HospitalType INT,
	@FullAddress NVARCHAR(256),
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
	@TagInput NVARCHAR(4000),
	@SpecialityList NVARCHAR(4000),
	@ServiceList NVARCHAR(4000),
	@FacilityList NVARCHAR(4000)
AS
BEGIN
	BEGIN TRANSACTION T
	BEGIN TRY
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
			Coordinate,
			Created_Person,
			Is_Active
		)
		VALUES
		(
			@HospitalName,
			@HospitalType,
			@FullAddress,
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
			@Coordinate,
			@CreatedPerson,
			'True'
		)

		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN T;
			RETURN 0;
		END

		-- SELECT HOSPITAL ID
		DECLARE @HospitalID INT = (SELECT TOP 1 Hospital_ID
								   FROM Hospital
								   ORDER BY Hospital_ID DESC)

		-- INSERT TO HOSPITAL_SPECIALITY TABLE
		IF (@SpecialityList IS NOT NULL AND @SpecialityList != '')
		BEGIN
			DECLARE @RowNumber INT = 1
			DECLARE @TotalToken  INT = 0
			DECLARE @Token NVARCHAR(MAX)		
			DECLARE @SpecialityId INT = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, ',') TokenList)

			IF (@TotalToken = 0)
			BEGIN
				SET @SpecialityId = (SELECT Speciality_ID
									 FROM Speciality
									 WHERE Speciality_Name = @SpecialityList)

				IF (@SpecialityId IS NOT NULL AND @SpecialityId != 0)
				BEGIN
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
						@SpecialityId,
						'False',
						'True'
					)
				END
			END
			ELSE
			BEGIN
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT LTRIM(TokenList.Token)
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										   FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, ',') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					SET @SpecialityId = (SELECT Speciality_ID
										 FROM Speciality
										 WHERE Speciality_Name = @Token)

					IF (@SpecialityId IS NOT NULL AND @SpecialityId != 0)
					BEGIN
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
							@SpecialityId,
							'False',
							'True'
						)
					END

					SET @RowNumber += 1
				END
			END
		END

		-- INSERT TO SERVCE TABLE
		IF (@ServiceList IS NOT NULL AND @ServiceList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			DECLARE @ServiceId INT = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, ',') TokenList)

			IF (@TotalToken = 0)
			BEGIN
				SET @ServiceId = (SELECT Service_ID
								  FROM [Service]
								  WHERE [Service_Name] = @ServiceList)

				IF (@ServiceId IS NOT NULL AND @ServiceId != 0)
				BEGIN
					INSERT INTO Hospital_Service
					(
						Hospital_ID,
						Service_ID,
						Is_Active
					)
					VALUES
					(
						@HospitalID,
						@ServiceId,
						'True'
					)
				END	
			END
			ELSE
			BEGIN
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT LTRIM(TokenList.Token)
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										   FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, ',') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					SET @ServiceId = (SELECT Service_ID
									  FROM [Service]
									  WHERE [Service_Name] = @Token)

					IF (@ServiceId IS NOT NULL AND @ServiceId != 0)
					BEGIN
						INSERT INTO Hospital_Service
						(
							Hospital_ID,
							Service_ID,
							Is_Active
						)
						VALUES
						(
							@HospitalID,
							@ServiceId,
							'True'
						)
					END			

					SET @RowNumber += 1
				END
			END
		END

		-- INSERT TO FACILITY TABLE
		IF (@FacilityList IS NOT NULL AND @FacilityList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			DECLARE @FacilityId INT

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, ',') TokenList)

			IF (@TotalToken = 0)
			BEGIN
				SET @FacilityId = (SELECT Facility_ID
								   FROM Facility
								   WHERE Facility_Name = @Token)

				IF (@FacilityId IS NOT NULL AND @FacilityId != 0)
				BEGIN
					INSERT INTO Hospital_Facility
					(
						Hospital_ID,
						Facility_ID,
						Is_Active
					)
					VALUES
					(
						@HospitalID,
						@FacilityId,
						'True'
					)
				END
			END
			ELSE
			BEGIN
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT LTRIM(TokenList.Token)
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										   FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, ',') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					SET @FacilityId = (SELECT Facility_ID
									   FROM Facility
									   WHERE Facility_Name = @Token)

					IF (@FacilityId IS NOT NULL AND @FacilityId != 0)
					BEGIN
						INSERT INTO Hospital_Facility
						(
							Hospital_ID,
							Facility_ID,
							Is_Active
						)
						VALUES
						(
							@HospitalID,
							@FacilityId,
							'True'
						)
					END

					SET @RowNumber += 1
				END
			END		
		END

		-- INSERT TO WORD_DICTIONARY TABLE
		DECLARE @WordID INT = 0

		IF (EXISTS(SELECT Word_ID
				   FROM Tag
				   WHERE LOWER(Word) = LOWER(@HospitalName)))
		BEGIN
			SET @WordID = (SELECT Word_ID
						   FROM Tag
						   WHERE LOWER(Word) = LOWER(@HospitalName))

			INSERT INTO Tag_Hospital
			VALUES(@WordID, @HospitalID)
		END
		ELSE
		BEGIN
			INSERT INTO Tag
			VALUES(@HospitalName, 3)

			SET @WordID = (SELECT TOP 1 Word_ID
						   FROM Tag
						   ORDER BY Word_ID DESC)

			INSERT INTO Tag_Hospital
			VALUES(@WordID, @HospitalID)
		END

		-- CHECK IF THERE IS ADDITIONAL TAG WORDS
		IF (@TagInput IS NOT NULL AND @TagInput != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',') TokenList)
			
			IF (@TotalToken = 0)
			BEGIN
				IF (NOT EXISTS(SELECT Word_ID
						   FROM Tag
						   WHERE LOWER(Word) = LOWER(@TagInput)))
				BEGIN
					INSERT INTO Tag
					VALUES(@TagInput, 3)

					SET @WordID = (SELECT TOP 1 Word_ID
								   FROM Tag
								   ORDER BY Word_ID DESC)

					-- INSERT TO WORD_HOSPITAL TABLE
					INSERT INTO Tag_Hospital
					VALUES(@WordID, @HospitalID)
				END
			END
			ELSE
			BEGIN
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT LTRIM(TokenList.Token)
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										   FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					-- CHECK IF TOKEN IS NULL
					IF (@Token IS NOT NULL)
					BEGIN
						SET @WordID = (SELECT TOP 1 Word_ID
									   FROM Tag
									   ORDER BY Word_ID DESC)

						IF (EXISTS(SELECT Word_ID
								   FROM Tag
								   WHERE LOWER(Word) = LOWER(@Token)))
						BEGIN
							SET @WordID = (SELECT Word_ID
										   FROM Tag
										   WHERE LOWER(Word) = LOWER(@Token))
						END
						ELSE
						BEGIN
							INSERT INTO Tag
							VALUES(@Token, 3)

							SET @WordID = (SELECT TOP 1 Word_ID
										   FROM Tag
										   ORDER BY Word_ID DESC)
						END

						-- INSERT TO WORD_HOSPITAL TABLE
						INSERT INTO Tag_Hospital
						VALUES(@WordID, @HospitalID)
					END

					SET @RowNumber += 1
				END
			END		
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