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
	SELECT	F.Facility_ID,F.Facility_Name
	FROM	Facility AS F,Hospital_Facility AS HF
	WHERE	F.Facility_ID = HF.Facility_ID AND
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
	SELECT	S.Service_ID,S.Service_Name
	FROM	Service AS S, Hospital_Service AS HS
	WHERE	S.Service_ID=HS.Service_ID AND
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