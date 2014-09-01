-- FUNCTION TO RETURN NUMBER OF RELATIVE TAGS OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_NUMBER_OF_RELATIVE_TAG', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_NUMBER_OF_RELATIVE_TAG]
GO
CREATE FUNCTION [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_TAG]
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

	DECLARE @NumberOfTag INT

	IF (@Mode = 0)
	BEGIN
		SELECT @NumberOfTag = (SELECT COUNT(w.Word_ID)
							   FROM Tag w, Tag_Hospital wh
							   WHERE wh.Hospital_ID = @HospitalID AND
									 w.Word_ID = wh.Word_ID AND
									 w.[Type] = 3 AND
									 FREETEXT (w.Word, @WhatPhrase))
	END
	ELSE
	BEGIN
		SELECT @NumberOfTag = (SELECT COUNT(w.Word_ID)
							   FROM [NON_DIACRITIC_TAG] w, Tag_Hospital wh
							   WHERE wh.Hospital_ID = @HospitalID AND
									 w.Word_ID = wh.Word_ID AND
									 (N'%' + w.Word + N'%' LIKE
								      N'%' + @WhatPhrase + N'%' OR
								      N'%' + @WhatPhrase + N'%' LIKE
								      N'%' + w.Word + N'%'))
	END
	
	IF (@NumberOfTag IS NULL)
		RETURN 0;
	ELSE
		RETURN @NumberOfTag;

	RETURN 0;
END