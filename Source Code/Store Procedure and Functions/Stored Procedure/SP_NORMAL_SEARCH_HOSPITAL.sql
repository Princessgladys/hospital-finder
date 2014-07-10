-- SCRIPT TO SEARCH HOSPITALS
-- USING NORMAL OPTION
-- SONNX
IF OBJECT_ID('[SP_NORMAL_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
	@WhatPhrase NVARCHAR(128),
	@CityName NVARCHAR(32),
	@DistrictName NVARCHAR(32),
	@PercentageOfSimilarity FLOAT
AS
BEGIN
	-- VARIABLE FOR ROW NUMBER
	DECLARE @RowNumber INT

	-- VARIABLES FOR HOSPITALS
	DECLARE @TotalHospital INT
	DECLARE @HospitalName NVARCHAR(64)
	
	-- TRANSFORM @WhatPhrase to non-diacritic Vietnamese
	SET @WhatPhrase = [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (@WhatPhrase)
	
	-- CHECK IF THERE IS ANY HOSPITAL NAME IN @WhatPhrase
	SET @TotalHospital = (SELECT COUNT(h.Hospital_ID)
						  FROM Hospital h)
	SET @RowNumber = 1

	WHILE (@RowNumber <= @TotalHospital)
	BEGIN
		-- GET HOSPITAL NAME FROM DATABASE
		SELECT @HospitalName = LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (h.Hospital_Name))
							   FROM (SELECT ROW_NUMBER()
									 OVER (ORDER BY TokenList.ID ASC) AS RowNumber, h.Hospital_Name
									 FROM Hospital h) AS h
							   WHERE RowNumber = @RowNumber

		-- FIND MATCHED RESULT
		IF ([dbo].[FU_IS_PATTERN_MATCHED] (@HospitalName, @WhatPhrase) = 1)
		BEGIN
			-- QUERY HOSPITAL NAME IN DATABASE
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
				   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h
			WHERE LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (h.Hospital_Name)) LIKE
				  (N'%' + @HospitalName + N'%') OR
				  [dbo].[FU_STRING_COMPARE]
					(LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]
						(h.Hospital_Name)), @HospitalName) >= @PercentageOfSimilarity
			RETURN;
		END

		SET @RowNumber += 1
	END

	-- VARIABLES FOR SPECIALITY
	DECLARE @TotalSpeciality INT
	DECLARE @SpecialityName NVARCHAR(64)
	DECLARE @IsHaveSpeciality INT = 0

	-- CHECK IF THERE IS ANY SPECIALITY NAME IN @WhatPhrase
	SET @TotalSpeciality = (SELECT COUNT(s.Speciality_ID)
							FROM Speciality s)
	SET @RowNumber = 1

	WHILE (@RowNumber < @TotalSpeciality)
	BEGIN
		-- GET SPECIALITY NAME FROM DATABASE
		SELECT @SpecialityName = LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (s.Speciality_Name))
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, s.Speciality_Name
									   FROM Speciality s) AS s
								 WHERE RowNumber = @RowNumber

		-- FIND MATCHED RESULT
		IF ([dbo].[FU_IS_PATTERN_MATCHED] (@WhatPhrase, @SpecialityName) = 1)
		BEGIN
			SET @IsHaveSpeciality = 1
			BREAK
		END

		SET @RowNumber += 1
	END

	-- VARIABLE FOR DISEASE
	DECLARE @TotalDisease INT
	DECLARE @DiseaseName NVARCHAR(64)
	DECLARE @IsHaveDisease INT = 0

	-- CHECK IF THERE IS ANY DISEASE NAME IN @WhatPhrase
	SET @TotalDisease = (SELECT COUNT(d.Disease_ID)
						 FROM Disease d)
	SET @RowNumber = 1

	WHILE (@RowNumber < @TotalDisease)
	BEGIN
		-- GET DISEASE NAME FROM DATABASE
		SELECT @DiseaseName = LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (d.Disease_Name))
							  FROM (SELECT ROW_NUMBER()
									OVER (ORDER BY TokenList.ID ASC) AS RowNumber, d.Disease_Name
									FROM Disease d) AS d
							  WHERE RowNumber = @RowNumber

		-- FIND MATCHED RESULT
		IF ([dbo].[FU_IS_PATTERN_MATCHED] (@WhatPhrase, @DiseaseName) = 1)
		BEGIN
			SET @IsHaveDisease = 1
			BREAK
		END

		SET @RowNumber += 1
	END

	-- VARIABLE FOR WHERE PHRASE
	DECLARE @WherePhrase INT

	IF (@CityName IS NULL AND @DistrictName IS NULL)
		SET @WherePhrase = 0
	ELSE
		SET @WherePhrase = 1

	-- VARIABLES FOR SEARCH QUERY
	DECLARE @SelectPhrase NVARCHAR(512) = NULL
	SET @SelectPhrase = N'SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,' +
						N'h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,' +
						N'h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,' +
						N'h.Is_Allow_Appointment, h.Is_Active'

	DECLARE @FromPhrase NVARCHAR(512) = NULL
	SET @FromPhrase = N'FROM Hospital h, Hospital_Speciality hs, Speciality_Disease sd'

	DECLARE @ConditionPhrase NVARCHAR(512) = NULL
	SET @ConditionPhrase = N'WHERE h.Is_Active = ''True'''

	DECLARE @OrderPhrase NVARCHAR(512)
	SET @OrderPhrase = N'ORDER BY h.Hospital_Name'

	DECLARE @SqlQuery NVARCHAR(512) = NULL
	
	-- CHECK IF ONLY WHAT PHRASE IS AVAILABLE
	IF (@WherePhrase = 0)
	BEGIN
		SET @FromPhrase += N', Speciality s, Disease d'

		SET @ConditionPhrase += CASE WHEN @IsHaveSpeciality = 1
								THEN N''
								ELSE '' END;

	END






































































	-- CHECK IF BOTH CITY AND DISTRICT ARE NULL
	IF ((@WherePhrase = 0) AND (@WhatPhrase = 1))
	BEGIN
		-- CASE SPECIALITY_ID AND DISEASE_NAME ARE NOT NULL
		IF ((@SpecialityID IS NOT NULL) AND (@DiseaseName IS NOT NULL))
		BEGIN
			DECLARE @DiseaseID INT
			SET @DiseaseID = (SELECT Disease_ID
							  FROM Disease
							  WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
			
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID IS NOT NULL)
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Disease_ID = @DiseaseID AND
					  d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
			ELSE
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
		END
		
		-- CASE SPECIALITY_ID IS NOT NULL BUT DISEASE_NAME IS NULL
		IF ((@SpecialityID IS NOT NULL) AND (@DiseaseName IS NULL))
		BEGIN
			SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
				   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
			WHERE d.Speciality_ID = @SpecialityID AND
				  s.Hospital_ID = h.Hospital_ID AND
				  h.Is_Active = 'True'
			ORDER BY h.Hospital_Name
			RETURN;
		END
		
		-- CASE SPECIALITY_ID IS NULL BUT DISEASE_NAME IS NOT NULL
		IF ((@SpecialityID IS NULL) AND (@DiseaseName IS NOT NULL))
		BEGIN
			DECLARE @DiseaseID2 INT
			SET @DiseaseID2 = (SELECT Disease_ID
							   FROM Disease
							   WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
							  
			-- CHECK IF @DiseaseID2 IS NOT NULL
			IF (@DiseaseID2 IS NOT NULL)
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Disease_ID = @DiseaseID2 AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
			ELSE
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h
				WHERE h.Is_Active = 'True'
				ORDER BY Hospital_Name
				RETURN;
			END
		END
	END

	-- CHECK IF BOTH SPECIALITY_ID AND DISEASE_NAME ARE NULL
	IF ((@WhatPhrase = 0) AND (@WherePhrase = 1))
	BEGIN
		-- CASE CITY_ID AND DISTRICT_ID ARE NOT NULL
		IF ((@CityID IS NOT NULL) AND (@DistrictID IS NOT NULL))
		BEGIN
			SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
				   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h
			WHERE h.City_ID = @CityID AND
				  h.District_ID = @DistrictID AND
				  h.Is_Active = 'True'
			ORDER BY h.Hospital_Name
			RETURN;
		END

		-- CASE CITY_ID IS NOT NULL BUT DISTRICT_ID IS NULL
		IF ((@CityID IS NOT NULL) AND (@DistrictID IS NULL))
		BEGIN
			SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
				   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h
			WHERE h.City_ID = @CityID AND
				  h.Is_Active = 'True'
			ORDER BY h.Hospital_Name
			RETURN;
		END

		-- CASE CITY_ID IS NULL BUT DISTRICT_ID IS NOT NULL
		IF ((@CityID IS NULL) AND (@DistrictID IS NOT NULL))
		BEGIN
			SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
				   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h
			WHERE h.District_ID = @DistrictID AND
				  h.Is_Active = 'True'
			ORDER BY h.Hospital_Name
			RETURN;
		END
	END
	
	-- CASE THAT WHAT PHRASE AND WHERE PHRASE HAVE VALUE
	IF ((@WhatPhrase = 1) AND (@WherePhrase = 1))
	BEGIN
		-- CASE THAT ALL PARAMETERS ARE NOT NULL
		IF ((@CityID IS NOT NULL) AND
			(@DistrictID IS NOT NULL) AND
			(@SpecialityID IS NOT NULL) AND
			(@DiseaseName IS NOT NULL))
		BEGIN
			DECLARE @DiseaseID3 INT
				SET @DiseaseID3 = (SELECT Disease_ID
								   FROM Disease
								   WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
		
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID3 IS NOT NULL)
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Disease_ID = @DiseaseID3 AND
					  d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
			ELSE
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
		END

		-- CASE THAT ONLY CITY IS NULL
		IF ((@CityID IS NULL) AND ((@DistrictID IS NOT NULL) AND
			(@SpecialityID IS NOT NULL) AND
			(@DiseaseName IS NOT NULL)))
		BEGIN
			DECLARE @DiseaseID4 INT
				SET @DiseaseID4 = (SELECT Disease_ID
								   FROM Disease
								   WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
		
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID4 IS NOT NULL)
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Disease_ID = @DiseaseID4 AND
					  d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
			ELSE
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
		END

		-- CASE THAT ONLY DISTRICT IS NULL
		IF ((@DistrictID IS NULL) AND ((@CityID IS NOT NULL) AND
			(@SpecialityID IS NOT NULL) AND
			(@DiseaseName IS NOT NULL)))
		BEGIN
			DECLARE @DiseaseID5 INT
				SET @DiseaseID5 = (SELECT Disease_ID
								   FROM Disease
								   WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
		
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID5 IS NOT NULL)
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Disease_ID = @DiseaseID5 AND
					  d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
			ELSE
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Speciality_ID = @SpecialityID AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
		END

		-- CASE THAT ONLY SPECIALITY IS NULL
		IF ((@SpecialityID IS NULL) AND ((@CityID IS NOT NULL) AND
			(@DistrictID IS NOT NULL) AND
			(@DiseaseName IS NOT NULL)))
		BEGIN
			DECLARE @DiseaseID6 INT
				SET @DiseaseID6 = (SELECT Disease_ID
								   FROM Disease
								   WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
		
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID6 IS NOT NULL)
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE d.Disease_ID = @DiseaseID6 AND
					  s.Hospital_ID = h.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
			ELSE
			BEGIN
				SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					   h.Is_Allow_Appointment, h.Is_Active
				FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
				WHERE s.Hospital_ID = h.Hospital_ID AND
					  h.City_ID = @CityID AND
					  h.District_ID = @DistrictID AND
					  h.Is_Active = 'True'
				ORDER BY h.Hospital_Name
				RETURN;
			END
		END

		-- CASE THAT ONLY DISEASE IS NULL
		IF ((@DiseaseName IS NULL) AND ((@CityID IS NOT NULL) AND
			(@DistrictID IS NOT NULL) AND
			(@SpecialityID IS NOT NULL)))
		BEGIN
			SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
					h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
					h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
					h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
			WHERE d.Speciality_ID = @SpecialityID AND
				  s.Hospital_ID = h.Hospital_ID AND
				  h.City_ID = @CityID AND
				  h.District_ID = @DistrictID AND
				  h.Is_Active = 'True'
			ORDER BY h.Hospital_Name
			RETURN;
		END
	END
END