--VIETLP--
IF OBJECT_ID('[SP_LOAD_TOP_10_HOSPITAL_APPOINTMENT]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_TOP_10_HOSPITAL_APPOINTMENT
GO
CREATE PROCEDURE SP_LOAD_TOP_10_HOSPITAL_APPOINTMENT
	@From_Date DATE,
	@To_Date DATE
AS
BEGIN
	SELECT TOP 10 h.Hospital_ID, h.Hospital_Name, h.Address, h.Rating, h.Rating_Count, COUNT(*) AS Appointment_Count
	FROM Hospital h, Appointment a
	WHERE h.Hospital_ID = a.Curing_Hospital AND a.Appointment_Date BETWEEN @From_Date AND @To_Date
	GROUP BY h.Hospital_ID, h.Hospital_Name, h.Address, h.Rating, h.Rating_Count
	ORDER BY Appointment_Count DESC
END

EXEC SP_LOAD_TOP_10_HOSPITAL_APPOINTMENT '2014-05-05', '2014-11-11'