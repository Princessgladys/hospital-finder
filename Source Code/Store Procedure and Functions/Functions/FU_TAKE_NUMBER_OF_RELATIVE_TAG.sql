-- FUNCTION TO RETURN NUMBER OF TAGS OF A SPECIFIC HOSPITAL
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
	DECLARE @NumberOfTag INT

	SELECT @NumberOfTag = (SELECT COUNT(*)
						   FROM (SELECT w.Word_ID
							     FROM WordDictionary w, Word_Hospital wh
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