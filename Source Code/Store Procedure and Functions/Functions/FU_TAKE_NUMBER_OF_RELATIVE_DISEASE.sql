-- FUNCTION TO RETURN NUMBER OF RELATIVE DISEASE OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_NUMBER_OF_RELATIVE_DISEASE', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_NUMBER_OF_RELATIVE_DISEASE]
GO
CREATE FUNCTION [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE]
(
	@HospitalID INT,
	@WhatPhrase NVARCHAR(4000)
)
RETURNS INT
AS
BEGIN
	DECLARE @NumberOfDisease INT

	SELECT @NumberOfDisease = (SELECT COUNT(*)
							   FROM (SELECT d.Disease_ID
									 FROM Disease d, Speciality_Disease sd,
										  Hospital h, Hospital_Speciality hs
									 WHERE FREETEXT(Disease_Name, @WhatPhrase) AND
										   d.Disease_ID = sd.Disease_ID AND
										   sd.Speciality_ID = hs.Speciality_ID AND
										   h.Hospital_ID = hs.Hospital_ID AND
										   h.Hospital_ID = @HospitalID) d)

	IF (@NumberOfDisease IS NULL)
		RETURN 0;
	ELSE
		RETURN @NumberOfDisease;

	RETURN 0;
END