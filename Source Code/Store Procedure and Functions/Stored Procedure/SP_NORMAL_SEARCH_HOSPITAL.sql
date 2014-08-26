-- SCRIPT TO SEARCH HOSPITALS
-- USING NORMAL OPTION
-- SONNX
IF OBJECT_ID('[SP_NORMAL_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
	@WhatPhrase NVARCHAR(128),
	@CityID INT,
	@DistrictID INT
AS
BEGIN
	-- DEFINE @WherePhrase
	DECLARE @WherePhrase INT
	IF (@CityID != 0 OR @DistrictID != 0)
		SET @WherePhrase = 1
	ELSE
		SET @WherePhrase = 0

	-- DEFINE INDEX NUMBER FOR SORT ORDER
	DECLARE @ExactlyIndexOfTag INT = 1
	DECLARE @ExactlyIndexOfSpeciality INT = 2
	DECLARE @ExactlyIndexOfDisease INT = 3

	DECLARE @RelativeIndexOfTag INT = 8
	DECLARE @RelativeIndexOfSpeciality INT = 9
	DECLARE @RelativeIndexOfDisease INT = 10
	
	DECLARE @NonDiacriticIndexOfTag INT = 11
	DECLARE @NonDiacriticIndexOfSpeciality INT = 12
	DECLARE @NonDiacriticIndexOfDisease INT = 13

	-- DEFINE PRIORITY OF SORT ORDER
	DECLARE @ExactlyPriorityOfCity INT = 10000
	DECLARE @ExactlyPriorityOfDistrict INT = 10000

	DECLARE @PriorityOfRatingPoint INT = 1000
	DECLARE @PriorityOfRatingCount INT = 10

	-- DEFINE SUPPORT VARIABLES
	DECLARE @RowNum INT = 1
	DECLARE @TempHospitalID INT = 0
	DECLARE @TempMatchedIndex INT = 0
	DECLARE @RatingPoint FLOAT
	DECLARE @RatingCount INT
	DECLARE @NonDiacriticWhatPhrase NVARCHAR(4000) =
			[dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (@WhatPhrase)

	-- DEFINE TEMPORARY TABLE THAT CONTAIN LIST OF HOSPITALS
	-- THAT CONTAINS MATCHED RESULT INDEX
	DECLARE @TempHospitalList TABLE(ID INT IDENTITY(1,1) PRIMARY KEY,
									Hospital_ID INT,
									[Index] INT)

	-- CHECK IF WHAT PHRASE IS NOT NULL
	IF (@WhatPhrase != '')
	BEGIN
-- QUERY FROM TAG TABLE-----------------------------------------------------------------------
	
		-- FIND EXACTLY MATCHING TAGS WORD
		DECLARE @NumOfHospitalFoundByExactlyTag INT = 0
		SET @NumOfHospitalFoundByExactlyTag = (SELECT COUNT (Word_ID)
											   FROM Tag
											   WHERE (N'%' + Word + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
													  N'%' + @WhatPhrase + N'%' LIKE N'%' + Word + N'%') AND
													 [Type] = 3)

		IF (@NumOfHospitalFoundByExactlyTag > 0)
		BEGIN
			INSERT INTO @TempHospitalList (Hospital_ID, [Index])
			SELECT DISTINCT wh.Hospital_ID, @ExactlyIndexOfTag
			FROM Tag w, Tag_Hospital wh
			WHERE (N'%' + Word + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
				   N'%' + @WhatPhrase + N'%' LIKE N'%' + Word + N'%') AND
				  w.[Type] = 3 AND
				  w.Word_ID = wh.Word_ID
		END
		-- FIND RELATIVE MATCHING TAGS WORD
		ELSE
		BEGIN
			DECLARE @NumOfHospitalFoundByRelativeTag INT = 0

			-- DIACRITIC VIETNAMSESE
			SET @NumOfHospitalFoundByRelativeTag = 
				(SELECT DISTINCT  COUNT(wh.Hospital_ID)
				 FROM Tag w, Tag_Hospital wh
				 WHERE w.[Type] = 3 AND
					   FREETEXT (w.Word, @WhatPhrase) AND
					   w.Word_ID = wh.Word_ID)

			IF (@NumOfHospitalFoundByRelativeTag > 0)
			BEGIN
				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT wh.Hospital_ID, @RelativeIndexOfTag
				FROM Tag w, Tag_Hospital wh
				WHERE w.[Type] = 3 AND
					  FREETEXT (w.Word,  @WhatPhrase) AND
					  w.Word_ID = wh.Word_ID
			END
			-- NON-DIACRITIC VIETNAMESE
			ELSE
			BEGIN
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT wh.Hospital_ID, @NonDiacriticIndexOfTag
				FROM Tag w, Tag_Hospital wh
				WHERE (N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](w.Word) + N'%' LIKE
						N'%' + @NonDiacriticWhatPhrase + N'%' OR
						N'%' + @NonDiacriticWhatPhrase + N'%' LIKE
						N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](w.Word) + N'%') AND
						[Type] = 3 AND
						w.Word_ID = wh.Word_ID
			END
		END

