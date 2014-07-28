-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
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

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
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

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO LOAD ALL DOCTOR OF SPECIALITY
-- ANHDTH
IF OBJECT_ID('[SP_LOAD_DOCTOR_BY_SPECIALITYID]') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_DOCTOR_BY_SPECIALITYID]
GO
CREATE PROCEDURE [dbo].[SP_LOAD_DOCTOR_BY_SPECIALITYID]
	@SpecialityID int,
	@hospitalID int
AS
BEGIN
	SELECT	D.Doctor_ID,D.First_Name,D.Last_Name,
			D.Degree,D.Experience,D.Photo_ID,D.Working_Day
	FROM	Doctor AS D, Doctor_Speciality AS DS,Doctor_Hospital AS DH
	WHERE	@SpecialityID = DS.Speciality_ID AND
			DS.Doctor_ID = D.Doctor_ID AND
			DH.Hospital_ID = @hospitalID AND
			D.Is_Active=1
	GROUP BY D.Doctor_ID,D.First_Name,D.Last_Name,
			D.Degree,D.Experience,D.Photo_ID,D.Working_Day
END
-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
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

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIP TO LOAD TOP 10 RATED HOSPITALS
-- SONNX
IF OBJECT_ID('[SP_TAKE_TOP_10_RATED_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE [SP_TAKE_TOP_10_RATED_HOSPITAL]
GO
CREATE PROCEDURE SP_TAKE_TOP_10_RATED_HOSPITAL
AS
BEGIN
	SELECT TOP 10 h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, 
		   h.District_ID, h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website,
		   h.Start_Time, h.End_Time, h.Coordinate, h.Short_Description,
		   h.Full_Description, h.Is_Allow_Appointment,
		   [dbo].[FU_CALCULATE_AVERAGE_RATING](h.Hospital_ID) AS AverageScore
	FROM Hospital h
	WHERE Is_Active = 'True'
	ORDER BY ([dbo].[FU_CALCULATE_AVERAGE_RATING](h.Hospital_ID)) DESC
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO LOAD ALL DOCTOR OF HOSPITAL
-- ANHDTH


IF OBJECT_ID ('SP_LOAD_DOCTOR_IN_DOCTOR_HOSPITAL') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_DOCTOR_IN_DOCTOR_HOSPITAL]
GO
CREATE PROCEDURE SP_LOAD_DOCTOR_IN_DOCTOR_HOSPITAL
	@hospitalID INT
AS
BEGIN
	SELECT		D.Doctor_ID,D.First_Name,D.Last_Name,
				D.Degree,D.Experience,D.Working_Day,D.Photo_ID
	FROM		Doctor_Hospital AS DH,Doctor AS D
	WHERE		@hospitalID = DH.Hospital_ID AND
				DH.Doctor_ID = D.Doctor_ID AND
				D.Is_Active=1
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
--Procedure load all facilities of hospital
--ANHDTH
IF OBJECT_ID ('SP_LOAD_FACILITY_IN_HOSPITAL_FACILITY') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_FACILITY_IN_HOSPITAL_FACILITY]
GO
CREATE PROCEDURE SP_LOAD_FACILITY_IN_HOSPITAL_FACILITY
	@hospitalID INT
AS
BEGIN
	SELECT	F.Facility_ID,F.Facility_Name,F.Facility_Type,FT.[Type_Name]
	FROM	Facility AS F,Hospital_Facility AS HF,FacilityType AS FT
	WHERE	F.Facility_ID = HF.Facility_ID AND
			F.Facility_Type=FT.[Type_ID] AND
			HF.Hospital_ID=@hospitalID
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
--PROCEDURE TO LOAD ALL SERVICES OF HOSPITAL
--ANHDTH
IF OBJECT_ID('SP_LOAD_SERVICE_IN_HOSPITAL_SERVICE') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_SERVICE_IN_HOSPITAL_SERVICE]
GO
CREATE PROCEDURE SP_LOAD_SERVICE_IN_HOSPITAL_SERVICE
	@hospitalID int
AS
BEGIN
	SELECT	S.Service_ID,S.Service_Name,S.Service_Type,ST.[Type_Name]
	FROM	Service AS S, Hospital_Service AS HS,ServiceType AS ST
	WHERE	S.Service_ID=HS.Service_ID AND
			S.Service_Type=ST.[Type_ID] AND
			HS.Hospital_ID=@hospitalID
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
--PROCEDURE TO LOAD HOSPITAL TYPE
--ANHDTH
IF OBJECT_ID('SP_LOAD_TYPE_OF_HOSPITAL') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_TYPE_OF_HOSPITAL]
GO
CREATE PROCEDURE SP_LOAD_TYPE_OF_HOSPITAL
	@hospitalID int
AS
BEGIN
	SELECT	T.Type_ID,T.Type_Name
	FROM	HospitalType AS T, Hospital AS H
	WHERE	T.Type_ID=H.Hospital_Type AND
			H.Hospital_ID=@hospitalID
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
--PROCEDURE TO SEARCH DOCTOR BY DOCTOR NAME AND SPECIALITY
--ANHDTH

