-- FUNCTION TO RETURN NUMBER OF RELATIVE SPECIALITY OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY]
GO
CREATE FUNCTION [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY]
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

	DECLARE @NumberOfSpeciality INT

	IF (@Mode = 0)
	BEGIN
		SELECT @NumberOfSpeciality = (SELECT COUNT(s.Speciality_ID)
									  FROM Speciality s, Hospital h, Hospital_Speciality hs
									  WHERE h.Hospital_ID = @HospitalID AND
											h.Hospital_ID = hs.Hospital_ID AND
											hs.Speciality_ID = s.Speciality_ID AND
											FREETEXT (Speciality_Name, @WhatPhrase))
	END
	ELSE
	BEGIN
		SELECT @NumberOfSpeciality = (SELECT COUNT(s.Speciality_ID)
									  FROM Speciality s, Hospital h, Hospital_Speciality hs
									  WHERE h.Hospital_ID = @HospitalID AND
											h.Hospital_ID = hs.Hospital_ID AND
											hs.Speciality_ID = s.Speciality_ID AND
											(N'%' + @WhatPhrase + N'%' LIKE 
										     N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) + N'%' OR
										     N'%' + [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) + N'%' LIKE
										     N'%' + @WhatPhrase + N'%'))
	END


	IF (@NumberOfSpeciality IS NULL)
		RETURN 0;
	ELSE
		RETURN @NumberOfSpeciality;

	RETURN 0;
END