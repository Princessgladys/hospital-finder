-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO LOAD A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('[SP_LOAD_SPECIFIC_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_SPECIFIC_HOSPITAL
GO
CREATE PROCEDURE SP_LOAD_SPECIFIC_HOSPITAL
	@HospitalID INT
AS
BEGIN
	SELECT h.Hospital_ID, h.Hospital_Name, h.Hospital_Type, h.[Address], h.Ward_ID, h.District_ID,
		   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
		   h.OrDinary_End_Time, h.Holiday_Start_Time, h.Holiday_End_Time, h.Rating_Count,
		   h.Coordinate, h.Short_Description, h.Full_Description, h.Rating,
		   h.Is_Allow_Appointment, h.Is_Active, h.Created_Person,
		   c.City_Name, d.[Type] + ' '+  d.District_Name AS District_Name,
		   w.[Type] + ' '  + w.Ward_Name AS Ward_Name
	FROM Hospital h, City c, District d, Ward w
	WHERE h.Hospital_ID = @HospitalID AND
		  h.City_ID = c.City_ID AND
		  h.District_ID = d.District_ID AND
		  h.Ward_ID = w.Ward_ID
END