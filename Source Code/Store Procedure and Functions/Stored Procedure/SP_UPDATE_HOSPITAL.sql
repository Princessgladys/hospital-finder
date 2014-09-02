-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF AN USER HAVE BEEN EXISTED IN DATABSE
-- SONNX
IF OBJECT_ID('[SP_UPDATE_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_UPDATE_HOSPITAL
GO
CREATE PROCEDURE SP_UPDATE_HOSPITAL
	@HospitalID INT,
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
	@FullDescription NVARCHAR(MAX),
	@SpecialityList NVARCHAR(4000),
	@ServiceList NVARCHAR(4000),
	@FacilityList NVARCHAR(4000),
	@TagInput NVARCHAR(4000),
	@PhotoList NVARCHAR(MAX)
AS
BEGIN
	BEGIN TRANSACTION T
	BEGIN TRY
		DECLARE @HospitalNameWordID INT =
			(SELECT t.Word_ID
			 FROM Tag_Hospital th, Tag t
			 WHERE th.Hospital_ID = 90 AND
				   t.Word = N'Nhà tui')

		-- UPDATE HOSPITAL TABLE
		BEGIN
			DECLARE @OldHospitalName NVARCHAR(128) =
				(SELECT Hospital_Name
				 FROM Hospital
				 WHERE Hospital_ID = @HospitalID)

			IF (@OldHospitalName != @HospitalName)
			BEGIN
				UPDATE Tag
				SET Word = @HospitalName
				WHERE Word_ID = @HospitalNameWordID
			END

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
			WHERE Hospital_ID = @HospitalID

			IF @@ROWCOUNT = 0
			BEGIN
				ROLLBACK TRAN T;
				RETURN 0;
			END
		END

		-- UPDATE SPECIALITY TABLE
		IF (@SpecialityList != '')
		BEGIN
			DECLARE @RowNumber INT = 1
			DECLARE @TotalToken INT = 0
			DECLARE @Token NVARCHAR(256)
			DECLARE @TotalSpeciality INT = 0
			DECLARE @TempSpecialityID INT = 0
			DECLARE @TempSpecialityName NVARCHAR(256) = N''

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList)

			SELECT @TotalSpeciality = (SELECT COUNT(Hospital_ID)
									   FROM Hospital_Speciality
									   WHERE Hospital_ID = @HospitalID)

			-- UPDATE IS_ACTIVE STATUS FROM 'TRUE' TO 'FALSE'
			IF (@TotalSpeciality > @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalSpeciality)
				BEGIN
					SELECT @TempSpecialityID = (SELECT hs.Speciality_ID
											   FROM (SELECT ROW_NUMBER()
													 OVER (ORDER BY hs.Speciality_ID ASC) AS RowNumber, hs.Speciality_ID
													 FROM Hospital_Speciality hs
													 WHERE  Hospital_ID = @HospitalID) AS hs
											   WHERE RowNumber = @RowNumber)
					SELECT @TempSpecialityName = (SELECT Speciality_Name
												  FROM Speciality
												  WHERE Speciality_ID = @TempSpecialityID)
					IF (NOT EXISTS(SELECT Token
								   FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|')
								   WHERE [dbo].[FU_REMOVE_WHITE_SPACE] (Token) = 
										 [dbo].[FU_REMOVE_WHITE_SPACE] (@TempSpecialityName)))
					BEGIN
						DELETE FROM Hospital_Speciality
						WHERE Speciality_ID = @TempSpecialityID AND
							  Hospital_ID = @HospitalID
					END

					SET @RowNumber += 1
				END
			END

			-- UPDATE IS_ACTIVE STATUS FROM 'FALSE' TO 'TRUE' AND MAPPING NEW SPECIALITY
			IF (@TotalSpeciality < @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT TokenList.Token
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										   FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					-- CHECK STATUS OF SPECIALITY IN HOSPITAL
					IF (NOT EXISTS(SELECT hs.Hospital_ID
									FROM Hospital_Speciality hs
									WHERE hs.Speciality_ID = @Token AND
											hs.Hospital_ID = @HospitalID))
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
		END
		ELSE
		BEGIN
			DELETE FROM Hospital_Speciality
			WHERE Hospital_ID = @HospitalID
		END

		-- UPDATE SERVICE TABLE
		IF (@ServiceList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			SET @Token = N''
			DECLARE @TotalService INT = 0
			DECLARE @TempServiceID INT = 0
			DECLARE @TempServiceName NVARCHAR(256) = N''

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList)

			SELECT @TotalService = (SELECT COUNT(Hospital_ID)
									FROM Hospital_Service
									WHERE Hospital_ID = @HospitalID)

			-- UPDATE IS_ACTIVE STATUS FROM 'TRUE' TO 'FALSE'
			IF (@TotalService > @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalService)
				BEGIN
					SELECT @TempServiceID = (SELECT hs.Service_ID
											 FROM (SELECT ROW_NUMBER()
												   OVER (ORDER BY hs.Service_ID ASC) AS RowNumber, hs.Service_ID
												   FROM Hospital_Service hs
												   WHERE  Hospital_ID = @HospitalID) AS hs
											 WHERE RowNumber = @RowNumber)
					SELECT @TempServiceName = (SELECT [Service_Name]
											   FROM [Service]
											   WHERE Service_ID = @TempServiceID)
					IF (NOT EXISTS(SELECT Token
								   FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|')
								   WHERE [dbo].[FU_REMOVE_WHITE_SPACE] (Token) = 
										 [dbo].[FU_REMOVE_WHITE_SPACE] (@TempServiceName)))
					BEGIN
						DELETE FROM Hospital_Service
						WHERE Service_ID = @TempServiceID AND
							  Hospital_ID = @HospitalID
					END

					SET @RowNumber += 1
				END
			END

			-- UPDATE IS_ACTIVE STATUS FROM 'FALSE' TO 'TRUE' AND MAPPING NEW SERVICE
			IF (@TotalService < @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT TokenList.Token
									 FROM (SELECT ROW_NUMBER()
										 OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										 FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					-- CHECK STATUS OF SERVICE IN HOSPITAL
					IF (NOT EXISTS(SELECT hs.Hospital_ID
								FROM Hospital_Service hs
								WHERE hs.Service_ID = @Token AND
										hs.Hospital_ID = @HospitalID))
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
		END
		ELSE
		BEGIN
			DELETE FROM Hospital_Service
			WHERE Hospital_ID = @HospitalID
		END

		-- UPDATE FACILITY TABLE
		IF (@FacilityList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			SET @Token = N''
			DECLARE @TotalFacility INT = 0
			DECLARE @TempFacilityID INT = 0
			DECLARE @TempFacilityName NVARCHAR(256) = N''

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList)

			SELECT @TotalFacility = (SELECT COUNT(Hospital_ID)
									 FROM Hospital_Facility
									 WHERE Hospital_ID = @HospitalID)

			-- UPDATE IS_ACTIVE STATUS FROM 'TRUE' TO 'FALSE'
			IF (@TotalFacility > @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalFacility)
				BEGIN
					SELECT @TempFacilityID = (SELECT hf.Facility_ID
											  FROM (SELECT ROW_NUMBER()
													OVER (ORDER BY hf.Facility_ID ASC) AS RowNumber, hf.Facility_ID
													FROM Hospital_Facility hf
													WHERE  Hospital_ID = @HospitalID) AS hf
											  WHERE RowNumber = @RowNumber)
					SELECT @TempFacilityName = (SELECT Facility_Name
												FROM Facility
												WHERE Facility_ID = @TempFacilityID)
					IF (NOT EXISTS(SELECT Token
								   FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|')
								   WHERE [dbo].[FU_REMOVE_WHITE_SPACE] (Token) = 
										 [dbo].[FU_REMOVE_WHITE_SPACE] (@TempFacilityName)))
					BEGIN
						DELETE FROM Hospital_Facility
						WHERE Facility_ID = @TempFacilityID AND
							  Hospital_ID = @HospitalID
					END

					SET @RowNumber += 1
				END
			END

			-- UPDATE IS_ACTIVE STATUS FROM 'FALSE' TO 'TRUE' AND MAPPING NEW FACILITY
			IF (@TotalFacility < @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT TokenList.Token
									 FROM (SELECT ROW_NUMBER()
										 OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										 FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					-- CHECK STATUS OF SERVICE IN HOSPITAL
					IF (NOT EXISTS(SELECT hs.Hospital_ID
								FROM Hospital_Facility hs
								WHERE hs.Facility_ID = @Token AND
										hs.Hospital_ID = @HospitalID))
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
		END
		ELSE
		BEGIN
			DELETE FROM Hospital_Facility
			WHERE Hospital_ID = @HospitalID
		END

		-- UPDATE TAG TABLE
		IF (@TagInput != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0
			SET @Token = N''
			DECLARE @TotalTag INT = 0
			DECLARE @TempWordID INT = 0
			DECLARE @TempWord NVARCHAR(256) = N''

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',') TokenList)

			SELECT @TotalTag = (SELECT COUNT(Hospital_ID)
								FROM Tag_Hospital
								WHERE Hospital_ID = @HospitalID)

			-- UPDATE IS_ACTIVE STATUS FROM 'TRUE' TO 'FALSE'
			IF (@TotalTag > @TotalToken)
			BEGIN
				SET @RowNumber = 1
				WHILE (@RowNumber <= @TotalTag)
				BEGIN
					SELECT @TempWordID = (SELECT th.Word_ID
										  FROM (SELECT ROW_NUMBER()
												OVER (ORDER BY th.Word_ID ASC) AS RowNumber, th.Word_ID
												FROM Tag_Hospital th
												WHERE  Hospital_ID = @HospitalID) AS th
										  WHERE RowNumber = @RowNumber)
					SELECT @TempWord = (SELECT Word
										FROM Tag
										WHERE Word_ID = @TempWordID)
					IF (NOT EXISTS(SELECT Token
								   FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',')
								   WHERE [dbo].[FU_REMOVE_WHITE_SPACE] (Token) = 
										 [dbo].[FU_REMOVE_WHITE_SPACE] (@TempWord)))
					BEGIN
						DELETE FROM Tag_Hospital
						WHERE Hospital_ID = @HospitalID AND
							  Word_ID = @TempWordID
					END

					SET @RowNumber += 1
				END
			END

			-- UPDATE IS_ACTIVE STATUS FROM 'FALSE' TO 'TRUE' AND MAPPING NEW TAG
			IF (@TotalTag < @TotalToken)
			BEGIN
				SET @RowNumber = 1
				SET @TempWordID = 0
				WHILE (@RowNumber <= @TotalToken)
				BEGIN
					SELECT @Token = (SELECT TokenList.Token
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
										   FROM [dbo].[FU_STRING_TOKENIZE] (@TagInput, ',') TokenList) AS TokenList
									 WHERE RowNumber = @RowNumber)

					IF (NOT EXISTS(SELECT Word_ID
								   FROM Tag
								   WHERE LOWER(Word) = LOWER(@Token)))
					BEGIN
						INSERT INTO Tag
						VALUES(@Token, 3)

						INSERT INTO Tag_Hospital
						VALUES ((SELECT TOP 1 Word_ID
								 FROM Tag
								 WHERE Word = @Token), @HospitalID)
					END
					ELSE
					BEGIN
						IF (NOT EXISTS(SELECT *
							   FROM Tag_Hospital
							   WHERE Hospital_ID = @HospitalID AND
									 Word_ID = (SELECT Word_ID
												FROM Tag
												WHERE LOWER(Word) = LOWER(@Token))))
						BEGIN
							INSERT INTO Tag_Hospital
							VALUES ((SELECT Word_ID
									 FROM Tag
									 WHERE LOWER(Word) = LOWER(@Token)), @HospitalID)
						END
					END

					SET @RowNumber += 1
				END
			END
		END
		ELSE
		BEGIN
			DELETE FROM Tag_Hospital
			WHERE Hospital_ID = @HospitalID AND
				  Word_ID != @HospitalNameWordID
		END

		-- UPDATE PHOTO TABLE
		IF (@PhotoList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@PhotoList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@PhotoList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)

				INSERT INTO Photo
				(
					File_Path,
					Add_Date,
					Hospital_ID,
					Uploaded_Person,
					Is_Active
				)
				VALUES
				(
					@Token,
					GETDATE(),
					@HospitalID,
					@CreatedPerson,
					'True'
				)

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