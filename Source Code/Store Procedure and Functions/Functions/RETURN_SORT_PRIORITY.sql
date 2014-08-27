-- FUNCTION TO RETURN SORT PRIORITY IN NORMAL SEARCH MODE
-- SONNX
IF OBJECT_ID('RETURN_SORT_PRIORITY', 'FN') IS NOT NULL
	DROP FUNCTION [RETURN_SORT_PRIORITY]
GO
CREATE FUNCTION RETURN_SORT_PRIORITY
(
	@HospitalID INT,
	@Index INT,
	@WhatPhrase NVARCHAR(128)
)
RETURNS INT
AS
BEGIN
	DECLARE @Priority INT = 0

	-- DEFINE PRIORITY OF SORT ORDER
	DECLARE @PriorityOfRatingPoint INT = 1000
	DECLARE @PriorityOfRatingCount INT = 100

	DECLARE @ExactlyPriorityOfTag INT = 10000
	DECLARE @ExactlyPriorityOfSpeciality INT = 10000
	DECLARE @ExactlyPriorityOfDisease INT = 10000

	DECLARE @RelativePriorityOfTag INT = 100
	DECLARE @RelativePriorityOfSpeciality INT = 100
	DECLARE @RelativePriorityOfDisease INT = 100
	
	DECLARE @NonDiacriticPriorityOfTag INT = 10
	DECLARE @NonDiacriticPriorityOfSpeciality INT = 10
	DECLARE @NonDiacriticPriorityOfDisease INT = 10

	-- DEFINE INDEX NUMBER FOR SORT ORDER
	DECLARE @ExactlyIndexOfTag INT = 1
	DECLARE @ExactlyIndexOfSpeciality INT = 2
	DECLARE @ExactlyIndexOfDisease INT = 3

	DECLARE @RelativeIndexOfTag INT = 8
	DECLARE @RelativeIndexOfSpeciality INT = 9
	DECLARE @RelativeIndexOfDisease INT = 10
	
	DECLARE @NonDiacriticIndexOfTag INT = 11
	DECLARE @NonDiacriticIndexOfSpeciality INT = 12
	DECLARE @NonDiacriticIndexOfDisease INT = 13

	DECLARE @AdvancedSearch INT = 99

	-- RETURN PRIORITY OF EXACTLY TAG
	SELECT @Priority = 
		CASE @Index
			WHEN @ExactlyIndexOfTag THEN @ExactlyPriorityOfTag
			WHEN @ExactlyIndexOfSpeciality THEN @ExactlyPriorityOfSpeciality
			WHEN @ExactlyIndexOfDisease THEN @ExactlyPriorityOfDisease

			WHEN @RelativeIndexOfTag THEN
				[dbo].[FU_TAKE_NUMBER_OF_RELATIVE_TAG] (@HospitalID, @WhatPhrase) * @RelativePriorityOfTag
			WHEN @RelativeIndexOfSpeciality THEN
				[dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY] (@HospitalID, @WhatPhrase) * @RelativePriorityOfSpeciality
			WHEN @RelativeIndexOfDisease THEN
				[dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE] (@HospitalID, @WhatPhrase) * @RelativePriorityOfDisease

			WHEN @NonDiacriticIndexOfTag THEN
				[dbo].[FU_TAKE_NUMBER_OF_RELATIVE_TAG] (@HospitalID, @WhatPhrase) * @NonDiacriticPriorityOfTag
			WHEN @NonDiacriticIndexOfSpeciality
				THEN [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_SPECIALITY] (@HospitalID, @WhatPhrase) * @NonDiacriticPriorityOfSpeciality
			WHEN @NonDiacriticIndexOfDisease
				THEN [dbo].[FU_TAKE_NUMBER_OF_RELATIVE_DISEASE] (@HospitalID, @WhatPhrase) * @NonDiacriticPriorityOfDisease

			WHEN @AdvancedSearch THEN
				CONVERT(INT, [dbo].[FU_TAKE_RATING_POINT] (@HospitalID) * @PriorityOfRatingPoint) +
				CONVERT(INT, [dbo].[FU_TAKE_RATING_COUNT] (@HospitalID) * @PriorityOfRatingCount)
		END

	-- RETURN SORT PRIORTY
	RETURN @Priority
END