-- REMOVE NOISE WORD FROM WHAT PHRASE---------------------------------------------------------
		
		SET @WhatPhrase = REPLACE(@WhatPhrase, N'bệnh viện', N'')
		SET @WhatPhrase = REPLACE(@WhatPhrase, N'bệnh', N'')
		SET @WhatPhrase = REPLACE(REPLACE(REPLACE(@WhatPhrase,' ','<>'),'><',''),'<>',' ')
		SET @NonDiacriticWhatPhrase = REPLACE(@NonDiacriticWhatPhrase, N'benh vien', N'')
		SET @NonDiacriticWhatPhrase = REPLACE(@NonDiacriticWhatPhrase, N'benh', N'')
		SET @NonDiacriticWhatPhrase = REPLACE(REPLACE(REPLACE(@NonDiacriticWhatPhrase,' ','<>'),'><',''),'<>',' ')

-- QUERY FROM SPECIALITY TABLE----------------------------------------------------------------

		-- FIND EXACTLY SPECIALITY
		DECLARE @NumOfHospitalFoundByExactlySpeciality INT = 0
		SET @NumOfHospitalFoundByExactlySpeciality = (SELECT COUNT(Speciality_ID)
													  FROM Speciality
													  WHERE N'%' + @WhatPhrase + N'%' LIKE N'%' + Speciality_Name + N'%' OR
															N'%' + Speciality_Name + N'%' LIKE N'%' + @WhatPhrase + N'%')

		IF (@NumOfHospitalFoundByExactlySpeciality > 0)
		BEGIN
			-- INSERT TO @TempHospitalList
			INSERT INTO @TempHospitalList (Hospital_ID, [Index])
			SELECT DISTINCT h.Hospital_ID, @ExactlyIndexOfSpeciality
			FROM Speciality s, Hospital h, Hospital_Speciality hs
			WHERE (N'%' + @WhatPhrase + N'%' LIKE N'%' + Speciality_Name + N'%' OR
				   N'%' + Speciality_Name + N'%' LIKE N'%' + @WhatPhrase + N'%') AND
				  s.Speciality_ID = hs.Speciality_ID AND
				  h.Hospital_ID = hs.Hospital_ID
		END
		-- FIND RELATIVE MATCHING SPECIALITY
		ELSE
		BEGIN
			DECLARE @NumOfHospitalFoundByRelativeSpeciality INT = 0

			-- DIACRITIC VIETNAMSESE
			SET @NumOfHospitalFoundByRelativeSpeciality = 
					(SELECT COUNT(s.Speciality_ID)
					 FROM Speciality s, Hospital h, Hospital_Speciality hs
					 WHERE FREETEXT (Speciality_Name, @WhatPhrase) AND
						   s.Speciality_ID = hs.Speciality_ID AND
						   h.Hospital_ID = hs.Hospital_ID)

			IF (@NumOfHospitalFoundByRelativeSpeciality > 0)
			BEGIN
				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @RelativeIndexOfSpeciality
				FROM Speciality s, Hospital h, Hospital_Speciality hs
				WHERE FREETEXT (Speciality_Name, @WhatPhrase) AND
					  s.Speciality_ID = hs.Speciality_ID AND
					  h.Hospital_ID = hs.Hospital_ID
			END
			-- NON-DIACRITIC VIETNAMESE
			ELSE
			BEGIN
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @NonDiacriticIndexOfSpeciality
				FROM Speciality s, Hospital h, Hospital_Speciality hs
				WHERE (N'%' + @NonDiacriticWhatPhrase + N'%' LIKE 
					   N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) + N'%' OR
					   N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) + N'%' LIKE
					   N'%' + @NonDiacriticWhatPhrase + N'%') AND
					  s.Speciality_ID = hs.Speciality_ID AND
					  h.Hospital_ID = hs.Hospital_ID
			END
		END