IF OBJECT_ID('SP_SEARCH_DOCTOR') IS NOT NULL
	DROP PROCEDURE [SP_SEARCH_DOCTOR]
GO
CREATE PROCEDURE SP_SEARCH_DOCTOR
	@doctorName NVARCHAR(64),
	@specialityID INT,
	@hospitalID INT
AS
BEGIN
	IF(@doctorName='')
		SET @doctorName=NULL
	IF(@specialityID=0)
		SET @specialityID=NULL
		
	IF(@doctorName IS NULL)
		BEGIN
			SELECT	D.Doctor_ID,D.First_Name,D.Last_Name,
					D.Degree,D.Experience,D.Photo_ID,D.Working_Day
			FROM	Doctor AS D, Doctor_Speciality AS DS,Doctor_Hospital AS DH
			WHERE	@SpecialityID = DS.Speciality_ID AND
					DS.Doctor_ID = D.Doctor_ID AND
					DH.Hospital_ID = @hospitalID AND
					D.Is_Active=1
			GROUP BY D.Doctor_ID,D.First_Name,D.Last_Name,
					D.Degree,D.Experience,D.Photo_ID,D.Working_Day
		END
	ELSE
		BEGIN
			IF(@specialityID IS NULL)
				BEGIN
					SELECT	D.Doctor_ID,D.First_Name,D.Last_Name,
							D.Degree,D.Experience,D.Photo_ID,D.Working_Day
					FROM	Doctor AS D,Doctor_Hospital AS DH
					WHERE	D.Last_Name+' '+D.First_Name LIKE N'%'+@doctorName+'%' AND
							DH.Hospital_ID = @hospitalID AND
							D.Doctor_ID=dh.Doctor_ID AND
							D.Is_Active=1
					GROUP BY D.Doctor_ID,D.First_Name,D.Last_Name,
							D.Degree,D.Experience,D.Photo_ID,D.Working_Day,DH.Hospital_ID
					RETURN;
				END
			ELSE -- search doctor by doctor name and speciality
				BEGIN
					SELECT	*
					FROM	
						(SELECT	D.Doctor_ID,D.First_Name,D.Last_Name,
								D.Degree,D.Experience,D.Photo_ID,D.Working_Day
						FROM	Doctor AS D, Doctor_Speciality AS DS,Doctor_Hospital AS DH
						WHERE	@SpecialityID = DS.Speciality_ID AND
								DS.Doctor_ID = D.Doctor_ID AND
								DH.Hospital_ID = @hospitalID AND
								D.Is_Active=1
						GROUP BY D.Doctor_ID,D.First_Name,D.Last_Name,
								D.Degree,D.Experience,D.Photo_ID,D.Working_Day) AS T
					WHERE		T.Last_Name+' '+T.First_Name LIKE N'%'+@doctorName+'%' 
					RETURN;
				END
		END
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHANGE STATUS OF A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('SP_CHANGE_HOSPITAL_STATUS', 'P') IS NOT NULL
	DROP PROCEDURE [SP_CHANGE_HOSPITAL_STATUS]
GO
CREATE PROCEDURE SP_CHANGE_HOSPITAL_STATUS
	@HospitalID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CurrentStatus BIT = (SELECT Is_Active
								  FROM Hospital
								  WHERE Hospital_ID = @HospitalID)

	IF (@CurrentStatus = 'True')
	BEGIN
		UPDATE Hospital
		SET Is_Active = 'False'
		WHERE Hospital_ID = @HospitalID
		RETURN @@ROWCOUNT;
	END
	ELSE
	BEGIN
		UPDATE Hospital
		SET Is_Active = 'True'
		WHERE Hospital_ID = @HospitalID
		RETURN @@ROWCOUNT;
	END

	RETURN 0;
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF AN USER HAVE BEEN EXISTED IN DATABSE
-- SONNX
IF OBJECT_ID('[SP_CHECK_VALID_USER_IN_CHARGED]', 'P') IS NOT NULL
	DROP PROCEDURE SP_CHECK_VALID_USER_IN_CHARGED
GO
CREATE PROCEDURE SP_CHECK_VALID_USER_IN_CHARGED
	@Email NVARCHAR(64)
AS
BEGIN
	IF (EXISTS(SELECT u.[User_ID]
			   FROM [User] u
			   WHERE u.Email = @Email AND
					 u.Hospital_ID IS NULL AND
					 u.Is_Active = 'True'))
		RETURN 1;
	ELSE
	BEGIN
		IF (EXISTS(SELECT u.[User_ID]
				   FROM [User] u
				   WHERE u.Email = @Email AND
						 u.Hospital_ID IS NOT NULL AND
						 u.Is_Active = 'True'))
			RETURN 2;
		ELSE
			RETURN 0;
	END
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
--PROCEDURE TO GET ALL SPECIALITY OF DOCTOR
--ANHDTH

IF OBJECT_ID('SP_LOAD_SPECIALITY_IN_DOCTOR_SPECIALITY') IS NOT NULL
	DROP PROCEDURE [SP_LOAD_SPECIALITY_IN_DOCTOR_SPECIALITY]
