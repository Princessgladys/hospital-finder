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
	@LocationAddress NVARCHAR(128),
	@StreetAddress NVARCHAR(128),
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
	@AvgCuringTime INT,
	@SpecialityList NVARCHAR(4000),
	@ServiceList NVARCHAR(4000),
	@FacilityList NVARCHAR(4000)
AS
BEGIN
	BEGIN TRANSACTION T
	BEGIN TRY
	-- INSERT TO HOSPITAL TABLE
	BEGIN
		DECLARE @FullAddress NVARCHAR(512)
		DECLARE @CityName NVARCHAR(128)
		DECLARE @DistrictName NVARCHAR(128)
		DECLARE @WardName NVARCHAR(128)

		SET @CityName = (SELECT City_Name
						 FROM City
						 WHERE City_ID = @CityID)

		SET @DistrictName = (SELECT [Type] + ' ' + District_Name
							 FROM District
							 WHERE District_ID = @DistrictID)

		SET @WardName = (SELECT [Type] + ' ' + Ward_Name
						 FROM Ward
						 WHERE Ward_ID = @WardID)
		
		SET @FullAddress = @LocationAddress + ' ' + @StreetAddress + ', ' +
			@WardName + ', ' + @DistrictName + ', ' + @CityName

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
			Avg_Curing_Time,
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
			@AvgCuringTime,
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
		IF (@SpecialityList != '')
		BEGIN
			DECLARE @RowNumber INT = 1
			DECLARE @TotalToken  INT = 0
			DECLARE @Token NVARCHAR(MAX)		
			DECLARE @SpecialityId INT = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, ',') TokenList)

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

					IF @@ROWCOUNT = 0
					BEGIN
						ROLLBACK TRAN T;
						RETURN 0;
					END
				END

				SET @RowNumber += 1
			END
		END

		-- INSERT TO SERVCE TABLE
		IF (@ServiceList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			DECLARE @ServiceId INT = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, ',') TokenList)

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

					IF @@ROWCOUNT = 0
					BEGIN
						ROLLBACK TRAN T;
						RETURN 0;
					END
				END			

				SET @RowNumber += 1
			END
		END

		-- INSERT TO FACILITY TABLE
		IF (@FacilityList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			DECLARE @FacilityId INT

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, ',') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
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

					IF @@ROWCOUNT = 0
					BEGIN
						ROLLBACK TRAN T;
						RETURN 0;
					END
				END

				SET @RowNumber += 1
			END
		END

		-- INSERT TO WORD_DICTIONARY TABLE
		DECLARE @WordID INT = 0

		IF (EXISTS(SELECT Word_ID
				   FROM WordDictionary
				   WHERE LOWER(Word) = LOWER(@HospitalName)))
		BEGIN
			SET @WordID = (SELECT Word_ID
						   FROM WordDictionary
						   WHERE LOWER(Word) = LOWER(@HospitalName))

			INSERT INTO Word_Hospital
			VALUES(@WordID, @HospitalID)
		END
		ELSE
		BEGIN
			INSERT INTO WordDictionary
			VALUES(@HospitalName, 3)

			SET @WordID = (SELECT TOP 1 Word_ID
						   FROM WordDictionary
						   ORDER BY Word_ID DESC)

			INSERT INTO Word_Hospital
			VALUES(@WordID, @HospitalID)
		END

		-- CHECK IF THERE IS ADDITIONAL TAG WORDS
		IF (@TagInput != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT LTRIM(TokenList.Token)
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)

				SET @WordID = (SELECT TOP 1 Word_ID
							   FROM WordDictionary
							   ORDER BY Word_ID DESC)

				IF (EXISTS(SELECT Word_ID
						   FROM WordDictionary
						   WHERE LOWER(Word) = LOWER(@Token)))
				BEGIN
					SET @WordID = (SELECT Word_ID
								   FROM WordDictionary
								   WHERE LOWER(Word) = LOWER(@Token))
				END
				ELSE
				BEGIN
					INSERT INTO WordDictionary
					VALUES(@Token, 3)

					IF @@ROWCOUNT = 0
					BEGIN
						ROLLBACK TRAN T;
						RETURN 0;
					END

					SET @WordID = (SELECT TOP 1 Word_ID
								   FROM WordDictionary
								   ORDER BY Word_ID DESC)
				END

				-- INSERT TO WORD_HOSPITAL TABLE
				INSERT INTO Word_Hospital
				VALUES(@WordID, @HospitalID)

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
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN T
		RETURN 0;
	END CATCH
END