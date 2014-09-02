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
	DECLARE @PriorityOfRatingCount INT = 100

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
	-- REMOVE NOISE WORD FROM WHAT PHRASE---------------------------------------------------------
		
	SET @WhatPhrase = REPLACE(@WhatPhrase, N'bệnh viện', N'')
	SET @WhatPhrase = REPLACE(@WhatPhrase, N'bệnh', N'')
	SET @WhatPhrase = REPLACE(@WhatPhrase, N'khám', N'')
	SET @WhatPhrase = REPLACE(@WhatPhrase, N'chữa', N'')
	SET @WhatPhrase = REPLACE(REPLACE(REPLACE(@WhatPhrase,' ','<>'),'><',''),'<>',' ')
	SET @NonDiacriticWhatPhrase = REPLACE(@NonDiacriticWhatPhrase, N'benh vien', N'')
	SET @NonDiacriticWhatPhrase = REPLACE(@NonDiacriticWhatPhrase, N'benh', N'')
	SET @NonDiacriticWhatPhrase = REPLACE(@NonDiacriticWhatPhrase, N'kham', N'')
	SET @NonDiacriticWhatPhrase = REPLACE(REPLACE(REPLACE(@NonDiacriticWhatPhrase,' ','<>'),'><',''),'<>',' ')

	-- QUERY FROM TAG TABLE-----------------------------------------------------------------------
	
		-- FIND EXACTLY MATCHING TAGS WORD
		DECLARE @NumOfHospitalFoundByExactlyTag INT = 0
		SET @NumOfHospitalFoundByExactlyTag = (SELECT COUNT (Word_ID)
											   FROM Tag
											   WHERE [Type] = 3 AND
													 (N'%' + Word + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
													  N'%' + @WhatPhrase + N'%' LIKE N'%' + Word + N'%'))

		IF (@NumOfHospitalFoundByExactlyTag > 0)
		BEGIN
			INSERT INTO @TempHospitalList (Hospital_ID, [Index])
			SELECT DISTINCT wh.Hospital_ID, @ExactlyIndexOfTag
			FROM Tag w, Tag_Hospital wh
			WHERE w.Word_ID = wh.Word_ID AND 
				  w.[Type] = 3 AND
				  (N'%' + Word + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
				   N'%' + @WhatPhrase + N'%' LIKE N'%' + Word + N'%')
		END
		-- FIND RELATIVE MATCHING TAGS WORD
		ELSE
		BEGIN
			DECLARE @NumOfHospitalFoundByRelativeTag INT = 0

			-- DIACRITIC VIETNAMSESE
			SET @NumOfHospitalFoundByRelativeTag = 
				(SELECT DISTINCT  COUNT(wh.Hospital_ID)
				 FROM Tag w, Tag_Hospital wh
				 WHERE w.Word_ID = wh.Word_ID AND
					   w.[Type] = 3 AND
					   FREETEXT (w.Word, @WhatPhrase))

			IF (@NumOfHospitalFoundByRelativeTag > 0)
			BEGIN
				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT wh.Hospital_ID, @RelativeIndexOfTag
				FROM Tag w, Tag_Hospital wh
				WHERE w.Word_ID = wh.Word_ID AND 
					  w.[Type] = 3 AND
					  FREETEXT (w.Word,  @WhatPhrase)
					  
			END
			-- NON-DIACRITIC VIETNAMESE
			ELSE
			BEGIN
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT wh.Hospital_ID, @NonDiacriticIndexOfTag
				FROM [NON_DIACRITIC_TAG] w, Tag_Hospital wh
				WHERE w.Word_ID = wh.Word_ID AND
					  FREETEXT (w.Word, @WhatPhrase)		  
			END
		END

	-- QUERY FROM SPECIALITY TABLE----------------------------------------------------------------

		-- FIND EXACTLY SPECIALITY
		DECLARE @NumOfHospitalFoundByExactlySpeciality INT = 0
		SET @NumOfHospitalFoundByExactlySpeciality = (SELECT COUNT(Speciality_ID)
													  FROM Speciality
													  WHERE N'%' + Speciality_Name + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
															N'%' + @WhatPhrase + N'%' LIKE N'%' + Speciality_Name + N'%')

		IF (@NumOfHospitalFoundByExactlySpeciality > 0)
		BEGIN
			-- INSERT TO @TempHospitalList
			INSERT INTO @TempHospitalList (Hospital_ID, [Index])
			SELECT DISTINCT h.Hospital_ID, @ExactlyIndexOfSpeciality
			FROM Speciality s, Hospital h, Hospital_Speciality hs
			WHERE h.Hospital_ID = hs.Hospital_ID AND
				  hs.Speciality_ID = s.Speciality_ID AND
				  (N'%' + s.Speciality_Name + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
				   N'%' + @WhatPhrase + N'%' LIKE N'%' + s.Speciality_Name + N'%')
		END
		-- FIND RELATIVE MATCHING SPECIALITY
		ELSE
		BEGIN
			DECLARE @NumOfHospitalFoundByRelativeSpeciality INT = 0

			-- DIACRITIC VIETNAMSESE
			SET @NumOfHospitalFoundByRelativeSpeciality = 
					(SELECT COUNT(s.Speciality_ID)
					 FROM Speciality s, Hospital h, Hospital_Speciality hs
					 WHERE h.Hospital_ID = hs.Hospital_ID AND
						   hs.Speciality_ID = s.Speciality_ID AND 
						   FREETEXT (s.Speciality_Name, @WhatPhrase))

			IF (@NumOfHospitalFoundByRelativeSpeciality > 0)
			BEGIN
				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @RelativeIndexOfSpeciality
				FROM Speciality s, Hospital h, Hospital_Speciality hs
				WHERE h.Hospital_ID = hs.Hospital_ID AND
					  hs.Speciality_ID = s.Speciality_ID AND 
					  FREETEXT (s.Speciality_Name, @WhatPhrase)
			END
			-- NON-DIACRITIC VIETNAMESE
			ELSE
			BEGIN
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @NonDiacriticIndexOfSpeciality
				FROM [NON_DIACRITIC_SPECIALITY] s, Hospital h, Hospital_Speciality hs
				WHERE h.Hospital_ID = hs.Hospital_ID AND
					  hs.Speciality_ID = s.Speciality_ID AND
					  FREETEXT (s.Speciality_Name, @WhatPhrase)  
			END
		END

	-- QUERY FROM DISEASE TABLE-------------------------------------------------------------------

		-- FIND EXACTLY DISEASE
		DECLARE @NumOfHospitalFoundByExactlyDesease INT = 0
		SET @NumOfHospitalFoundByExactlyDesease = (SELECT COUNT(Disease_ID)
												   FROM Disease
												   WHERE N'%' + Disease_Name + N'%' LIKE N'%' + @WhatPhrase + N'%'OR
														 N'%' + @WhatPhrase + N'%' LIKE N'%' + Disease_Name + N'%')

		IF (@NumOfHospitalFoundByExactlyDesease > 0)
		BEGIN
			-- INSERT TO @TempHospitalList
			INSERT INTO @TempHospitalList (Hospital_ID, [Index])
			SELECT DISTINCT h.Hospital_ID, @ExactlyIndexOfDisease
			FROM Disease d, Speciality_Disease sd,
				 Hospital h, Hospital_Speciality hs, Speciality s
			WHERE h.Hospital_ID = hs.Hospital_ID AND
				  hs.Speciality_ID = s.Speciality_ID AND
				  s.Speciality_ID = sd.Speciality_ID AND
				  sd.Disease_ID = d.Disease_ID AND
				  (N'%' + Disease_Name + N'%' LIKE N'%' + @WhatPhrase + N'%' OR
				   N'%' + @WhatPhrase + N'%' LIKE N'%' + Disease_Name + N'%') 
		END
		-- FIND RELATIVE MATCHING DISEASE
		ELSE
		BEGIN
			DECLARE @NumOfHospitalFoundByRelativeDisease INT = 0
		
			-- DIACRITIC VIETNAMSESE
			SET @NumOfHospitalFoundByRelativeDisease = 
					(SELECT COUNT(d.Disease_ID)
					 FROM Disease d, Speciality_Disease sd,
						  Hospital h, Hospital_Speciality hs, Speciality s
					 WHERE h.Hospital_ID = hs.Hospital_ID AND
						   hs.Speciality_ID = s.Speciality_ID AND
						   s.Speciality_ID = sd.Speciality_ID AND
						   sd.Disease_ID = d.Disease_ID AND
						   FREETEXT (Disease_Name, @WhatPhrase))

			IF (@NumOfHospitalFoundByRelativeDisease > 0)
			BEGIN
				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @RelativeIndexOfDisease 
				FROM Disease d, Speciality_Disease sd,
					 Hospital h, Hospital_Speciality hs, Speciality s
				WHERE h.Hospital_ID = hs.Hospital_ID AND
					  hs.Speciality_ID = s.Speciality_ID AND
					  s.Speciality_ID = sd.Speciality_ID AND
					  sd.Disease_ID = d.Disease_ID AND
					  FREETEXT (Disease_Name, @WhatPhrase)
			END
			-- NON-DIACRITIC VIETNAMESE
			ELSE
			BEGIN
				INSERT INTO @TempHospitalList (Hospital_ID, [Index])
				SELECT DISTINCT h.Hospital_ID, @NonDiacriticIndexOfDisease
				FROM [NON_DIACRITIC_DISEASE] d, Speciality_Disease sd,
					 Hospital h, Hospital_Speciality hs, Speciality s
				WHERE h.Hospital_ID = hs.Hospital_ID AND
					  hs.Speciality_ID = s.Speciality_ID AND
					  s.Speciality_ID = sd.Speciality_ID AND
					  sd.Disease_ID = d.Disease_ID AND
					  FREETEXT(d.Disease_Name, @WhatPhrase)
			END
		END
	END
