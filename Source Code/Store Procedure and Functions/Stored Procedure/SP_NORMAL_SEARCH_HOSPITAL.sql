-- SCRIPT TO SEARCH HOSPITALS
-- USING NORMAL OPTION
-- SONNX
IF OBJECT_ID('[SP_NORMAL_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
	@WhatPhrase NVARCHAR(128), -- ALWAYS NOT NULL
	@CityName NVARCHAR(32),
	@DistrictName VARCHAR(32)
AS
BEGIN
	-- VARIABLE FOR PERCENTAGE OF SIMILARITY
	DECLARE @PercentageOfSimilarity FLOAT = 0.85

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
									 OVER (ORDER BY h.Hospital_ID ASC) AS RowNumber, h.Hospital_Name
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
			WHERE [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(h.Hospital_Name)) LIKE
				  (N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(@HospitalName)) + N'%') OR
				  [dbo].[FU_STRING_COMPARE] (LOWER(h.Hospital_Name), LOWER(@HospitalName))
							>= @PercentageOfSimilarity
			ORDER BY h.Hospital_Name
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
									   OVER (ORDER BY s.Speciality_ID ASC) AS RowNumber, s.Speciality_Name
									   FROM Speciality s) AS s
								 WHERE RowNumber = @RowNumber

		-- FIND MATCHED RESULT
		IF ([dbo].[FU_IS_PATTERN_MATCHED] (@WhatPhrase, @SpecialityName) = 1)
		BEGIN
			SET @IsHaveSpeciality = 1
			SET @SpecialityName = @SpecialityName
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
									OVER (ORDER BY d.Disease_ID ASC) AS RowNumber, d.Disease_Name
									FROM Disease d) AS d
							  WHERE RowNumber = @RowNumber

		-- FIND MATCHED RESULT
		IF ([dbo].[FU_IS_PATTERN_MATCHED] (@WhatPhrase, @DiseaseName) = 1)
		BEGIN
			SET @IsHaveDisease = 1
			SET @DiseaseName = @DiseaseName
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
	SET @FromPhrase = N'FROM Hospital h'

	DECLARE @ConditionPhrase NVARCHAR(512) = NULL
	SET @ConditionPhrase = N'WHERE h.Is_Active = ''True'''

	DECLARE @OrderPhrase NVARCHAR(512)
	SET @OrderPhrase = N'ORDER BY h.Hospital_Name'

	DECLARE @SqlQuery NVARCHAR(512) = NULL
	
	-- CHECK IF ONLY WHAT PHRASE IS AVAILABLE
	IF (@WherePhrase = 0)
	BEGIN
		SET @FromPhrase += CASE WHEN @IsHaveSpeciality = 1
						   THEN N', Speciality s, Hospital_Speciality hs'
						   ELSE '' END;
		SET @FromPhrase += CASE WHEN @IsHaveDisease = 1
						   THEN N', Disease d, Speciality_Disease sd'
						   ELSE '' END;

		SET @ConditionPhrase += CASE WHEN @IsHaveSpeciality = 1
								THEN N' AND ([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(s.Speciality_Name))' +
									 N' LIKE (N''%'' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(@SpecialityName)) + N''%'')' +
									 N' OR [dbo].[FU_STRING_COMPARE] (LOWER(s.Speciality_Name), LOWER(@SpecialityName))' +
									 N' >= @PercentageOfSimilarity)' +
									 N' AND hs.Speciality_ID = s.Speciality_ID'
								ELSE N' OR [dbo].[FU_STRING_COMPARE]' +
									 N' (LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]' +
									 N' (s.Speciality_Name)), @WhatPhrase) >= @PercentageOfSimilarity'
								END;
		SET @ConditionPhrase += CASE WHEN @IsHaveDisease = 1
								THEN N' AND ([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(d.Disease_Name))' +
									 N' LIKE (N''%'' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(@DiseaseName)) + N''%'')' +
									 N' OR [dbo].[FU_STRING_COMPARE] (LOWER(d.Disease_Name), LOWER(@DiseaseName))' +
									 N' >= @PercentageOfSimilarity)' +
									 N' AND sd.Disease_ID = d.Disease_ID'
								ELSE N' OR [dbo].[FU_STRING_COMPARE]' +
									 N' (LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]' +
									 N' (d.Disease_Name)), @WhatPhrase) >= @PercentageOfSimilarity'
								END;
		SET @ConditionPhrase += CASE WHEN (@IsHaveSpeciality = 1) AND (@IsHaveDisease = 1)
								THEN  N' AND h.Hospital_ID = hs.Speciality_ID'
								ELSE CASE WHEN (@IsHaveSpeciality = 0) AND (@IsHaveDisease = 0)
									 THEN ''
									 ELSE N' AND h.Hospital_ID = hs.Speciality_ID'
									 END
							    END;

		SET @SqlQuery = @SelectPhrase + CHAR(13) + @FromPhrase + CHAR(13) +
						@ConditionPhrase + CHAR(13) + @OrderPhrase

		EXECUTE SP_EXECUTESQL @SqlQuery,
							  N'@SpecialityName NVARCHAR(64), @DiseaseName NVARCHAR(64)',
							  @SpecialityName, @DiseaseName
		RETURN;
	END

	-- CHECK IF ONLY WHAT PHRASE IS AVAILABLE
	ELSE
	BEGIN
		SET @FromPhrase += CASE WHEN @IsHaveSpeciality = 1
						   THEN N', Speciality s, Hospital_Speciality hs'
						   ELSE '' END;
		SET @FromPhrase += CASE WHEN @IsHaveDisease = 1
						   THEN N', Disease d, Speciality_Disease sd'
						   ELSE '' END;
		SET @FromPhrase += CASE WHEN @CityName IS NOT NULL
						   THEN ' AND City c'
						   ELSE '' END;
		SET @FromPhrase += CASE WHEN @DistrictName IS NOT NULL
						   THEN ' AND District di'
						   ELSE '' END;

		SET @ConditionPhrase += CASE WHEN @IsHaveSpeciality = 1
								THEN N' AND ([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(s.Speciality_Name))' +
									 N' LIKE (N''%'' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(@SpecialityName)) + N''%'')' +
									 N' OR [dbo].[FU_STRING_COMPARE] (LOWER(s.Speciality_Name), LOWER(@SpecialityName))' +
									 N' >= @PercentageOfSimilarity)' +
									 N' AND hs.Speciality_ID = s.Speciality_ID'
								ELSE N' OR [dbo].[FU_STRING_COMPARE]' +
									 N' (LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]' +
									 N' (s.Speciality_Name)), @WhatPhrase) >= @PercentageOfSimilarity'
								END;
		SET @ConditionPhrase += CASE WHEN @IsHaveDisease = 1
								THEN N' AND ([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(d.Disease_Name))' +
									 N' LIKE (N''%'' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(@DiseaseName)) + N''%'')' +
									 N' OR [dbo].[FU_STRING_COMPARE] (LOWER(d.Disease_Name), LOWER(@DiseaseName))' +
									 N' >= @PercentageOfSimilarity)' +
									 N' AND sd.Disease_ID = d.Disease_ID'
								ELSE N' OR [dbo].[FU_STRING_COMPARE]' +
									 N' (LOWER([dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]' +
									 N' (d.Disease_Name)), @WhatPhrase) >= @PercentageOfSimilarity'
								END;
		SET @ConditionPhrase += CASE WHEN (@IsHaveSpeciality = 1) AND (@IsHaveDisease = 1)
								THEN  N' AND h.Hospital_ID = hs.Speciality_ID'
								ELSE CASE WHEN (@IsHaveSpeciality = 0) AND (@IsHaveDisease = 0)
									 THEN ''
									 ELSE N' AND h.Hospital_ID = hs.Speciality_ID'
									 END
							    END;
		SET @ConditionPhrase += CASE WHEN @CityName IS NOT NULL
								THEN N' AND [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(c.City_Name))' +
									 N' LIKE N''%@[dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(CityName))%''' +
									 N' AND h.City_ID = c.City_ID'
								ELSE ''
							    END;
		SET @ConditionPhrase += CASE WHEN @DistrictName IS NOT NULL
								THEN N' AND [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(di.District_Name))' +
									 N' LIKE N''%[dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (LOWER(@DistrictName))%''' +
									 N' AND h.District_ID = di.District_ID'
								ELSE ''
							    END;

		SET @SqlQuery = @SelectPhrase + CHAR(13) + @FromPhrase + CHAR(13) +
						@ConditionPhrase + CHAR(13) + @OrderPhrase

		EXECUTE SP_EXECUTESQL @SqlQuery,
							  N'@SpecialityName NVARCHAR(64), @DiseaseName NVARCHAR(64), @CityName NVARCHAR(32), @DistrictName NVARCHAR(32)',
							  @SpecialityName, @DiseaseName, @CityName, @DistrictName
		RETURN;
	END
END