-- FUNCTION TO RETURN NUMBER OF RELATIVE DISEASE OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_NUMBER_OF_RELATIVE_DISEASE', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_NUMBER_OF_RELATIVE_DISEASE]
GO
CREATE FUNCTION [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE]
(
	@HospitalID INT,
	@WhatPhrase NVARCHAR(4000),
	@Mode INT
)
RETURNS INT
AS
BEGIN
	IF (@WhatPhrase IS NULL OR @WhatPhrase = '')
	BEGIN
		RETURN 0;
	END

	DECLARE @NumberOfDisease INT

	IF (@Mode = 0)
	BEGIN
		SELECT @NumberOfDisease = (SELECT COUNT(d.Disease_ID)
								   FROM Disease d, Speciality_Disease sd,
										Hospital h, Hospital_Speciality hs
								   WHERE h.Hospital_ID = @HospitalID AND
										 h.Hospital_ID = hs.Hospital_ID AND
										 hs.Speciality_ID = sd.Speciality_ID AND
										 sd.Disease_ID = d.Disease_ID AND
										 FREETEXT(Disease_Name, @WhatPhrase))
	END
	ELSE
	BEGIN
		SELECT @NumberOfDisease = (SELECT COUNT(d.Disease_ID)
								   FROM [NON_DIACRITIC_DISEASE] d, Speciality_Disease sd,
										Hospital h, Hospital_Speciality hs, Speciality s
								   WHERE h.Hospital_ID = @HospitalID AND
									     h.Hospital_ID = hs.Hospital_ID AND
									     hs.Speciality_ID = sd.Speciality_ID AND
									     s.Speciality_ID = sd.Speciality_ID AND
									     sd.Disease_ID = d.Disease_ID AND
										 FREETEXT(d.Disease_Name, N'ngoai'))
	END

	IF (@NumberOfDisease IS NULL)
		RETURN 0;
	ELSE
		RETURN @NumberOfDisease;

	RETURN 0;
END