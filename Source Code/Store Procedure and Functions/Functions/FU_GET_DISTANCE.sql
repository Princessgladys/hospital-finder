-- FUNCTION TO GET DISTANCE BETWEEN 2 LOCATIONS
-- SONNX
IF OBJECT_ID('FU_GET_DISTANCE', 'F') IS NOT NULL
	DROP FUNCTION FU_GET_DISTANCE
GO
CREATE FUNCTION [dbo].[FU_GET_DISTANCE]
(
	@Latitude1 FLOAT,
	@Longitude1 FLOAT,
	@Latitude2 FLOAT,
	@Longitude2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
	DECLARE @Result FLOAT

	DECLARE @R FLOAT
	SET @R = 6371000

	DECLARE @LatitudeDistance FLOAT
	SET @LatitudeDistance = [dbo].[FU_GET_RADIUS](@Latitude2 - @Latitude1)

	DECLARE @LongitudeDistance FLOAT
	SET @LongitudeDistance = [dbo].[FU_GET_RADIUS](@Longitude2 - @Latitude2)

	DECLARE @A FLOAT
	SET @A = SIN(@LatitudeDistance / 2) * SIN(@LatitudeDistance / 2) +
			 COS([dbo].[FU_GET_RADIUS](@Latitude1)) * COS([dbo].[FU_GET_RADIUS](@Latitude1)) *
			 SIN(@LongitudeDistance / 2) * SIN(@LongitudeDistance / 2)

	DECLARE @C FLOAT
	SEt @C = 2 * ATN2(SQRT(@A), SQRT(1 - @A))

	SET @Result = @R * @C
	RETURN @Result
END