-- QUERY FROM DISEASE TABLE-------------------------------------------------------------------

		-- FIND EXACTLY DISEASE
		DECLARE @NumOfHospitalFoundByExactlyDesease INT = 0
		SET @NumOfHospitalFoundByExactlyDesease = (SELECT COUNT(Disease_ID)
												   FROM Disease
												   WHERE N'%' + @WhatPhrase + N'%' LIKE N'%' + Disease_Name + N'%' OR
														 N'%' + Disease_Name + N'%' LIKE N'%' + @WhatPhrase + N'%')

		IF (@NumOfHospitalFoundByExactlyDesease > 0)
		BEGIN
			-- INSERT TO @TempHospitalList
			INSERT INTO @TempHospitalList (Hospital_ID, [Index])
			SELECT DISTINCT h.Hospital_ID, @ExactlyIndexOfDisease
			FROM Disease d, Speciality_Disease sd,
				 Hospital h, Hospital_Speciality hs
			WHERE (N'%' + @WhatPhrase + N'%' LIKE N'%' + Disease_Name + N'%' OR
				   N'%' + Disease_Name + N'%' LIKE N'%' + @WhatPhrase + N'%') AND
				  d.Disease_ID = sd.Disease_ID AND
				  sd.Speciality_ID = hs.Speciality_ID AND
				  h.Hospital_ID = hs.Hospital_ID
		END
		-- FIND RELATIVE MATCHING DISEASE
		ELSE
		BEGIN
			DECLARE @NumOfHospitalFoundByRelativeDisease INT = 0
		
			-- DIACRITIC VIETNAMSESE
			SET @NumOfHospitalFoundByRelativeDisease = 
					(SELECT COUNT(d.Disease_ID)
					 FROM Disease d, Speciality_Disease sd,
						  Hospital h, Hospital_Speciality hs
					 WHERE FREETEXT (Disease_Name, @WhatPhrase) AND
						   d.Disease_ID = sd.Disease_ID AND
						   sd.Speciality_ID = hs.Speciality_ID AND
						   h.Hospital_ID = hs.Hospital_ID)

			IF (@NumOfHospitalFoundByRelativeDisease > 0)
			BEGIN
				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @RelativeIndexOfDisease 
				FROM Disease d, Speciality_Disease sd,
					 Hospital h, Hospital_Speciality hs
				WHERE FREETEXT(Disease_Name, @WhatPhrase) AND
					  d.Disease_ID = sd.Disease_ID AND
					  sd.Speciality_ID = hs.Speciality_ID AND
					  h.Hospital_ID = hs.Hospital_ID
			END
			-- NON-DIACRITIC VIETNAMESE
			ELSE
			BEGIN
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @NonDiacriticIndexOfDisease
				FROM Disease d, Speciality_Disease sd,
						Hospital h, Hospital_Speciality hs
				WHERE (N'%' + @NonDiacriticWhatPhrase + N'%' LIKE 
					   N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Disease_Name) + N'%' OR
					   N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Disease_Name) + N'%' LIKE
					   N'%' + @NonDiacriticWhatPhrase + N'%') AND
					  d.Disease_ID = sd.Disease_ID AND
					  sd.Speciality_ID = hs.Speciality_ID AND
					  h.Hospital_ID = hs.Hospital_ID
			END
		END
	END
