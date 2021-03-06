USE [master]
GO
/****** Object:  Database [HospitalF]    Script Date: 5/26/2014 10:26:55 PM ******/
CREATE DATABASE [HospitalF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HospitalF', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\HospitalF.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HospitalF_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\HospitalF_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HospitalF] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HospitalF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HospitalF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HospitalF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HospitalF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HospitalF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HospitalF] SET ARITHABORT OFF 
GO
ALTER DATABASE [HospitalF] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HospitalF] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HospitalF] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HospitalF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HospitalF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HospitalF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HospitalF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HospitalF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HospitalF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HospitalF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HospitalF] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HospitalF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HospitalF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HospitalF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HospitalF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HospitalF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HospitalF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HospitalF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HospitalF] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HospitalF] SET  MULTI_USER 
GO
ALTER DATABASE [HospitalF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HospitalF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HospitalF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HospitalF] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [HospitalF]
GO
/****** Object:  UserDefinedFunction [dbo].[FU_AUTO_GENERATE_ENTITIES_CLASS]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FU_AUTO_GENERATE_ENTITIES_CLASS]
(
	@TableName VARCHAR(MAX),
	@NameSpace VARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @result VARCHAR(MAX)

	SET @result = 'using System;' + CHAR(13) + CHAR(13)

	SET @result = @result + 'namespace ' +
		@NameSpace  + CHAR(13) + '{' + CHAR(13)

	SET @result = @result + 
		'	/// <summary>' + CHAR(13) +
		'	/// Class defines properties for ' + @TableName + ' table' + CHAR(13) +
		'	/// <summary>' + CHAR(13) +
		'	public class ' +
		REPLACE(@TableName, '_', '') + CHAR(13) + '	{' + CHAR(13)

	SET @result = @result + '		#region ' + @TableName + ' Properties' + CHAR(13) 

	SELECT @result = @result + CHAR(13) +
		   '		/// <summary>' + CHAR(13) +
		   '		/// Property for ' + ColumnName + CHAR(13) +
		   '		/// <summary>' + CHAR(13) +
		   '		public ' +
		   ColumnType + ' ' + ColumnName +
		   ' { get; set; }' + CHAR(13)
	FROM
	(
		SELECT 
			REPLACE(col.name, '_', '') ColumnName,
			column_id,
			CASE typ.name 
				WHEN 'bigint' THEN
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'long?' ELSE 'long' END
				WHEN 'binary' THEN 'byte[]'
				WHEN 'bit' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'bool?' ELSE 'bool' END            
				WHEN 'char' THEN 'string'
				WHEN 'date' THEN
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'DateTime?' ELSE 'DateTime' END                        
				WHEN 'datetime' THEN
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'DateTime?' ELSE 'DateTime' END                        
				WHEN 'datetime2' THEN  
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'DateTime?' ELSE 'DateTime' END                        
				WHEN 'datetimeoffset' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'DateTimeOffset?' ELSE 'DateTimeOffset' END                                    
				WHEN 'decimal' THEN  
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'decimal?' ELSE 'decimal' END                                    
				WHEN 'float' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'float?' ELSE 'float' END                                    
				WHEN 'image' THEN 'byte[]'
				WHEN 'int' THEN  
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'int?' ELSE 'int' END
				WHEN 'money' THEN
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'decimal?' ELSE 'decimal' END                                                
				WHEN 'nchar' THEN 'string'
				WHEN 'ntext' THEN 'string'
				WHEN 'numeric' THEN
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'decimal?' ELSE 'decimal' END                                                            
				WHEN 'nvarchar' THEN 'string'
				WHEN 'real' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'double?' ELSE 'double' END                                                                        
				WHEN 'smalldatetime' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'DateTime?' ELSE 'DateTime' END                                    
				WHEN 'smallint' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'int?' ELSE 'int'END            
				WHEN 'smallmoney' THEN  
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'decimal?' ELSE 'decimal' END                                                                        
				WHEN 'text' THEN 'string'
				WHEN 'time' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'TimeSpan?' ELSE 'TimeSpan' END                                                                                    
				WHEN 'timestamp' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'DateTime?' ELSE 'DateTime' END                                    
				WHEN 'tinyint' THEN 
					CASE col.IS_NULLABLE
						WHEN 'TRUE' THEN 'byte?' ELSE 'byte' END                                                
				WHEN 'uniqueidentifier' THEN 'Guid'
				WHEN 'varbinary' THEN 'byte[]'
				WHEN 'varchar' THEN 'string'
				ELSE 'Object'
			END ColumnType
		FROM sys.columns col join
			 sys.types typ
		ON col.system_type_id = typ.system_type_id AND
		   col.user_type_id = typ.user_type_id
		WHERE object_id = object_id(@TableName)
	) t
	ORDER BY column_id

	SET @result = @result + CHAR(13) + '		#endregion '+ CHAR(13) 

	SET @result = @result  + '	}' + CHAR(13) + '}'

	RETURN @result
END

GO
/****** Object:  UserDefinedFunction [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]
(
      @strInput NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN    
    IF @strInput IS NULL
		RETURN @strInput
    IF @strInput = ''
		RETURN @strInput

    DECLARE @RT NVARCHAR(4000)
    DECLARE @DIACRITIC_CHARS NCHAR(136)
    DECLARE @NON_DIACRITIC_CHARS NCHAR (136)
 
    SET @DIACRITIC_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
						   ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý
						   ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ
						   ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +
						   NCHAR(272) + NCHAR(208)

    SET @NON_DIACRITIC_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
							   iiiiiooooooooooooooouuuuuuuuuuyyyyy
							   AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII
							   OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 
    DECLARE @COUNTER INT
    DECLARE @COUNTER1 INT
    SET @COUNTER = 1
 
    WHILE (@COUNTER <= LEN(@strInput))
    BEGIN  
		SET @COUNTER1 = 1
		WHILE (@COUNTER1 <= LEN(@DIACRITIC_CHARS)+1)
		BEGIN
			IF UNICODE(SUBSTRING(@DIACRITIC_CHARS, @COUNTER1, 1)) =
			   UNICODE(SUBSTRING(@strInput,@COUNTER ,1))
				BEGIN          
					IF @COUNTER = 1
						SET @strInput = SUBSTRING(@NON_DIACRITIC_CHARS, @COUNTER1, 1) +
										SUBSTRING(@strInput, @COUNTER + 1,LEN(@strInput) - 1)                  
					ELSE
						SET @strInput = SUBSTRING(@strInput, 1, @COUNTER - 1) +
										SUBSTRING(@NON_DIACRITIC_CHARS, @COUNTER1,1) +
										SUBSTRING(@strInput, @COUNTER + 1, LEN(@strInput) - @COUNTER)
						BREAK
				END
			SET @COUNTER1 = @COUNTER1 + 1
		END
	SET @COUNTER = @COUNTER + 1
    END

    SET @strInput = REPLACE(@strInput, ' ', ' ')

    RETURN @strInput
END
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Appointment](
	[Appointment_ID] [int] IDENTITY(1,1) NOT NULL,
	[Patient_First_Name] [nvarchar](16) NULL,
	[Patient_Last_Name] [nvarchar](16) NULL,
	[Patient_Gender] [bit] NULL,
	[Patient_Birthday] [date] NULL,
	[Patient_Phone_Number] [varchar](13) NULL,
	[Appointment_Date] [date] NULL,
	[Start_Time] [time](7) NULL,
	[End_Time] [time](7) NULL,
	[In_Charge_Doctor] [int] NULL,
	[Curing_Hospital] [int] NULL,
	[Created_Person] [int] NULL,
	[Is_Active] [int] NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[Appointment_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[City]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[City_ID] [int] IDENTITY(1,1) NOT NULL,
	[City_Name] [nvarchar](32) NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[City_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Disease]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Disease](
	[Disease_ID] [int] IDENTITY(1,1) NOT NULL,
	[Disease_Name] [nvarchar](64) NULL,
 CONSTRAINT [PK_Disease] PRIMARY KEY CLUSTERED 
(
	[Disease_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[District]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[District_ID] [int] IDENTITY(1,1) NOT NULL,
	[District_Name] [nvarchar](32) NULL,
	[City_ID] [int] NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED 
(
	[District_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Doctor]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctor](
	[Doctor_ID] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [nvarchar](32) NULL,
	[Last_Name] [nvarchar](32) NULL,
	[Gender] [bit] NULL,
	[Speciality] [int] NULL,
	[Working_Hospital] [int] NULL,
	[Experience] [nvarchar](512) NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED 
(
	[Doctor_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[Feedback_ID] [int] IDENTITY(1,1) NOT NULL,
	[Feedback_Content] [nvarchar](256) NULL,
	[Created_Person] [int] NULL,
	[Feedback_Type] [int] NULL,
	[Hospital_ID] [int] NULL,
	[Created_Date] [datetime] NULL,
 CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[Feedback_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Feedback_Type]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback_Type](
	[Type_ID] [int] IDENTITY(1,1) NOT NULL,
	[Type_Name] [nvarchar](256) NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_Feedback_Type] PRIMARY KEY CLUSTERED 
(
	[Type_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Hospital]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hospital](
	[Hospital_ID] [int] IDENTITY(1,1) NOT NULL,
	[Hospital_Name] [nvarchar](64) NULL,
	[Address] [nvarchar](64) NULL,
	[District_ID] [int] NULL,
	[Phone_Number] [varchar](13) NULL,
	[Fax] [varchar](15) NULL,
	[Email] [varchar](64) NULL,
	[Website] [varchar](32) NULL,
	[Longitude] [varchar](7) NULL,
	[Latitude] [varchar](7) NULL,
	[Created_Person] [int] NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_Hospital] PRIMARY KEY CLUSTERED 
(
	[Hospital_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hospital_Speciality]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hospital_Speciality](
	[Hospital_ID] [int] NOT NULL,
	[Speciality_ID] [int] NOT NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_Hospital_Speciality] PRIMARY KEY CLUSTERED 
(
	[Hospital_ID] ASC,
	[Speciality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Photo]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Photo](
	[Photo_ID] [int] IDENTITY(1,1) NOT NULL,
	[File_Path] [varchar](128) NULL,
	[Add_Date] [datetime] NULL,
	[Hospital_ID] [int] NULL,
	[Uploaded_Person] [int] NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED 
(
	[Photo_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rating]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[Rating_ID] [int] IDENTITY(1,1) NOT NULL,
	[Score] [int] NULL,
	[Hospital_ID] [int] NULL,
	[Created_Person] [int] NULL,
 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
(
	[Rating_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Role_ID] [int] IDENTITY(1,1) NOT NULL,
	[Role_Name] [nvarchar](32) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Role_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SentenceDictionary]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SentenceDictionary](
	[Sentence_ID] [int] IDENTITY(1,1) NOT NULL,
	[Sentence] [nvarchar](64) NULL,
 CONSTRAINT [PK_SentenceDictionary] PRIMARY KEY CLUSTERED 
(
	[Sentence_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Speciality]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speciality](
	[Speciality_ID] [int] IDENTITY(1,1) NOT NULL,
	[Speciality_Name] [nvarchar](32) NULL,
 CONSTRAINT [PK_Speciality] PRIMARY KEY CLUSTERED 
(
	[Speciality_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Speciality_Disease]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speciality_Disease](
	[Speciality_ID] [int] NOT NULL,
	[Disease_ID] [int] NOT NULL,
 CONSTRAINT [PK_Speciality_Disease] PRIMARY KEY CLUSTERED 
(
	[Speciality_ID] ASC,
	[Disease_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](64) NULL,
	[Password] [nvarchar](32) NULL,
	[First_Name] [nvarchar](16) NULL,
	[Last_Name] [nvarchar](16) NULL,
	[Phone_Number] [varchar](13) NULL,
	[Role_ID] [int] NULL,
	[Created_Person] [int] NULL,
	[Hospital_ID] [int] NULL,
	[Is_Active] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WordDictionary]    Script Date: 5/26/2014 10:26:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WordDictionary](
	[Word_ID] [int] IDENTITY(1,1) NOT NULL,
	[Word] [nvarchar](8) NULL,
 CONSTRAINT [PK_WordDictionary] PRIMARY KEY CLUSTERED 
(
	[Word_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Role_ID], [Role_Name]) VALUES (1, N'Administrator')
INSERT [dbo].[Role] ([Role_ID], [Role_Name]) VALUES (2, N'Premium User')
INSERT [dbo].[Role] ([Role_ID], [Role_Name]) VALUES (3, N'Normal User')
SET IDENTITY_INSERT [dbo].[Role] OFF
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Doctor] FOREIGN KEY([In_Charge_Doctor])
REFERENCES [dbo].[Doctor] ([Doctor_ID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Doctor]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Hospital] FOREIGN KEY([Curing_Hospital])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Hospital]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_User] FOREIGN KEY([Created_Person])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_User]
GO
ALTER TABLE [dbo].[District]  WITH CHECK ADD  CONSTRAINT [FK_District_City] FOREIGN KEY([City_ID])
REFERENCES [dbo].[City] ([City_ID])
GO
ALTER TABLE [dbo].[District] CHECK CONSTRAINT [FK_District_City]
GO
ALTER TABLE [dbo].[Doctor]  WITH CHECK ADD  CONSTRAINT [FK_Doctor_Hospital] FOREIGN KEY([Working_Hospital])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[Doctor] CHECK CONSTRAINT [FK_Doctor_Hospital]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Feedback_Type] FOREIGN KEY([Feedback_Type])
REFERENCES [dbo].[Feedback_Type] ([Type_ID])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_Feedback_Type]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Hospital] FOREIGN KEY([Hospital_ID])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_Hospital]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_User] FOREIGN KEY([Created_Person])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_User]
GO
ALTER TABLE [dbo].[Hospital]  WITH CHECK ADD  CONSTRAINT [FK_Hospital_District] FOREIGN KEY([District_ID])
REFERENCES [dbo].[District] ([District_ID])
GO
ALTER TABLE [dbo].[Hospital] CHECK CONSTRAINT [FK_Hospital_District]
GO
ALTER TABLE [dbo].[Hospital]  WITH CHECK ADD  CONSTRAINT [FK_Hospital_User] FOREIGN KEY([Created_Person])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[Hospital] CHECK CONSTRAINT [FK_Hospital_User]
GO
ALTER TABLE [dbo].[Hospital_Speciality]  WITH CHECK ADD  CONSTRAINT [FK_Hospital_Speciality_Hospital] FOREIGN KEY([Hospital_ID])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[Hospital_Speciality] CHECK CONSTRAINT [FK_Hospital_Speciality_Hospital]
GO
ALTER TABLE [dbo].[Hospital_Speciality]  WITH CHECK ADD  CONSTRAINT [FK_Hospital_Speciality_Speciality] FOREIGN KEY([Speciality_ID])
REFERENCES [dbo].[Speciality] ([Speciality_ID])
GO
ALTER TABLE [dbo].[Hospital_Speciality] CHECK CONSTRAINT [FK_Hospital_Speciality_Speciality]
GO
ALTER TABLE [dbo].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Hospital] FOREIGN KEY([Hospital_ID])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[Photo] CHECK CONSTRAINT [FK_Photo_Hospital]
GO
ALTER TABLE [dbo].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_User] FOREIGN KEY([Uploaded_Person])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[Photo] CHECK CONSTRAINT [FK_Photo_User]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Hospital] FOREIGN KEY([Hospital_ID])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_Hospital]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_User] FOREIGN KEY([Created_Person])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_User]
GO
ALTER TABLE [dbo].[Speciality_Disease]  WITH CHECK ADD  CONSTRAINT [FK_Speciality_Disease_Disease] FOREIGN KEY([Disease_ID])
REFERENCES [dbo].[Disease] ([Disease_ID])
GO
ALTER TABLE [dbo].[Speciality_Disease] CHECK CONSTRAINT [FK_Speciality_Disease_Disease]
GO
ALTER TABLE [dbo].[Speciality_Disease]  WITH CHECK ADD  CONSTRAINT [FK_Speciality_Disease_Speciality] FOREIGN KEY([Speciality_ID])
REFERENCES [dbo].[Speciality] ([Speciality_ID])
GO
ALTER TABLE [dbo].[Speciality_Disease] CHECK CONSTRAINT [FK_Speciality_Disease_Speciality]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Hospital] FOREIGN KEY([Hospital_ID])
REFERENCES [dbo].[Hospital] ([Hospital_ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Hospital]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([Role_ID])
REFERENCES [dbo].[Role] ([Role_ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_User] FOREIGN KEY([Created_Person])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_User]
GO
USE [master]
GO
ALTER DATABASE [HospitalF] SET  READ_WRITE 
GO
