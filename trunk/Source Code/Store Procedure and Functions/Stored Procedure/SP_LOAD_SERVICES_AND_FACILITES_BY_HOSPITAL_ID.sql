-- SCRIPT LOAD SERVICES BY HOSPITAL_ID
-- VIETLP
IF OBJECT_ID('[SP_LOAD_SERVICES_BY_HOSPITAL_ID]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_SERVICES_BY_HOSPITAL_ID
GO

CREATE PROCEDURE SP_LOAD_SERVICES_BY_HOSPITAL_ID
	@Hospital_ID INT
AS
BEGIN
	SELECT s.Service_ID, Service_Name, Type_ID, Type_Name, Is_Active
	FROM Service s
	LEFT JOIN ServiceType st ON s.Service_Type = st.Type_ID
	LEFT JOIN Hospital_Service hs ON s.Service_ID = hs.Service_ID AND hs.Hospital_ID = @Hospital_ID
	ORDER BY Type_ID
END
GO

EXEC SP_LOAD_SERVICES_BY_HOSPITAL_ID 1
