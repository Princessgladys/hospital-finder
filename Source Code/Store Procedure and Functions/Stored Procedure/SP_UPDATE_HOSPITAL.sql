-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF AN USER HAVE BEEN EXISTED IN DATABSE
-- SONNX
IF OBJECT_ID('[SP_UPDATE_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_UPDATE_HOSPITAL
GO
CREATE PROCEDURE SP_UPDATE_HOSPITAL
	@HosptalID INT,
	@HospitalName NVARCHAR(64),
	@HospitalType INT,
	@Address NVARCHAR(128),
	@CityID INT,
	@DistrictID INT,
	@WardID INT,
	@PhoneNumber VARCHAR(64),
	@Fax VARCHAR(16),
	@Email VARCHAR(64),
	@Website VARCHAR(64),
	@HolidayStartTime NVARCHAR(7),
	@HolidayEndTime NVARCHAR(7),
	@OrdinaryStartTime NVARCHAR(7),
	@OrdinaryEndTime NVARCHAR(7),
	@Coordinate VARCHAR(26),
	@IsAllowAppointment BIT,
	@CreatedPerson INT,
	@FullDescription NVARCHAR(4000),
	@SpecialityList NVARCHAR(4000),
	@ServiceList NVARCHAR(4000),
	@FacilityList NVARCHAR(4000)
AS
BEGIN
	-- UPDATE HOSPITAL TABLE
	BEGIN
		UPDATE Hospital
		SET Hospital_Name = @HospitalName,
			Hospital_Type = @HospitalType,
			[Address] = @Address,
			City_ID = @CityID,
			District_ID = @DistrictID,
			Ward_ID = @WardID,
			Phone_Number = @PhoneNumber,
			Fax = @Fax,
			Email = @Email,
			Website = @Website,
			Holiday_Start_Time = @HolidayStartTime,
			Holiday_End_Time = @HolidayEndTime,
			Ordinary_Start_Time = @OrdinaryStartTime,
			Ordinary_End_Time = @OrdinaryEndTime,
			Is_Allow_Appointment = @IsAllowAppointment,
			Full_Description = @FullDescription,
			Coordinate = @Coordinate

		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN T;
			RETURN 0;
		END
	END
END