----------------------------------------------------------------------------------------------

	-- DEFINE TEMPORARY TABLE THAT CONTAIN LIST OF HOSPITALS
	-- THAT CONTAIN SORT PRIORITY
	DECLARE @ResultList TABLE(ID INT IDENTITY(1,1) PRIMARY KEY,
							  Hospital_ID INT,
							  [Priority] INT)

	---- CHECK IF THERE IS AT LEAST ONE RECORD IN @TempHospitalList
	DECLARE @NumOfRecordInTemp INT = 0;
	SELECT @NumOfRecordInTemp = (SELECT COUNT(Hospital_ID)
								 FROM @TempHospitalList)
	IF (@NumOfRecordInTemp > 0)
	BEGIN
		INSERT INTO @ResultList
		SELECT t.Hospital_ID,
			   t.[Priority] + CONVERT(INT, [dbo].[FU_TAKE_RATING_POINT] (t.Hospital_ID) * @PriorityOfRatingPoint) +
							  CONVERT(INT, [dbo].[FU_TAKE_RATING_COUNT] (t.Hospital_ID) * @PriorityOfRatingCount)
		FROM (SELECT t.Hospital_ID, 
					 SUM([dbo].[RETURN_SORT_PRIORITY]
						(t.Hospital_ID, t.[Index], @WhatPhrase, @NonDiacriticWhatPhrase)) AS [Priority]
			  FROM @TempHospitalList t
			  GROUP BY t.Hospital_ID) t
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
				FROM Hospital h, @ResultList temp
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
				FROM Hospital h, @ResultList temp
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
				FROM Hospital h, @ResultList temp
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
			FROM Hospital h, @ResultList temp
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