-- SCRIPT TO SEARCH HOSPITALS
-- SONNX
IF OBJECT_ID('[SP_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_SEARCH_HOSPITAL
	@CityID INT,
	@DistrictID INT,
	@SpecialityID INT,
	@DiseaseID INT
AS
BEGIN
	IF (@CityID = 0)
		SET @CityID = NULL
	iF (@DistrictID = 0)
		SET @DistrictID = NULL
	IF (@SpecialityID = 0)
		SET @SpecialityID = NULL
	iF (@DiseaseID = 0)
		SET @DiseaseID = NULL

	-- CHECK IF ALL PARAMETERS ARE NULL
	if (@CityID IS NULL AND @DistrictID IS NULL AND
		@SpecialityID IS NULL AND @DiseaseID IS NULL)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address]
		FROM Hospital h
		ORDER BY Hospital_Name
	END

	-- IN CASE SPECIALITY AND DISEASE ARE BOTH NULL
	IF ((@SpecialityID IS NULL AND @DiseaseID IS NULL) AND
		(@DistrictID IS NOT NULL OR @CityID IS NOT NULL))
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address]
		FROM Hospital h
		WHERE (@CityID IS NULL OR h.City_ID = @CityID) AND
			  (@DistrictID IS NULL OR h.District_ID = @DistrictID)
		ORDER BY Hospital_Name
	END

	-- IN CASE CITY AND DISTRICT ARE BOTH NULL
	IF ((@CityID IS NULL AND @DistrictID IS NULL) AND
		(@SpecialityID IS NOT NULL OR @DiseaseID IS NOT NULL))
	BEGIN
		IF (@SpecialityID IS NOT NULL AND @DiseaseID IS NULL)
		BEGIN
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address]
			FROM Hospital h, Hospital_Speciality s
			WHERE s.Speciality_ID = @SpecialityID AND
				  h.Hospital_ID = s.Hospital_ID
		END

		IF (@DiseaseID IS NOT NULL AND @SpecialityID IS NULL)
		BEGIN
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address]
			FROM Hospital h, Speciality_Disease d, Hospital_Speciality s
			WHERE d.Disease_ID = @DiseaseID AND
				  d.Speciality_ID = s.Speciality_ID AND
				  s.Hospital_ID = h.Hospital_ID
		END

		IF (@SpecialityID IS NOT NULL AND @DiseaseID IS NOT NULL)
		BEGIN
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address]
			FROM Hospital h, Speciality_Disease d, Hospital_Speciality s
			WHERE d.Disease_ID = @DiseaseID AND
				  d.Speciality_ID = @SpecialityID AND
				  s.Hospital_ID = h.Hospital_ID
		END
	END
END

EXEC SP_SEARCH_HOSPITAL NULL, NULL, NULL, NULL