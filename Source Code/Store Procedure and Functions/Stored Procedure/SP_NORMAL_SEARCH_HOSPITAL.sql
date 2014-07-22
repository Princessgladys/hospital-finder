-- SCRIPT TO SEARCH HOSPITALS
-- USING NORMAL OPTION
-- SONNX
IF OBJECT_ID('[SP_NORMAL_SEARCH_HOSPITAL]', 'P') IS NOT NULL
	DROP PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
GO
CREATE PROCEDURE SP_NORMAL_SEARCH_HOSPITAL
	@WhatPhrase NVARCHAR(128), -- ALWAYS NOT NULL
	@CityID INT,
	@DistrictName NVARCHAR(32)
AS
BEGIN
	-- DEFINE @WherePhrase
	DECLARE @WherePhrase INT
	IF (@CityID != 0 AND @DistrictName IS NOT NULL)
		SET @WherePhrase = 1
	ELSE
		SET @WherePhrase = 0

	-- DEFAULT VALUE TO CONSIDER ANALYZING @WhatPhrase
	DECLARE @Boundary INT = 30

	-- DECLARE TEMPORARY TABLE THAT CONTAIN LIST OF HOSPITALS
	DECLARE @HospitalList TABLE([Priority] INT IDENTITY(1,1) PRIMARY KEY,
							    Hospital_ID INT)

	-- DECLARE VARIABLE TO COUNT NUMBER OF RECORDS IN @HospitalList
	DECLARE @TotalRecordInHospitalList INT =
		(SELECT wh.Hospital_ID
		 FROM WordDictionary w, Word_Hospital wh
		 WHERE w.Word LIKE N'%' + @WhatPhrase + N'%' AND
			   w.[Priority] != 1 AND
			   w.Word_ID = wh.Word_ID)

	IF (@TotalRecordInHospitalList > 0)
	BEGIN
		IF (@WherePhrase = 1)
		BEGIN
			INSERT INTO @HospitalList
			SELECT (SELECT h.Hospital_ID
					FROM Hospital h, District d,
						 (SELECT wh.Hospital_ID
						  FROM WordDictionary w, Word_Hospital wh
						  WHERE w.Word LIKE N'%' + @WhatPhrase + N'%' AND
								w.[Priority] != 1 AND
								w.Word_ID = wh.Word_ID) w
					WHERE h.City_ID = @CityID AND
						  d.District_Name = @DistrictName AND
						  d.District_ID = h.District_ID AND
						  h.Hospital_ID = w.Hospital_ID)
		END
		ELSE
		BEGIN
			-- ADD THE HOSPITALS THAT HAVE CITY CONTAINS IN @WherePhrase
			IF (@CityID != 0)
			BEGIN
				INSERT INTO @HospitalList
				SELECT (SELECT h.Hospital_ID
						FROM Hospital h, 
							 (SELECT wh.Hospital_ID
							  FROM WordDictionary w, Word_Hospital wh
							  WHERE w.Word LIKE N'%' + @WhatPhrase + N'%' AND
									w.[Priority] != 1 AND
									w.Word_ID = wh.Word_ID) w
						WHERE h.City_ID = @CityID AND
							  w.Hospital_ID = h.Hospital_ID)
			END

			-- ADD THE HOSPITALS THAT HAVE DISTRICT CONTAINS IN @WherePhrase
			IF (@DistrictName IS NOT NULL)
			BEGIN
				INSERT INTO @HospitalList
				SELECT (SELECT h.Hospital_ID
						FROM District d, Hospital h,
							 (SELECT wh.Hospital_ID
							  FROM WordDictionary w, Word_Hospital wh
							  WHERE w.Word LIKE N'%' + @WhatPhrase + N'%' AND
									w.[Priority] != 1 AND
									w.Word_ID = wh.Word_ID) w
						WHERE d.District_Name = @DistrictName AND
							  d.District_ID = h.District_ID AND
							  w.Hospital_ID = h.Hospital_ID)
			END
		END
	END

	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.OrDinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END
	
-------------------------------------------------------------------------------------------

	-- ADD THE HOSPITALS THAT HAVE NAME CONTAINS IN @WhatPhrase
	INSERT INTO @HospitalList
	SELECT (SELECT h.Hospital_ID
			FROM Hospital h
			WHERE h.Hospital_Name LIKE
				  @WhatPhrase)

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------

	-- ADD THE HOSPITALS THAT HAVE SPECIALITY CONTAINS IN @WhatPhrase
	INSERT INTO @HospitalList
	SELECT (SELECT h.Hospital_ID
			FROM Speciality s, Hospital h, Hospital_Speciality hs
			WHERE s.Speciality_Name LIKE
				  @WhatPhrase AND
				  s.Speciality_ID = hs.Speciality_ID AND
				  h.Hospital_ID = hs.Hospital_ID)

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------

	-- CHECK IF @WherePhrase IS AVAILABLE
	IF (@WherePhrase = 1)
	BEGIN
		INSERT INTO @HospitalList
		SELECT (SELECT h.Hospital_ID
				FROM Hospital h, District d
				WHERE h.City_ID = @CityID AND
					  d.District_Name = @DistrictName AND
					  d.District_ID = h.District_ID)
	END
	ELSE
	BEGIN
		-- ADD THE HOSPITALS THAT HAVE CITY CONTAINS IN @WherePhrase
		IF (@CityID != 0)
		BEGIN
			INSERT INTO @HospitalList
			SELECT (SELECT h.Hospital_ID
					FROM Hospital h
					WHERE h.City_ID = @CityID)
		END

		-- ADD THE HOSPITALS THAT HAVE DISTRICT CONTAINS IN @WherePhrase
		IF (@DistrictName IS NOT NULL)
		BEGIN
			INSERT INTO @HospitalList
			SELECT (SELECT h.Hospital_ID
					FROM District d, Hospital h
					WHERE d.District_Name = @DistrictName AND
						  d.District_ID = h.District_ID)
		END
	END

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	

	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END


