-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
-- SCRIPT TO UPDATE SPECIALITY,FACILITY,SERVICE
-- ANHDTH
IF OBJECT_ID('[SP_UPDATE_SPECIALITY_SERVICE_FACILITY]', 'P') IS NOT NULL
	DROP PROCEDURE SP_UPDATE_SPECIALITY_SERVICE_FACILITY
GO
CREATE PROCEDURE SP_UPDATE_SPECIALITY_SERVICE_FACILITY
	@HospitalId int,
	@SpecialityList NVARCHAR(4000),
	@ServiceList NVARCHAR(4000),
	@FacilityList NVARCHAR(4000)
AS
BEGIN
	BEGIN TRANSACTION T
	-- INSERT TO HOSPITAL_SPECIALITY TABLE
		IF (@SpecialityList != '')
		BEGIN
			DECLARE @RowNumber INT = 1
			DECLARE @TotalToken  INT = 0;
			DECLARE @Token VARCHAR(4)
			DECLARE @Active bit='False';

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@SpecialityList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)
				SELECT @Active=	(SELECT	Is_Active
								FROM	Hospital_Speciality
								WHERE	Speciality_ID=@Token AND Hospital_ID=@HospitalId)
				IF(@Active IS NOT NULL)
					IF(@Active='False')
						UPDATE	Hospital_Speciality
						SET		Is_Active='True'
						WHERE	Hospital_ID=@HospitalId AND Speciality_ID=@Token
				IF(@Active IS NULL) 
					INSERT INTO Hospital_Speciality
					(
						Hospital_ID,
						Speciality_ID,
						Is_Main_Speciality,
						Is_Active
					)
					VALUES
					(
						@HospitalID,
						@Token,
						'False',
						'True')
				IF @@ERROR<> 0
				BEGIN
					ROLLBACK TRAN T;
					RETURN 0;
				END

				SET @RowNumber += 1
			END
		END
		
		-- INSERT TO SERVCE TABLE
		IF (@ServiceList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@ServiceList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)
				 SELECT @Active=	(SELECT	Is_Active
									FROM	Hospital_Service
									WHERE	Service_ID=@Token AND Hospital_ID=@HospitalId)
				IF(@Active IS NOT NULL)
					IF(@Active='False')
						UPDATE	Hospital_Service
						SET		Is_Active='True'
						WHERE	Hospital_ID=@HospitalId AND Service_ID=@Token
				ELSE
				INSERT INTO Hospital_Service
				(
					Hospital_ID,
					Service_ID,
					Is_Active
				)
				VALUES
				(
					@HospitalID,
					@Token,
					'True'
				)

				IF @@ERROR<> 0
				BEGIN
					ROLLBACK TRAN T;
					RETURN 0;
				END

				SET @RowNumber += 1
			END
		END

		-- INSERT TO FACILITY TABLE
		IF (@FacilityList != '')
		BEGIN
			SET @RowNumber = 1
			SET @TotalToken = 0

			SELECT @TotalToken = (SELECT COUNT(TokenList.ID)
								  FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList)

			WHILE (@RowNumber <= @TotalToken)
			BEGIN
				SELECT @Token = (SELECT TokenList.Token
								 FROM (SELECT ROW_NUMBER()
									   OVER (ORDER BY TokenList.ID ASC) AS RowNumber, TokenList.Token
									   FROM [dbo].[FU_STRING_TOKENIZE] (@FacilityList, '|') TokenList) AS TokenList
								 WHERE RowNumber = @RowNumber)
				SELECT @Active=	(SELECT	Is_Active
								FROM	Hospital_Facility
								WHERE	Facility_ID=@Token AND Hospital_ID=@HospitalId)
				IF(@Active IS NOT NULL)
					IF(@Active='False')
						UPDATE	Hospital_Facility
						SET		Is_Active='True'
						WHERE	Hospital_ID=@HospitalId AND Facility_ID=@Token
					ELSE
				INSERT INTO Hospital_Facility
				(
					Hospital_ID,
					Facility_ID,
					Is_Active
				)
				VALUES
				(
					@HospitalID,
					@Token,
					'True'
				)

				IF @@ERROR<> 0
				BEGIN
					ROLLBACK TRAN T;
					RETURN 0;
				END

				SET @RowNumber += 1
			END
		END
		
	COMMIT TRAN T;
	RETURN 1;
END