-- SCRIPT TO SEARCH HOSPITALS
-- SONNX
IF OBJECT_ID('[SP_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_SEARCH_HOSPITAL
	@CityID INT,
	@DistrictID INT,
	@SpecialityID INT,
	@DiseaseName NVARCHAR(64)
AS
BEGIN
	-- SET DEFAULT VALUE FOR INPUT PARAMETERS
	IF (@CityID = 0)
		SET @CityID = NULL

	iF (@DistrictID = 0)
		SET @DistrictID = NULL

	IF (@SpecialityID = 0)
		SET @SpecialityID = NULL

	iF (@DiseaseName IS NOT NULL)
	BEGIN
		IF ([dbo].[FU_REMOVE_WHITE_SPACE](@DiseaseName) = '')
			SET @DiseaseName = NULL
	END

	-- CHECK IF BOTH SPECIALITY AND DISEASE ARE NULL
	IF (((@SpecialityID IS NULL) AND (@DiseaseName IS NULL)) AND
		((@CityID IS NOT NULL) OR (@DistrictID IS NOT NULL)))
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
				  h.District_ID = @DistrictID
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
			WHERE h.City_ID = @CityID
			RETURN;
		END

		-- CASE CITY_ID IS NULL BUT DISTRICT_ID IS NOT NULL
		IF ((@CityID IS NOT NULL) AND (@DistrictID IS NOT NULL))
		BEGIN
			SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
				   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				   h.Is_Allow_Appointment, h.Is_Active
			FROM Hospital h
			WHERE h.District_ID = @DistrictID
			RETURN;
		END
	END
END

SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
	   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
	   h.End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
	   h.Is_Allow_Appointment, h.Is_Active
FROM Hospital h