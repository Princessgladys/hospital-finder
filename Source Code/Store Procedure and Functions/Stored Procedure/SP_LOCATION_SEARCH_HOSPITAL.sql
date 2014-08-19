-- SCRIPT TO SEARCH HOSPITALS
-- USING LOCATION OPTION
-- VIETLP
IF OBJECT_ID('[SP_LOCATION_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOCATION_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_LOCATION_SEARCH_HOSPITAL
	@Latitude FLOAT,
	@Longitude FLOAT,
	@Distance FLOAT	
AS
BEGIN
	SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
		   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
		   h.Ordinary_End_Time, h.Full_Description, h.Holiday_End_Time,
		   h.Is_Allow_Appointment, h.Is_Active, h.Coordinate, h.Holiday_Start_Time, h.Rating,
		   c.City_Name, d.District_Name, w.Ward_Name,
		   [dbo].[FU_GET_DISTANCE] 
			(CONVERT(FLOAT, SUBSTRING(h.Coordinate, 1, CHARINDEX(',', h.Coordinate) - 1)),
			 CONVERT(FLOAT, SUBSTRING(h.Coordinate, CHARINDEX(',', h.Coordinate) + 1, LEN(h.Coordinate))),
			 @Latitude, @Longitude) AS Distance
		   
	FROM City c, District d, Ward w, Hospital h
	WHERE h.Is_Active = 'True' AND
		  h.City_ID = c.City_ID AND h.District_ID = d.District_ID AND h.Ward_ID = w.Ward_ID AND
		  [dbo].[FU_GET_DISTANCE] 
			(CONVERT(FLOAT, SUBSTRING(h.Coordinate, 1, CHARINDEX(',', h.Coordinate) - 1)),
			 CONVERT(FLOAT, SUBSTRING(h.Coordinate, CHARINDEX(',', h.Coordinate) + 1, LEN(h.Coordinate))),
			 @Latitude, @Longitude) <= CONVERT(INT, @Distance)
	ORDER BY Distance
END
GO

EXEC SP_LOCATION_SEARCH_HOSPITAL 10.8525, 106.6225, 10000
GO

SELECT Hospital_Name, Address, Coordinate
FROM Hospital
ORDER BY Coordinate