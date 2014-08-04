-- FUNCTION TO RETURN RATING POINT OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('FU_TAKE_RATING_POINT', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TAKE_RATING_POINT]
GO
CREATE FUNCTION [dbo].[FU_TAKE_RATING_POINT]
(
	@HospitalID INT
)
RETURNS FLOAT
AS
BEGIN
	DECLARE @RatingPoint FLOAT

	SELECT @RatingPoint = (SELECT h.Rating
						   FROM Hospital h
						   WHERE h.Hospital_ID = @HospitalID)
	IF (@RatingPoint IS NULL)
		RETURN 0;
	ELSE
		RETURN @RatingPoint;

	RETURN 0;
END