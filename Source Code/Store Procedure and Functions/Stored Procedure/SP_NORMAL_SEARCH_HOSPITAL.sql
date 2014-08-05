-- SCRIPT TO SEARCH HOSPITALS
-- USING NORMAL OPTION
-- SONNX
IF OBJECT_ID('[SP_NORMAL_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
	@WhatPhrase NVARCHAR(128), -- ALWAYS NOT NULL
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

	-- DEFINE PRIORITY OF SORT ORDER
	DECLARE @ExactlyPriorityOfTag INT = 10000
	DECLARE @ExactlyPriorityOfSpeciality INT = 10000
	DECLARE @ExactlyPriorityOfDisease INT = 10000
	DECLARE @ExactlyPriorityOfCity INT = 10000
	DECLARE @ExactlyPriorityOfDistrict INT = 10000

	DECLARE @PriorityOfRatingPoint INT = 1000
	DECLARE @PriorityOfRatingCount INT = 100

	DECLARE @RelativePriorityOfTag INT = 100
	DECLARE @RelativePriorityOfSpeciality INT = 100
	DECLARE @RelativePriorityOfDisease INT = 100
	
	DECLARE @NonDiacriticPriorityOfTag INT = 10

	-- DEFINE SUPPORT VARIABLES
	DECLARE @RowNum INT = 1
	DECLARE @TempHospitalID INT = 0
	DECLARE @RatingPoint FLOAT
	DECLARE @RatingCount INT
	DECLARE @NonDiacriticWhatPhrase NVARCHAR(4000) =
			[dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (@WhatPhrase)

	-- DEFINE TEMPORARY TABLE THAT CONTAIN LIST OF HOSPITALS
	DECLARE @TempHospitalList TABLE(ID INT IDENTITY(1,1) PRIMARY KEY,
									Hospital_ID INT,
									[Priorty] INT)
	
-- QUERY FROM TAG TABLE-----------------------------------------------------------------------
	
	-- FIND EXACTLY MATCHING TAGS WORD
	DECLARE @NumOfHospitalFoundByExactlyTag INT = 0
	SET @NumOfHospitalFoundByExactlyTag = (SELECT COUNT(*)
										   FROM (SELECT Word_ID
												 FROM WordDictionary
												 WHERE @WhatPhrase LIKE  N'%' + Word + N'%' AND
													   [Type] = 2) w)

	IF (@NumOfHospitalFoundByExactlyTag > 0)
	BEGIN
		SET @RowNum = 1
		WHILE (@RowNum <= @NumOfHospitalFoundByExactlyTag)
		BEGIN
			SET @TempHospitalID = (SELECT h.Hospital_ID
								   FROM (SELECT ROW_NUMBER()
										 OVER (ORDER BY wh.Hospital_ID ASC) AS RowNumber, wh.Hospital_ID
										 FROM WordDictionary w, Word_Hospital wh
										 WHERE @WhatPhrase LIKE  N'%' + Word + N'%' AND
											   w.[Type] = 2 AND
											   w.Word_ID = wh.Word_ID) AS h
								   WHERE RowNumber = @RowNum)

			-- TAKE RATING POINT
			SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

			-- TAKE RATING COUNT NUMBER
			SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

			-- INSERT TO @TempHospitalList
			INSERT INTO @TempHospitalList
			SELECT @TempHospitalID, @ExactlyPriorityOfTag +
						@RatingPoint * @PriorityOfRatingPoint +
						@RatingCount * @PriorityOfRatingCount

			SET @RowNum += 1
		END
	END
	-- FIND RELATIVE MATCHING TAGS WORD
	ELSE
	BEGIN
		DECLARE @NumOfHospitalFoundByRelativeTag INT = 0

		-- DIACRITIC VIETNAMSESE
		SET @NumOfHospitalFoundByRelativeTag = 
			(SELECT COUNT(*)
			 FROM (SELECT DISTINCT wh.Hospital_ID
				   FROM WordDictionary w, Word_Hospital wh
				   WHERE w.[Type] = 2 AND
						 FREETEXT (w.Word, @WhatPhrase) AND
						 w.Word_ID = wh.Word_ID) n)

		IF (@NumOfHospitalFoundByRelativeTag > 0)
		BEGIN
			SET @RowNum = 1
			WHILE (@RowNum <= @NumOfHospitalFoundByRelativeTag)
			BEGIN
				SET @TempHospitalID = (SELECT DISTINCT h.Hospital_ID
									   FROM (SELECT ROW_NUMBER()
											 OVER (ORDER BY wh.Hospital_ID ASC) AS RowNumber, wh.Hospital_ID
											 FROM WordDictionary w, Word_Hospital wh
											 WHERE w.[Type] = 2 AND
												   FREETEXT (w.Word,  @WhatPhrase) AND
												   w.Word_ID = wh.Word_ID) AS h
									   WHERE RowNumber = @RowNum)
				
				-- ADD ONLY HOSPITALS THAT HAS NOT BEEN APPEARED IN TEMPORARY LIST
				IF (NOT EXISTS(SELECT Hospital_ID
							   FROM @TempHospitalList
							   WHERE Hospital_ID = @TempHospitalID))
				BEGIN
					-- TAKE RATING POINT
					SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

					-- TAKE RATING COUNT NUMBER
					SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

					-- INSERT TO @TempHospitalList
					INSERT INTO @TempHospitalList
					SELECT @TempHospitalID,
						   (SELECT [dbo].FU_TAKE_NUMBER_OF_RELATIVE_TAG (@TempHospitalID, @WhatPhrase) *
								   @RelativePriorityOfTag +
								   CONVERT(INT, @RatingPoint * @PriorityOfRatingPoint) +
								   @RatingCount * @PriorityOfRatingCount)
				END

				SET @RowNum += 1
			END
		END

		-- NON-DIACRITIC VIETNAMESE
		SET @NumOfHospitalFoundByRelativeTag = 
			(SELECT COUNT(*)
			 FROM (SELECT Word_ID
				   FROM WordDictionary
				   WHERE @NonDiacriticWhatPhrase LIKE  N'%' +
						 [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Word) + N'%' AND
						 [Type] = 2) w)

		IF (@NumOfHospitalFoundByRelativeTag > 0)
		BEGIN
			SET @RowNum = 1
			WHILE (@RowNum <= @NumOfHospitalFoundByRelativeTag)
			BEGIN
				SET @TempHospitalID = (SELECT h.Hospital_ID
									   FROM (SELECT ROW_NUMBER()
											 OVER (ORDER BY wh.Hospital_ID ASC) AS RowNumber, wh.Hospital_ID
											 FROM WordDictionary w, Word_Hospital wh
											 WHERE @NonDiacriticWhatPhrase LIKE  N'%' +
												   [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Word) + N'%' AND
												   [Type] = 2 AND
												   w.Word_ID = wh.Word_ID) AS h
									   WHERE RowNumber = @RowNum)
				
				-- ADD ONLY HOSPITALS THAT HAS NOT BEEN APPEARED IN TEMPORARY LIST
				IF (NOT EXISTS(SELECT Hospital_ID
							   FROM @TempHospitalList
							   WHERE Hospital_ID = @TempHospitalID))
				BEGIN
					-- TAKE RATING POINT
					SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

					-- TAKE RATING COUNT NUMBER
					SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

					-- INSERT TO @TempHospitalList
					INSERT INTO @TempHospitalList
					SELECT @TempHospitalID,
						   (SELECT [dbo].FU_TAKE_NUMBER_OF_RELATIVE_TAG (@TempHospitalID, @NonDiacriticWhatPhrase) *
								   @NonDiacriticPriorityOfTag +
								   CONVERT(INT, @RatingPoint * @PriorityOfRatingPoint) +
								   @RatingCount * @PriorityOfRatingCount)
				END

				SET @RowNum += 1
			END
		END
	END

-- QUERY FROM SPECIALITY TABLE----------------------------------------------------------------

	-- FIND EXACTLY SPECIALITY
	DECLARE @NumOfHospitalFoundByExactlySpeciality INT = 0
	SET @NumOfHospitalFoundByExactlySpeciality = (SELECT COUNT(*)
												  FROM (SELECT Speciality_ID
														FROM Speciality
														WHERE @WhatPhrase LIKE  N'%' + Speciality_Name + N'%') s)

	IF (@NumOfHospitalFoundByExactlySpeciality > 0)
	BEGIN
		SET @RowNum = 1
		WHILE (@RowNum <= @NumOfHospitalFoundByExactlySpeciality)
		BEGIN
			SET @TempHospitalID = (SELECT h.Hospital_ID
								   FROM (SELECT ROW_NUMBER()
										 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_ID
										 FROM Speciality s, Hospital h, Hospital_Speciality hs
										 WHERE @WhatPhrase LIKE  N'%' + Speciality_Name + N'%' AND
											   s.Speciality_ID = hs.Speciality_ID AND
											   h.Hospital_ID = hs.Hospital_ID) AS h
								   WHERE RowNumber = @RowNum)

			-- CHECK IF HOSPITAL HAS BEED ADDED TO TEMPORARY LIST
			IF (EXISTS(SELECT Hospital_ID
						   FROM @TempHospitalList
						   WHERE Hospital_ID = @TempHospitalID))
			BEGIN
				-- UPDATE VALUE OF PRIORITY
				UPDATE @TempHospitalList
				SET [Priorty] = (SELECT [Priorty]
								 FROM @TempHospitalList
								 WHERE Hospital_ID = @TempHospitalID) + @ExactlyPriorityOfSpeciality
			END
			ELSE
			BEGIN	
				-- TAKE RATING POINT
				SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

				-- TAKE RATING COUNT NUMBER
				SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList
				SELECT @TempHospitalID, @ExactlyPriorityOfSpeciality +
							@RatingPoint * @PriorityOfRatingPoint +
							@RatingCount * @PriorityOfRatingCount
			END

			SET @RowNum += 1
		END
	END
	-- FIND RELATIVE MATCHING SPECIALITY
	ELSE
	BEGIN
		DECLARE @NumOfHospitalFoundByRelativeSpeciality INT = 0

		-- DIACRITIC VIETNAMSESE
		SET @NumOfHospitalFoundByRelativeSpeciality = 
				(SELECT COUNT(*)
				 FROM (SELECT s.Speciality_ID
					   FROM Speciality s, Hospital h, Hospital_Speciality hs
					   WHERE FREETEXT (Speciality_Name, @WhatPhrase) AND
							 s.Speciality_ID = hs.Speciality_ID AND
							 h.Hospital_ID = hs.Hospital_ID) s)

		IF (@NumOfHospitalFoundByRelativeSpeciality > 0)
		BEGIN
			SET @RowNum = 1
			WHILE (@RowNum <= @NumOfHospitalFoundByRelativeSpeciality)
			BEGIN
				SET @TempHospitalID = (SELECT h.Hospital_ID
									   FROM (SELECT ROW_NUMBER()
											 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_ID
											 FROM Speciality s, Hospital h, Hospital_Speciality hs
											 WHERE FREETEXT (Speciality_Name, @WhatPhrase) AND
												   s.Speciality_ID = hs.Speciality_ID AND
												   h.Hospital_ID = hs.Hospital_ID) AS h
									   WHERE RowNumber = @RowNum)

				-- CHECK IF HOSPITAL HAS BEED ADDED TO TEMPORARY LIST
				IF (EXISTS(SELECT Hospital_ID
							   FROM @TempHospitalList
							   WHERE Hospital_ID = @TempHospitalID))
				BEGIN
					-- UPDATE VALUE OF PRIORITY
					UPDATE @TempHospitalList
					SET [Priorty] = (SELECT [Priorty]
									 FROM @TempHospitalList
									 WHERE Hospital_ID = @TempHospitalID) +
									 [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY] (@TempHospitalID, @WhatPhrase) *
									 @RelativePriorityOfSpeciality
				END
				ELSE
				BEGIN	
					-- TAKE RATING POINT
					SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

					-- TAKE RATING COUNT NUMBER
					SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

					-- INSERT TO @TempHospitalList
					INSERT INTO @TempHospitalList
					SELECT @TempHospitalID,
								[dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY] (@TempHospitalID, @WhatPhrase) * 
								@ExactlyPriorityOfSpeciality +
								@RatingPoint * @PriorityOfRatingPoint +
								@RatingCount * @PriorityOfRatingCount
				END

				SET @RowNum += 1
			END
		END
		-- NON-DIACRITIC VIETNAMESE
		ELSE
		BEGIN
			SET @NumOfHospitalFoundByRelativeSpeciality = 
					(SELECT COUNT(*)
					 FROM (SELECT s.Speciality_ID
						   FROM Speciality s, Hospital h, Hospital_Speciality hs
						   WHERE @NonDiacriticWhatPhrase LIKE  N'%' +
								 [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) + N'%' AND
								 s.Speciality_ID = hs.Speciality_ID AND
								 h.Hospital_ID = hs.Hospital_ID) s)

			IF (@NumOfHospitalFoundByRelativeSpeciality > 0)
			BEGIN
				SET @RowNum = 1
				WHILE (@RowNum <= @NumOfHospitalFoundByRelativeSpeciality)
				BEGIN
					SET @TempHospitalID = (SELECT h.Hospital_ID
										   FROM (SELECT ROW_NUMBER()
												 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_ID
												 FROM Speciality s, Hospital h, Hospital_Speciality hs
												 WHERE @NonDiacriticWhatPhrase LIKE  N'%' +
													   [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) + N'%' AND
													   s.Speciality_ID = hs.Speciality_ID AND
													   h.Hospital_ID = hs.Hospital_ID) AS h
										   WHERE RowNumber = @RowNum)

					-- CHECK IF HOSPITAL HAS BEED ADDED TO TEMPORARY LIST
					IF (EXISTS(SELECT Hospital_ID
								   FROM @TempHospitalList
								   WHERE Hospital_ID = @TempHospitalID))
					BEGIN
						-- UPDATE VALUE OF PRIORITY
						UPDATE @TempHospitalList
						SET [Priorty] = (SELECT [Priorty]
										 FROM @TempHospitalList
										 WHERE Hospital_ID = @TempHospitalID) +
											   [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY](@TempHospitalID, @NonDiacriticWhatPhrase) *
											   @RelativePriorityOfSpeciality
					END
					ELSE
					BEGIN	
						-- TAKE RATING POINT
						SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

						-- TAKE RATING COUNT NUMBER
						SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

						-- INSERT TO @TempHospitalList
						INSERT INTO @TempHospitalList
						SELECT @TempHospitalID,
							   [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY](@TempHospitalID, @NonDiacriticWhatPhrase) * 
							   @ExactlyPriorityOfSpeciality +
							   @RatingPoint * @PriorityOfRatingPoint +
							   @RatingCount * @PriorityOfRatingCount
					END

					SET @RowNum += 1
				END
			END
		END
	END

-- QUERY FROM DISEASE TABLE-------------------------------------------------------------------

	-- FIND EXACTLY DISEASE
	DECLARE @NumOfHospitalFoundByExactlyDesease INT = 0
	SET @NumOfHospitalFoundByExactlyDesease = (SELECT COUNT(*)
											   FROM (SELECT Disease_ID
													 FROM Disease
													 WHERE @WhatPhrase LIKE  N'%' + Disease_Name + N'%') d)

	IF (@NumOfHospitalFoundByExactlyDesease > 0)
	BEGIN
		SET @RowNum = 1
		WHILE (@RowNum <= @NumOfHospitalFoundByExactlyDesease)
		BEGIN
			SET @TempHospitalID = (SELECT h.Hospital_ID
								   FROM (SELECT ROW_NUMBER()
										 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_ID
										 FROM Disease d, Speciality_Disease sd,
											  Hospital h, Hospital_Speciality hs
										 WHERE @WhatPhrase LIKE  N'%' + Disease_Name + N'%' AND
											   d.Disease_ID = sd.Disease_ID AND
											   sd.Speciality_ID = hs.Speciality_ID AND
											   h.Hospital_ID = hs.Hospital_ID) AS h
								   WHERE RowNumber = @RowNum)

			-- CHECK IF HOSPITAL HAS BEED ADDED TO TEMPORARY LIST
			IF (EXISTS(SELECT Hospital_ID
					   FROM @TempHospitalList
					   WHERE Hospital_ID = @TempHospitalID))
			BEGIN
				-- UPDATE VALUE OF PRIORITY
				UPDATE @TempHospitalList
				SET [Priorty] = (SELECT [Priorty]
								 FROM @TempHospitalList
								 WHERE Hospital_ID = @TempHospitalID) + @ExactlyPriorityOfDisease
			END
			ELSE
			BEGIN	
				-- TAKE RATING POINT
				SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

				-- TAKE RATING COUNT NUMBER
				SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

				-- INSERT TO @TempHospitalList
				INSERT INTO @TempHospitalList
				SELECT @TempHospitalID, @ExactlyPriorityOfDisease +
							@RatingPoint * @PriorityOfRatingPoint +
							@RatingCount * @PriorityOfRatingCount
			END

			SET @RowNum += 1
		END
	END
	-- FIND RELATIVE MATCHING DISEASE
	ELSE
	BEGIN
		DECLARE @NumOfHospitalFoundByRelativeDisease INT = 0
		
		-- DIACRITIC VIETNAMSESE
		SET @NumOfHospitalFoundByRelativeDisease = 
				(SELECT COUNT(*)
				 FROM (SELECT d.Disease_ID
					   FROM Disease d, Speciality_Disease sd,
							Hospital h, Hospital_Speciality hs
					   WHERE FREETEXT (Disease_Name, @WhatPhrase) AND
							 d.Disease_ID = sd.Disease_ID AND
							 sd.Speciality_ID = hs.Speciality_ID AND
							 h.Hospital_ID = hs.Hospital_ID) d)

		IF (@NumOfHospitalFoundByRelativeDisease > 0)
		BEGIN
			SET @RowNum = 1
			WHILE (@RowNum <= @NumOfHospitalFoundByRelativeDisease)
			BEGIN
				SET @TempHospitalID = (SELECT h.Hospital_ID
									   FROM (SELECT ROW_NUMBER()
											 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_ID
											 FROM Disease d, Speciality_Disease sd,
												  Hospital h, Hospital_Speciality hs
											 WHERE FREETEXT(Disease_Name, @WhatPhrase) AND
												   d.Disease_ID = sd.Disease_ID AND
												   sd.Speciality_ID = hs.Speciality_ID AND
												   h.Hospital_ID = hs.Hospital_ID) AS h
									   WHERE RowNumber = @RowNum)

				-- CHECK IF HOSPITAL HAS BEED ADDED TO TEMPORARY LIST
				IF (EXISTS(SELECT Hospital_ID
							   FROM @TempHospitalList
							   WHERE Hospital_ID = @TempHospitalID))
				BEGIN
					-- UPDATE VALUE OF PRIORITY
					UPDATE @TempHospitalList
					SET [Priorty] = (SELECT [Priorty]
									 FROM @TempHospitalList
									 WHERE Hospital_ID = @TempHospitalID) +
										   [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE] (@TempHospitalID, @WhatPhrase) *
										   @RelativePriorityOfDisease
				END
				ELSE
				BEGIN	
					-- TAKE RATING POINT
					SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

					-- TAKE RATING COUNT NUMBER
					SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

					-- INSERT TO @TempHospitalList
					INSERT INTO @TempHospitalList
					SELECT @TempHospitalID,
								[dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE] (@TempHospitalID, @WhatPhrase) * 
								@RelativePriorityOfDisease +
								@RatingPoint * @PriorityOfRatingPoint +
								@RatingCount * @PriorityOfRatingCount
				END

				SET @RowNum += 1
			END
		END
		-- NON-DIACRITIC VIETNAMESE
		ELSE
		BEGIN
			SET @NumOfHospitalFoundByRelativeSpeciality = 
					(SELECT COUNT(*)
					 FROM (SELECT d.Disease_ID
						   FROM Disease d, Speciality_Disease sd,
								Hospital h, Hospital_Speciality hs
						   WHERE @NonDiacriticWhatPhrase LIKE  N'%' +
							     [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Disease_Name) + N'%' AND
								 d.Disease_ID = sd.Disease_ID AND
								 sd.Speciality_ID = hs.Speciality_ID AND
								 h.Hospital_ID = hs.Hospital_ID) s)

			IF (@NumOfHospitalFoundByRelativeDisease > 0)
			BEGIN
				SET @RowNum = 1
				WHILE (@RowNum <= @NumOfHospitalFoundByRelativeDisease)
				BEGIN
					SET @TempHospitalID = (SELECT h.Hospital_ID
										   FROM (SELECT ROW_NUMBER()
												 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_ID
												 FROM Disease d, Speciality_Disease sd,
													  Hospital h, Hospital_Speciality hs
												 WHERE @NonDiacriticWhatPhrase LIKE  N'%' +
													   [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Disease_Name) + N'%' AND
													   d.Disease_ID = sd.Disease_ID AND
													   sd.Speciality_ID = hs.Speciality_ID AND
													   h.Hospital_ID = hs.Hospital_ID) AS h
										   WHERE RowNumber = @RowNum)

					-- CHECK IF HOSPITAL HAS BEED ADDED TO TEMPORARY LIST
					IF (EXISTS(SELECT Hospital_ID
								   FROM @TempHospitalList
								   WHERE Hospital_ID = @TempHospitalID))
					BEGIN
						-- UPDATE VALUE OF PRIORITY
						UPDATE @TempHospitalList
						SET [Priorty] = (SELECT [Priorty]
										 FROM @TempHospitalList
										 WHERE Hospital_ID = @TempHospitalID) +
											   [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE](@TempHospitalID, @NonDiacriticWhatPhrase) *
											   @RelativePriorityOfDisease
					END
					ELSE
					BEGIN	
						-- TAKE RATING POINT
						SET @RatingPoint = [dbo].[FU_TAKE_RATING_POINT] (@TempHospitalID)

						-- TAKE RATING COUNT NUMBER
						SET @RatingCount = [dbo].[FU_TAKE_RATING_COUNT] (@TempHospitalID)

						-- INSERT TO @TempHospitalList
						INSERT INTO @TempHospitalList
						SELECT @TempHospitalID,
							   [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE](@TempHospitalID, @NonDiacriticWhatPhrase) * 
							   @RelativePriorityOfDisease +
							   @RatingPoint * @PriorityOfRatingPoint +
							   @RatingCount * @PriorityOfRatingCount
					END

					SET @RowNum += 1
				END
			END
		END
	END

----------------------------------------------------------------------------------------------

	-- CHECK IF WHERE PHRASE IS AVAILABLE
	DECLARE @NumOfRecordInTemp INT = 0;
	SELECT @NumOfRecordInTemp = (SELECT COUNT(*)
								 FROM (SELECT Hospital_ID
										FROM @TempHospitalList) t)

	IF (@WherePhrase != 0)
	BEGIN
		IF (@NumOfRecordInTemp > 0)
		BEGIN
			-- CHECK IF CITY IS NOT NULL
			IF (@CityID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h, @TempHospitalList temp
				WHERE h.Hospital_ID = temp.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.Is_Active = 'True'
				ORDER BY temp.Priorty DESC
				RETURN;
			END

			-- CHECK IF DISTRICT IS NOT NLL
			IF (@DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h, @TempHospitalList temp
				WHERE h.Hospital_ID = temp.Hospital_ID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY temp.Priorty DESC
				RETURN;
			END

			-- CHECK IF BOTH CITY AND DISTRICT ARE NOT NULL
			IF (@CityID != 0 AND @DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
					   h.Rating, h.Rating_Count
				FROM Hospital h, @TempHospitalList temp
				WHERE h.Hospital_ID = temp.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY temp.Priorty DESC
				RETURN;
			END
		END
		ELSE
		BEGIN
			-- CHECK IF CITY IS NOT NULL
			IF (@CityID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
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
					   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
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

			-- CHECK IF BOTH CITY AND DISTRICT ARE NOT NULL
			IF (@CityID != 0 AND @DistrictID != 0)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
					   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
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
		END
	END
	-- THERE IS NO WHERE PHRASE
	ELSE
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @TempHospitalList temp
		WHERE h.Hospital_ID = temp.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY temp.Priorty DESC
		RETURN;
	END
END