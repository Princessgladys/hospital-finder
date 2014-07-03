-- SCRIPT TO SEARCH HOSPITALS
-- SONNX
IF OBJECT_ID('[SP_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_SEARCH_HOSPITAL
	@HospitalID INT,
	@CityID INT,
	@DistrictID INT,
	@SpecialityID INT,
	@DiseaseName NVARCHAR(64)
AS
BEGIN
	-- SET DEFAULT VALUE FOR INPUT PARAMETERS
	IF (@HospitalID = 0)
		SET @HospitalID = NULL

	-- CHECK IF HOSPITAL ID IS NOT NULL
	IF (@HospitalID IS NOT NULL)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
			   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active
		FROM Hospital h, Hospital_Speciality s, Speciality_Disease d
		WHERE h.Hospital_ID = @HospitalID
		ORDER BY h.Hospital_Name
		RETURN;
	END

	IF (@CityID = 0)
		SET @CityID = NULL

	iF (@DistrictID = 0)
		SET @DistrictID = NULL

	IF (@SpecialityID = 0)
		SET @SpecialityID = NULL

	IF (@DiseaseName IS NOT NULL)
	BEGIN
		IF ([dbo].[FU_REMOVE_WHITE_SPACE](@DiseaseName) = '')
			SET @DiseaseName = NULL
	END

	-- SET VALUE FOR 'WHAT' and 'WHERE' PHRASE
	DECLARE @WhatPhrase INT,
			@WherePhrase INT

	IF (@SpecialityID IS NULL AND @DiseaseName IS NULL)
		SET @WhatPhrase = 0
	ELSE
		SET @WhatPhrase = 1

	IF (@CityID IS NULL AND @DistrictID IS NULL)
		SET @WherePhrase = 0
	ELSE
		SET @WherePhrase = 1

	-- CHECK IF ALL PARAMETERS ARE NULL
	IF ((@WhatPhrase = 0) AND (@WherePhrase = 0))
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
			   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active
		FROM Hospital h
		WHERE h.Is_Active = 'True'
		ORDER BY Hospital_Name
		RETURN;
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
							  WHERE Disease_Name LIKE N'%@DiseaseName%')
			
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID IS NOT NULL)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
							   WHERE Disease_Name LIKE N'%@DiseaseName%')
							  
			-- CHECK IF @DiseaseID2 IS NOT NULL
			IF (@DiseaseID2 IS NOT NULL)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
								   WHERE Disease_Name LIKE N'%@DiseaseName%')
		
			-- CHECK IF @DiseaseID IS NOT NULL
			IF (@DiseaseID3 IS NOT NULL)
			BEGIN
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
				SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
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
END