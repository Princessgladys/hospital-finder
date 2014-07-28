-- SCRIPT LOAD SERVICES BY HOSPITAL_ID
-- VIETLP
IF OBJECT_ID('[SP_LOAD_SERVICES_BY_HOSPITAL_ID]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_SERVICES_BY_HOSPITAL_ID
GO

CREATE PROCEDURE SP_LOAD_SERVICES_BY_HOSPITAL_ID
	@Hospital_ID INT
AS
BEGIN
	SELECT s.Service_ID, s.Service_Name, st.Type_ID, st.Type_Name, hs.Is_Active
	FROM Service s
	LEFT JOIN ServiceType st ON s.Service_Type = st.Type_ID
	LEFT JOIN Hospital_Service hs ON s.Service_ID = hs.Service_ID AND hs.Hospital_ID = @Hospital_ID
	ORDER BY st.Type_ID
END
GO

EXEC SP_LOAD_SERVICES_BY_HOSPITAL_ID 1
GO
----------------------------------------------------------------
IF OBJECT_ID('[SP_LOAD_FACILITIES_BY_HOSPITAL_ID]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_FACILITIES_BY_HOSPITAL_ID
GO

CREATE PROCEDURE SP_LOAD_FACILITIES_BY_HOSPITAL_ID
	@Hospital_ID INT
AS
BEGIN
	SELECT f.Facility_ID, f.Facility_Name, ft.Type_ID, ft.Type_Name, hf.Is_Active
	FROM Facility f
	LEFT JOIN FacilityType ft ON f.Facility_Type = ft.Type_ID
	LEFT JOIN Hospital_Facility hf ON f.Facility_ID = hf.Facility_ID AND hf.Hospital_ID = @Hospital_ID
	ORDER BY ft.Type_ID
END
GO

EXEC SP_LOAD_FACILITIES_BY_HOSPITAL_ID 1
GO