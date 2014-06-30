-- SCRIPT TO LOAD A SPECIFIC DISTRICT BASE ON CITI_ID
-- SONNX
IF OBJECT_ID('[SP_LOAD_CITY_LIST]', 'P') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_CITY_LIST]
GO
CREATE PROCEDURE [dbo].[SP_LOAD_CITY_LIST]
AS
BEGIN
	SELECT City_ID, City_Name
	FROM City
END

-- SCRIPT TO LOAD ALL DISTRICTS BASE ON CITY
-- SONNX
IF OBJECT_ID('[SP_LOAD_DISTRICT_LIST]', 'P') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_DISTRICT_LIST]
GO
CREATE PROCEDURE [dbo].[SP_LOAD_DISTRICT_LIST]
	@CityID INT
AS
BEGIN
	SELECT District_ID, District_Name
	FROM District
	WHERE City_ID = @CityID
END

-- SCRIPT TO LOAD ALL DISEASES BASE ON SPECILITY
-- SONNX
IF OBJECT_ID('SP_LOAD_DISEASE_LIST', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_DISEASE_LIST
GO
CREATE PROCEDURE [dbo].[SP_LOAD_DISEASE_LIST]
	@SpecialityID INT
AS
BEGIN
	SELECT do.Disease_ID, do.Disease_Name
	FROM Disease do, Speciality_Disease sd
	WHERE sd.Speciality_ID = @SpecialityID AND
		  do.Disease_ID = sd.Disease_ID	  
END

-- SCRIPT TO LOAD DISEASES IN A SPECIFIC SPECIALITY
-- SONNX
IF OBJECT_ID('[SP_LOAD_DISEASE_IN_SPECIALITY]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_DISEASE_IN_SPECIALITY
GO
CREATE PROCEDURE [dbo].SP_LOAD_DISEASE_IN_SPECIALITY
	@SpecialityID INT
AS
BEGIN
	SELECT d.Disease_ID, d.Disease_Name
	FROM Speciality_Disease s, Disease d
	WHERE s.Speciality_ID = @SpecialityID AND
		  s.Disease_ID = d.Disease_ID
END

-- SCRIPT TO LOAD ALL SPECIALITIES
-- SONNX
IF OBJECT_ID('[SP_LOAD_SPECIALITY_LIST]', 'P') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_SPECIALITY_LIST]
GO
CREATE PROCEDURE [dbo].[SP_LOAD_SPECIALITY_LIST]
AS
BEGIN
	SELECT Speciality_ID, Speciality_Name
	FROM Speciality
END

-- SCRIPT TO LOAD ALL SPECIALITIES OF HOSPITAL
-- ANHDTH
IF OBJECT_ID('[SP_LOAD_SPECIALITY_BY_HOSPITALID]') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_SPECIALITY_BY_HOSPITALID]
GO
CREATE PROCEDURE [dbo].[SP_LOAD_SPECIALITY_BY_HOSPITALID]
	@Hospital_ID INT
AS
BEGIN
	SELECT	s.Speciality_ID,s.Speciality_Name
	FROM	Hospital_Speciality AS hs, Speciality AS s
	WHERE	@Hospital_ID=hs.Hospital_ID AND
			hs.Speciality_ID=s.Speciality_ID
END

-- SCRIPT TO LOAD ALL DOCTOR OF SPECIALITY
-- ANHDTH
IF OBJECT_ID('[SP_LOAD_DOCTOR_BY_SPECIALITYID]') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_DOCTOR_BY_SPECIALITYID]
GO
CREATE PROCEDURE [dbo].[SP_LOAD_DOCTOR_BY_SPECIALITYID]
	@SpecialityID int
AS
BEGIN
	SELECT	D.Doctor_ID,D.First_Name,D.Last_Name
	FROM	Doctor AS D, Doctor_Speciality AS DS
	WHERE	@SpecialityID = DS.Speciality_ID AND
			DS.Doctor_ID = D.Doctor_ID
END

-- SCRIPT TO INSERT APPOINTMENT
-- ANHDTH
IF OBJECT_ID('[SP_INSERT_APPOINTMENT]') IS NOT NULL
	DROP PROCEDURE [SP_INSERT_APPOINTMENT]
GO
CREATE PROCEDURE [dbo].[SP_INSERT_APPOINTMENT]
	@FullName nvarchar(32),
	@Gender bit,
	@Birthday date,
	@PhoneNo varchar(13),
	@Email varchar(64),
	@Date date,
	@Start_time time,
	@End_time time,
	@Doctor_ID int,
	@Hospital_ID int,
	@Confirm_Code varchar(8)
AS
BEGIN
	INSERT INTO Appointment
		([Patient_Full_Name]
		,[Patient_Gender]
		,[Patient_Birthday]
		,[Patient_Phone_Number]
		,[Patient_Email]
		,[Appointment_Date]
		,[Start_Time]
		,[End_Time]
		,[In_Charge_Doctor]
		,[Curing_Hospital]
		,[Confirm_Code])
	VALUES
		(@FullName
		,@Gender
		,@Birthday
		,@PhoneNo
		,@Email
		,@Date
		,@Start_time
		,@End_time
		,@Doctor_ID
		,@Hospital_ID
		,@Confirm_Code)
	IF @@ROWCOUNT >0
		RETURN 1
	ELSE
		RETURN 0
END