GO
CREATE PROCEDURE SP_LOAD_SPECIALITY_IN_DOCTOR_SPECIALITY
	@doctorID int
AS
BEGIN
	SELECT	S.Speciality_ID,S.Speciality_Name
	FROM	Speciality AS S,Doctor_Speciality AS DS
	WHERE	S.Speciality_ID=DS.Speciality_ID AND
			DS.Doctor_ID=@doctorID AND DS.Is_Active=1
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT LOAD ALL SERVICES AND ITS TYPES
-- SONNX
IF OBJECT_ID('[SP_TAKE_SERVICE_AND_TYPE]', 'P') IS NOT NULL
	DROP PROCEDURE SP_TAKE_SERVICE_AND_TYPE
GO
CREATE PROCEDURE SP_TAKE_SERVICE_AND_TYPE
AS
BEGIN
	SELECT s.Service_ID, s.[Service_Name], t.[Type_Name], t.[Type_ID]
	FROM [Service] s, ServiceType t
	WHERE s.Service_Type = t.[Type_ID]
	ORDER BY t.[Type_ID] ASC
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT LOAD ALL FACILITIES AND ITS TYPES
-- SONNX
IF OBJECT_ID('[SP_TAKE_FACILITY_AND_TYPE]', 'P') IS NOT NULL
	DROP PROCEDURE SP_TAKE_FACILITY_AND_TYPE
GO
CREATE PROCEDURE SP_TAKE_FACILITY_AND_TYPE
AS
BEGIN
	SELECT f.Facility_ID, f.Facility_Name, t.[Type_Name], t.[Type_ID]
	FROM Facility f, FacilityType t
	WHERE f.Facility_Type = t.[Type_ID]
	ORDER BY t.[Type_ID] ASC
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO CHECK IF THERE IS A SIMILAR HOSPITAL WITH NAME AND ADDRESS
-- ARE EQUAL WITH GIVEN DATA FROM USER
-- SONNX
IF OBJECT_ID('[SP_CHECK_NOT_DUPLICATED_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_CHECK_NOT_DUPLICATED_HOSPITAL
GO
CREATE PROCEDURE SP_CHECK_NOT_DUPLICATED_HOSPITAL
	@CityID INT,
	@DistrictID INT,
	@WardID INT,
	@Address NVARCHAR(64)
AS
BEGIN
	IF (EXISTS(SELECT Hospital_ID
			   FROM Hospital
			   WHERE City_ID = @CityID AND
				     District_ID = @DistrictID AND
				     Ward_ID = @WardID AND
				     [dbo].[FU_REMOVE_WHITE_SPACE]([Address]) LIKE
					 N'%' + [dbo].[FU_REMOVE_WHITE_SPACE](@Address) + N'%'))
		RETURN 0;
	ELSE
		RETURN 1;
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO INSERT NEW HOSPITAL USER
-- SONNX
IF OBJECT_ID('[SP_INSERT_HOSPITAL_USER]', 'P') IS NOT NULL
	DROP PROCEDURE SP_INSERT_HOSPITAL_USER
GO
CREATE PROCEDURE SP_INSERT_HOSPITAL_USER
	@Email NVARCHAR(64),
	@SecondaryEmail NVARCHAR(64),
	@Password NVARCHAR(32),
	@FirstName NVARCHAR(16),
	@LastName NVARCHAR(16),
	@PhoneNumber NVARCHAR(16),
	@ConfirmPerson INT
AS
BEGIN
	INSERT INTO [User]
	(
		Email,
		[Password],
		Secondary_Email,
		First_Name,
		Last_Name,
		Role_ID,
		Confirmed_Person,
		Phone_Number,
		Is_Active
	)
	VALUES
	(
		@Email,
		@Password,
		@SecondaryEmail,
		@FirstName,
		@LastName,
		3,
		@ConfirmPerson,
		@PhoneNumber,
		'True'
	)

	IF @@ROWCOUNT = 0
		RETURN 0;
	ELSE
		RETURN 1;
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO LOAD A SPECIFIC HOSPITAL
-- SONNX
IF OBJECT_ID('[SP_LOAD_SPECIFIC_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_LOAD_SPECIFIC_HOSPITAL
GO
CREATE PROCEDURE SP_LOAD_SPECIFIC_HOSPITAL
	@HospitalID INT
AS
BEGIN
	SELECT h.Hospital_ID, h.Hospital_Name, h.Hospital_Type, h.[Address], h.Ward_ID, h.District_ID,
		   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
		   h.OrDinary_End_Time, h.Holiday_Start_Time, h.Holiday_End_Time, h.Rating_Count,
		   h.Coordinate, h.Short_Description, h.Full_Description, h.Rating,
		   h.Is_Allow_Appointment, h.Is_Active, h.Created_Person,
		   c.City_Name, d.[Type] + ' '+  d.District_Name AS District_Name,
		   w.[Type] + ' '  + w.Ward_Name AS Ward_Name
	FROM Hospital h, City c, District d, Ward w
	WHERE h.Hospital_ID = @HospitalID AND
		  h.City_ID = c.City_ID AND
		  h.District_ID = d.District_ID AND
		  h.Ward_ID = w.Ward_ID
END