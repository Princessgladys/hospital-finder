-- SCRIPT TO SEARCH HOSPITALS
-- USING ADVANCED OPTION
-- SONNX
IF OBJECT_ID('[SP_ADVANCED_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_ADVANCED_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_ADVANCED_SEARCH_HOSPITAL
	@CityID INT, -- ALWAYS NOT NULL
	@DistrictID INT,
	@SpecialityID INT,
	@DiseaseName NVARCHAR(64)
AS
BEGIN
	-- SET DEFAULT VALUES FOR INPUT PARAMETERS
	iF (@DistrictID = 0)
		SET @DistrictID = NULL

	IF (@SpecialityID = 0)
		SET @SpecialityID = NULL

	IF (@DiseaseName IS NOT NULL)
	BEGIN
		IF ([dbo].[FU_REMOVE_WHITE_SPACE](@DiseaseName) = '')
			SET @DiseaseName = NULL
	END

	DECLARE @DiseaseID INT = NULL
	IF (@DiseaseName IS NOT NULL)
		BEGIN
			SET @DiseaseID = (SELECT Disease_ID
							  FROM Disease
							  WHERE Disease_Name LIKE (N'%' + @DiseaseName + N'%'))
		END

	-- SET VALUE FOR 'WHAT' and 'WHERE' PHRASE
	DECLARE @WhatPhrase INT = NULL,
			@WherePhrase INT = 1

	IF (@SpecialityID IS NULL AND @DiseaseName IS NULL)
		SET @WhatPhrase = 0
	ELSE
		SET @WhatPhrase = 1

	DECLARE @SelectPhrase NVARCHAR(512) = NULL
	SET @SelectPhrase = N'SELECT DISTINCT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,' +
						N'h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,' +
						N'h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,' +
						N'h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time'

	DECLARE @FromPhrase NVARCHAR(512) = NULL
	SET @FromPhrase = N'FROM Hospital h'

	DECLARE @ConditionPhrase NVARCHAR(512) = NULL
	SET @ConditionPhrase = N'WHERE h.Is_Active = ''True'''

	DECLARE @OrderPhrase NVARCHAR(512)
	SET @OrderPhrase = N'ORDER BY h.Hospital_Name'

	DECLARE @SqlQuery NVARCHAR(512) = NULL

	-- CASE THAT WHAT PHRASE IS NULL AND WHERE PHRASE HAVE VALUE(S)
	IF (@WhatPhrase = 0)
	BEGIN
		SET @ConditionPhrase += CASE WHEN @DistrictID IS NOT NULL
								THEN N' AND h.District_ID = @DistrictID'
								ELSE ''
								END;
		SET @ConditionPhrase += N' AND h.City_ID = @CityID'
			
		SET @SqlQuery = @SelectPhrase + CHAR(13) + @FromPhrase + CHAR(13) +
						@ConditionPhrase + CHAR(13) + @OrderPhrase

		EXECUTE SP_EXECUTESQL @SqlQuery,
							  N'@CityID INT, @DistrictID INT',
							  @CityID, @DistrictID
		RETURN;
	END
	
	-- CASE THAT WHAT PHRASE AND WHERE PHRASE HAVE VALUE
	IF (@WhatPhrase = 1)
	BEGIN		
		SET @FromPhrase += CASE WHEN @DiseaseID IS NOT NULL
						   THEN N', Speciality_Disease sd'
						   ELSE ''
						   END;
		SET @FromPhrase += CASE WHEN (@SpecialityID IS NOT NULL) AND (@DiseaseID IS NOT NULL)
						   THEN N', Hospital_Speciality hs'
						   ELSE CASE WHEN (@SpecialityID IS NULL) AND (@DiseaseID IS NULL) 
									 THEN ''
									 ELSE N', Hospital_Speciality hs'
									 END
						   END;

		SET @ConditionPhrase += CASE WHEN @SpecialityID IS NOT NULL
								THEN N' AND hs.Speciality_ID = @SpecialityID'
								ELSE ''
								END;
		SET @ConditionPhrase += CASE WHEN @DiseaseID IS NOT NULL
								THEN N' AND sd.Disease_ID = @DiseaseID' +
									 N' AND hs.Speciality_ID = sd.Speciality_ID'
								ELSE ''
								END;
		SET @ConditionPhrase += CASE WHEN (@SpecialityID IS NOT NULL) AND (@DiseaseID IS NOT NULL)
								THEN N' AND h.Hospital_ID = hs.Hospital_ID'
								ELSE CASE WHEN (@SpecialityID IS NULL) AND (@DiseaseID IS NULL)
									 THEN ''
									 ELSE N' AND h.Hospital_ID = hs.Hospital_ID'
									 END
								END;

		SET @ConditionPhrase += N' AND h.City_ID = @CityID'
		SET @ConditionPhrase += CASE WHEN @DistrictID IS NOT NULL
								THEN N' AND h.District_ID = @DistrictID'
								ELSE ''
								END;

		SET @SqlQuery = @SelectPhrase + CHAR(13) + @FromPhrase + CHAR(13) +
						@ConditionPhrase + CHAR(13) + @OrderPhrase

		EXECUTE SP_EXECUTESQL @SqlQuery,
							  N'@CityID INT, @DistrictID INT, @SpecialityID INT, @DiseaseID INT',
							  @CityID, @DistrictID, @SpecialityID, @DiseaseID
		RETURN;
	END
END