-------------------------------------------------------------------------------------------

	-- ADD THE HOSPITALS THAT HAVE DISEASE CONTAINS IN @WhatPhrase
	INSERT INTO @HospitalList
	SELECT (SELECT h.Hospital_ID
			FROM Disease d, Speciality_Disease sd,
					Hospital h, Hospital_Speciality hs
			WHERE d.Disease_Name LIKE
				  @WhatPhrase AND
				  d.Disease_ID = sd.Disease_ID AND
				  sd.Speciality_ID = hs.Speciality_ID AND
				  h.Hospital_ID = hs.Hospital_ID)

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
				h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
				h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------

	-- TRANSFORM @WhatPhrase TO NON-DIACRITIC VIETNAMSE
	SET @WhatPhrase = [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (@WhatPhrase)

	-- HOPITAL NAME
	INSERT INTO @HospitalList
	SELECT (SELECT wh.Hospital_ID
			FROM WordDictionary w, Word_Hospital wh
			WHERE w.Word LIKE N'%' + @WhatPhrase + N'%' AND
				  w.[Priority] != 1 AND
				  w.Word_ID = wh.Word_ID)

	-- DECLARE VARIABLE TO COUNT NUMBER OF RECORDS IN @HospitalList
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)

	-- CHECK IF THERE ARE LESS THAN 30 RECORD IN @TotalHospital
	-- IF MORE THAN 30, QUERY DATA IN [Hospital] TABLE AND RETURN
	-- IF NOT, CONTINUE ANALYZING @WhatPhrase AND @WherePhrase
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END
	
-------------------------------------------------------------------------------------------

	-- ADD THE HOSPITALS THAT HAVE NAME CONTAINS IN @WhatPhrase
	INSERT INTO @HospitalList
	SELECT (SELECT h.Hospital_ID
			FROM Hospital h
			WHERE [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](h.Hospital_Name) LIKE
				  @WhatPhrase)

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Ordinary_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------

	-- ADD THE HOSPITALS THAT HAVE SPECIALITY CONTAINS IN @WhatPhrase
	INSERT INTO @HospitalList
	SELECT (SELECT h.Hospital_ID
			FROM Speciality s, Hospital h, Hospital_Speciality hs
			WHERE [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](s.Speciality_Name) LIKE
				  @WhatPhrase AND
				  s.Speciality_ID = hs.Speciality_ID AND
				  h.Hospital_ID = hs.Hospital_ID)

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------

	-- CHECK IF @WherePhrase IS AVAILABLE
	SET @DistrictName = [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (@DistrictName)

	IF (@WherePhrase = 1)
	BEGIN
		INSERT INTO @HospitalList
		SELECT (SELECT h.Hospital_ID
				FROM Hospital h, District d
				WHERE h.City_ID = @CityID AND
					  [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (d.District_Name)
					  LIKE @DistrictName AND
					  d.District_ID = h.District_ID)
	END
	ELSE
	BEGIN
		-- ADD THE HOSPITALS THAT HAVE CITY CONTAINS IN @WherePhrase
		IF (@CityID != 0)
		BEGIN
			INSERT INTO @HospitalList
			SELECT (SELECT h.Hospital_ID
					FROM Hospital h
					WHERE h.City_ID = @CityID)
		END

		-- ADD THE HOSPITALS THAT HAVE DISTRICT CONTAINS IN @WherePhrase
		IF (@DistrictName IS NOT NULL)
		BEGIN
			INSERT INTO @HospitalList
			SELECT (SELECT h.Hospital_ID
					FROM District d, Hospital h
					WHERE [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE] (d.District_Name)
						  LIKE @DistrictName AND
						  d.District_ID = h.District_ID)
		END
	END

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
									  FROM @HospitalList)
	

	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
			   h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
			   h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
			   h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
			   h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------

	-- ADD THE HOSPITALS THAT HAVE DISEASE CONTAINS IN @WhatPhrase
	INSERT INTO @HospitalList
	SELECT (SELECT h.Hospital_ID
			FROM Disease d, Speciality_Disease sd,
					Hospital h, Hospital_Speciality hs
			WHERE [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](d.Disease_Name) LIKE
				  @WhatPhrase AND
				  d.Disease_ID = sd.Disease_ID AND
				  sd.Speciality_ID = hs.Speciality_ID AND
				  h.Hospital_ID = hs.Hospital_ID)

	-- COUNT NUMBER OF RECORD IN @HospitalList AGAIN
	SET @TotalRecordInHospitalList = (SELECT COUNT([Priority])
										FROM @HospitalList)
	
	IF (@TotalRecordInHospitalList > @Boundary)
	BEGIN
		SELECT h.Hospital_ID, h.Hospital_Name, h.[Address], h.Ward_ID, h.District_ID,
				h.City_ID, h.Phone_Number, h.Fax, h.Email, h.Website, h.Ordinary_Start_Time,
				h.Holiday_End_Time, h.Coordinate, h.Short_Description, h.Full_Description,
				h.Is_Allow_Appointment, h.Is_Active, h.Holiday_Start_Time, h.Holiday_End_Time,
				h.Rating
		FROM Hospital h, @HospitalList list
		WHERE h.Hospital_ID = list.Hospital_ID AND
			  h.Is_Active = 'True'
		ORDER BY list.[Priority] ASC
		RETURN;
	END

-------------------------------------------------------------------------------------------
END