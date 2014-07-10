-- SCRIPT TO SEARCH HOSPITALS
-- USING LOCATION OPTION
-- VIETLP
IF OBJECT_ID('[SP_LOCATION_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOCATION_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_LOCATION_SEARCH_HOSPITAL
	@Latitude FLOAT,
	@Longitude FLOAT,
	@Distance INT	
AS
BEGIN
	SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
		   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Start_Time,
		   h.End_Time, h.Short_Description, h.Full_Description,
		   h.Is_Allow_Appointment, h.Is_Active, h.Coordinate
	FROM Hospital h
	WHERE h.Is_Active = 'True' AND
		  [dbo].[FU_GET_DISTANCE] 
			(CONVERT(FLOAT, SUBSTRING(h.Coordinate, 1, CHARINDEX(',', h.Coordinate) - 1)),
			 CONVERT(FLOAT, SUBSTRING(h.Coordinate, CHARINDEX(',', h.Coordinate) + 1, LEN(h.Coordinate))),
			 @Latitude, @Longitude) <= @Distance
END

<-- EXEC -->
EXEC SP_LOCATION_SEARCH_HOSPITAL 10.784075, 106.689859, 700