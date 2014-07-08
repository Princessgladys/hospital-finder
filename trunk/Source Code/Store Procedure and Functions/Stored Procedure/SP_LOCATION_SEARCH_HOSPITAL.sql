-- SCRIPT TO SEARCH HOSPITALS
-- USING LOCATION OPTION
-- SONNX
IF OBJECT_ID('[SP_LOCATION_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOCATION_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_LOCATION_SEARCH_HOSPITAL
	@Distance INT,
	@Latitude FLOAT,
	@Longitude FLOAT
AS
BEGIN
	SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
		   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
		   h.End_Time, h.Short_Description, h.Full_Description,
		   h.Is_Allow_Appointment, h.Is_Active, h.Coordinate
	FROM Hospital h
	WHERE h.Is_Active = 'True' AND
		  [dbo].[FU_GET_DISTANCE]
			(CONVERT(FLOAT, PARSENAME(REPLACE(h.Coordinate, ',', '.'), 2)),
			 CONVERT(FLOAT, PARSENAME(REPLACE(h.Coordinate, ',', '.'), 1)),
			 @Latitude, @Longitude) > @Distance
END