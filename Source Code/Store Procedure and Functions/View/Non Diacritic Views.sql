-- TAG -----------------------------------------------------------------------------
IF OBJECT_ID ('[dbo].[NON_DIACRITIC_TAG]', 'V') IS NOT NULL
	DROP VIEW [dbo].[NON_DIACRITIC_TAG];
GO
CREATE VIEW [dbo].[NON_DIACRITIC_TAG]
WITH SCHEMABINDING
AS
SELECT Word_ID,
	   [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Word) AS Word
FROM [dbo].[Tag]
WHERE [Type] = 3

CREATE UNIQUE CLUSTERED INDEX [NON_DIACRITIC_INDEX]
ON [dbo].[NON_DIACRITIC_TAG] (Word_ID)
GO    

-- SPECIALITY ----------------------------------------------------------------------
IF OBJECT_ID ('[dbo].[NON_DIACRITIC_SPECIALITY]', 'V') IS NOT NULL
	DROP VIEW [dbo].[NON_DIACRITIC_SPECIALITY];
GO
CREATE VIEW [dbo].[NON_DIACRITIC_SPECIALITY]
WITH SCHEMABINDING
AS
SELECT Speciality_ID,
	   [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Speciality_Name) AS Speciality_Name
FROM [dbo].[Speciality]

CREATE UNIQUE CLUSTERED INDEX [NON_DIACRITIC_INDEX]
ON [dbo].[NON_DIACRITIC_SPECIALITY] (Speciality_ID)
GO

-- DISEASE -------------------------------------------------------------------------
IF OBJECT_ID ('[dbo].[NON_DIACRITIC_DISEASE]', 'V') IS NOT NULL
	DROP VIEW [dbo].[NON_DIACRITIC_DISEASE];
GO
CREATE VIEW [dbo].[NON_DIACRITIC_DISEASE]
WITH SCHEMABINDING
AS
SELECT Disease_ID,
	   [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE](Disease_Name) AS Disease_Name
FROM [dbo].[Disease]

CREATE UNIQUE CLUSTERED INDEX [NON_DIACRITIC_INDEX]
ON [dbo].[NON_DIACRITIC_DISEASE] (Disease_ID)
GO