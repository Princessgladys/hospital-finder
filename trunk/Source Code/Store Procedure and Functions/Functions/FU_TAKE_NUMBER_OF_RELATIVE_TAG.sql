-- FUNCTION TO RETURN NUMBER OF RELATIVE TAGS OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_NUMBER_OF_RELATIVE_TAG', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_NUMBER_OF_RELATIVE_TAG]
GO
CREATE FUNCTION [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_TAG]
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

	DECLARE @NumberOfTag INT

	SELECT @NumberOfTag = (SELECT COUNT(*)
						   FROM (SELECT w.Word_ID
							     FROM Tag w, Tag_Hospital wh
							     WHERE w.[Type] = 2 AND
									   FREETEXT (w.Word, @WhatPhrase) AND
									   w.Word_ID = wh.Word_ID AND
									   wh.Hospital_ID = @HospitalID) w)

	IF (@NumberOfTag IS NULL)
		RETURN 0;
	ELSE
		RETURN @NumberOfTag;

	RETURN 0;
END