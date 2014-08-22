-- FUNCTION TO RETURN NUMBER OF RELATIVE SPECIALITY OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY]
GO
CREATE FUNCTION [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY]
(
	@HospitalID INT,
	@WhatPhrase NVARCHAR(4000)
)
RETURNS INT
AS
BEGIN
	IF (@WhatPhrase IS NULL OR @WhatPhrase = '')
	BEGIN
		RETURN 0;
	END

	DECLARE @NumberOfSpeciality INT

	SELECT @NumberOfSpeciality = (SELECT COUNT(*)
								  FROM (SELECT s.Speciality_ID
										FROM Speciality s, Hospital h, Hospital_Speciality hs
										WHERE FREETEXT (Speciality_Name, @WhatPhrase) AND
											  s.Speciality_ID = hs.Speciality_ID AND
											  h.Hospital_ID = hs.Hospital_ID AND
											  h.Hospital_ID = @HospitalID) s)

	IF (@NumberOfSpeciality IS NULL)
		RETURN 0;
	ELSE
		RETURN @NumberOfSpeciality;

	RETURN 0;
END