----------------------------------------------------------------------------------------------

	-- DEFINE TEMPORARY TABLE THAT CONTAIN LIST OF HOSPITALS
	-- THAT CONTAIN SORT PRIORITY
	DECLARE @ResultHospitalList TABLE(ID INT IDENTITY(1,1) PRIMARY KEY,
										Hospital_ID INT,
										[Priority] INT)

	-- CHECK IF THERE IS AT LEAST ONE RECORD IN @TempHospitalList
	DECLARE @NumOfRecordInTemp INT = 0;
	SELECT @NumOfRecordInTemp = (SELECT COUNT(*)
								 FROM (SELECT Hospital_ID
										FROM @TempHospitalList) t)
	IF (@NumOfRecordInTemp > 0)
	BEGIN
		SET @RowNum = 1
		WHILE (@RowNum <= @NumOfRecordInTemp)
		BEGIN
			SET @TempMatchedIndex = (SELECT h.[Index]
									 FROM (SELECT ROW_NUMBER()
										   OVER (ORDER BY t.Hospital_ID ASC) AS RowNumber, t.[Index]
										   FROM @TempHospitalList t) AS h
									 WHERE RowNumber = @RowNum)

			SELECT @TempHospitalID = (SELECT h.Hospital_ID
									  FROM (SELECT ROW_NUMBER()
											OVER (ORDER BY t.Hospital_ID ASC) AS RowNumber, t.Hospital_ID
											FROM @TempHospitalList t) AS h
									  WHERE RowNumber = @RowNum)

			-- CHECK IF HOSPITAL ID HAS BEEN EXISTED IN @ResultHospitalList
			IF (NOT EXISTS (SELECT r.Hospital_ID
							FROM @ResultHospitalList r
							WHERE r.Hospital_ID = @TempHospitalID))
			BEGIN
				-- TAKE RATING POINT
				SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

				-- TAKE RATING COUNT NUMBER
				SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

				INSERT INTO @ResultHospitalList
				SELECT @TempHospitalID,
							[dbo].[RETURN_SORT_PRIORITY] (@TempHospitalID, @TempMatchedIndex, @WhatPhrase) +
							CONVERT(INT, @RatingPoint * @PriorityOfRatingPoint) +
							CONVERT(INT, @RatingCount * @PriorityOfRatingCount)
			END
			ELSE
			BEGIN
				-- UPDATE VALUE OF PRIORITY
				UPDATE @ResultHospitalList
				SET [Priority] = (SELECT [Priority]
								  FROM @ResultHospitalList
								  WHERE Hospital_ID = @TempHospitalID) +
								 [dbo].[RETURN_SORT_PRIORITY] (@TempHospitalID, @TempMatchedIndex, @WhatPhrase)
				WHERE Hospital_ID = @TempHospitalID
			END

			SET @RowNum += 1
		END
	END

	-- CHECK IF WHERE PHRASE IS AVAILABLE
	IF (@WherePhrase != 0)
	BEGIN
		IF (@NumOfRecordInTemp > 0)
		BEGIN
			-- CHECK IF BOTH CITY AND DISTRICT ARE NOT NULL
			IF (@CityID != 0 AND @DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h, @ResultHospitalList temp
				WHERE h.Hospital_ID = temp.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY temp.[Priority] DESC
				RETURN;
			END

			-- CHECK IF CITY IS NOT NULL
			IF (@CityID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h, @ResultHospitalList temp
				WHERE h.Hospital_ID = temp.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.Is_Active = 'True'
				ORDER BY temp.[Priority] DESC
				RETURN;
			END

			-- CHECK IF DISTRICT IS NOT NULL
			IF (@DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h, @ResultHospitalList temp
				WHERE h.Hospital_ID = temp.Hospital_ID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY temp.[Priority] DESC
				RETURN;
			END
		END
		ELSE
		BEGIN
			-- CHECK IF BOTH CITY AND DISTRICT ARE NOT NULL
			IF (@CityID != 0 AND @DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h
				WHERE h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY [dbo].[FU_TAKE_RATING_POINT] (h.Hospital_ID) +
						 [dbo].[FU_TAKE_RATING_COUNT] (h.Hospital_ID) +
						 @ExactlyPriorityOfCity + @ExactlyPriorityOfDistrict DESC
				RETURN;
			END

			-- CHECK IF CITY IS NOT NULL
			IF (@CityID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h
				WHERE h.City_ID = @CityID AND
					  h.Is_Active = 'True'
				ORDER BY [dbo].[FU_TAKE_RATING_POINT] (h.Hospital_ID) +
						 [dbo].[FU_TAKE_RATING_COUNT] (h.Hospital_ID) +
						 @ExactlyPriorityOfCity DESC
				RETURN;
			END

			-- CHECK IF DISTRICT IS NOT NLL
			IF (@DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h
				WHERE h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY [dbo].[FU_TAKE_RATING_POINT] (h.Hospital_ID) +
						 [dbo].[FU_TAKE_RATING_COUNT] (h.Hospital_ID) +
						 @ExactlyPriorityOfDistrict DESC
				RETURN;
			END
		END
	END
	-- THERE IS NO WHERE PHRASE
	ELSE
	BEGIN
		IF (@NumOfRecordInTemp > 0)
		BEGIN
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
				   h.Ordinary_End_Time, h.Coordinate, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
				   h.Rating
			FROM Hospital h, @ResultHospitalList temp
			WHERE h.Hospital_ID = temp.Hospital_ID AND
				  h.Is_Active = 'True'
			ORDER BY temp.[Priority] DESC
			RETURN;
		END
		ELSE
		BEGIN
			RETURN;
		END		
	END
END