-- SCRIPT TO TRANSFORM DIACRITIC VIETNAMESE TO NON-DIACRITIC
-- SONNX
IF OBJECT_ID('[FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]', 'FN') IS NOT NULL
	DROP FUNCTION [FU_TRANSFORM_TO_NON_DIACRITIC_VIETNAMESE]
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

    DECLARE @l_str NVARCHAR(4000);
	SET @l_str = LTRIM(@strInput);
	SET @l_str = LOWER(RTRIM(@l_str));
 
	SET @l_str = REPLACE(@l_str, N'á', 'a');
 
	SET @l_str = REPLACE(@l_str, N'à', 'a');
	SET @l_str = REPLACE(@l_str, N'ả', 'a');
	SET @l_str = REPLACE(@l_str, N'ã', 'a');
	SET @l_str = REPLACE(@l_str, N'ạ', 'a');
 
	SET @l_str = REPLACE(@l_str, N'â', 'a');
	SET @l_str = REPLACE(@l_str, N'ấ', 'a');
	SET @l_str = REPLACE(@l_str, N'ầ', 'a');
	SET @l_str = REPLACE(@l_str, N'ẩ', 'a');
	SET @l_str = REPLACE(@l_str, N'ẫ', 'a');
	SET @l_str = REPLACE(@l_str, N'ậ', 'a');
 
	SET @l_str = REPLACE(@l_str, N'ă', 'a');
	SET @l_str = REPLACE(@l_str, N'ắ', 'a');
	SET @l_str = REPLACE(@l_str, N'ằ', 'a');
	SET @l_str = REPLACE(@l_str, N'ẳ', 'a');
	SET @l_str = REPLACE(@l_str, N'ẵ', 'a');
	SET @l_str = REPLACE(@l_str, N'ặ', 'a');
 
	SET @l_str = REPLACE(@l_str, N'é', 'e');
	SET @l_str = REPLACE(@l_str, N'è', 'e');
	SET @l_str = REPLACE(@l_str, N'ẻ', 'e');
	SET @l_str = REPLACE(@l_str, N'ẽ', 'e');
	SET @l_str = REPLACE(@l_str, N'ẹ', 'e');
 
	SET @l_str = REPLACE(@l_str, N'ê', 'e');
	SET @l_str = REPLACE(@l_str, N'ế', 'e');
	SET @l_str = REPLACE(@l_str, N'ề', 'e');
	SET @l_str = REPLACE(@l_str, N'ể', 'e');
	SET @l_str = REPLACE(@l_str, N'ễ', 'e');
	SET @l_str = REPLACE(@l_str, N'ệ', 'e');
 
	SET @l_str = REPLACE(@l_str, N'í', 'i');
	SET @l_str = REPLACE(@l_str, N'ì', 'i');
	SET @l_str = REPLACE(@l_str, N'ỉ', 'i');
	SET @l_str = REPLACE(@l_str, N'ĩ', 'i');
	SET @l_str = REPLACE(@l_str, N'ị', 'i');
 
	SET @l_str = REPLACE(@l_str, N'ó', 'o');
	SET @l_str = REPLACE(@l_str, N'ò', 'o');
	SET @l_str = REPLACE(@l_str, N'ỏ', 'o');
	SET @l_str = REPLACE(@l_str, N'õ', 'o');
	SET @l_str = REPLACE(@l_str, N'ọ', 'o');
 
	SET @l_str = REPLACE(@l_str, N'ô', 'o');
	SET @l_str = REPLACE(@l_str, N'ố', 'o');
	SET @l_str = REPLACE(@l_str, N'ồ', 'o');
	SET @l_str = REPLACE(@l_str, N'ổ', 'o');
	SET @l_str = REPLACE(@l_str, N'ỗ', 'o');
	SET @l_str = REPLACE(@l_str, N'ộ', 'o');
 
	SET @l_str = REPLACE(@l_str, N'ơ', 'o');
	SET @l_str = REPLACE(@l_str, N'ớ', 'o');
	SET @l_str = REPLACE(@l_str, N'ờ', 'o');
	SET @l_str = REPLACE(@l_str, N'ở', 'o');
	SET @l_str = REPLACE(@l_str, N'ỡ', 'o');
	SET @l_str = REPLACE(@l_str, N'ợ', 'o');
 
	SET @l_str = REPLACE(@l_str, N'ú', 'u');
	SET @l_str = REPLACE(@l_str, N'ù', 'u');
	SET @l_str = REPLACE(@l_str, N'ủ', 'u');
	SET @l_str = REPLACE(@l_str, N'ũ', 'u');
	SET @l_str = REPLACE(@l_str, N'ụ', 'u');
 
	SET @l_str = REPLACE(@l_str, N'ư', 'u');
	SET @l_str = REPLACE(@l_str, N'ứ', 'u');
	SET @l_str = REPLACE(@l_str, N'ừ', 'u');
	SET @l_str = REPLACE(@l_str, N'ử', 'u');
	SET @l_str = REPLACE(@l_str, N'ữ', 'u');
	SET @l_str = REPLACE(@l_str, N'ự', 'u');
 
	SET @l_str = REPLACE(@l_str, N'ý', 'y');
	SET @l_str = REPLACE(@l_str, N'ỳ', 'y');
	SET @l_str = REPLACE(@l_str, N'ỷ', 'y');
	SET @l_str = REPLACE(@l_str, N'ỹ', 'y');
	SET @l_str = REPLACE(@l_str, N'ỵ', 'y');
 
	SET @l_str = REPLACE(@l_str, N'đ', 'd');

    RETURN @strInput
END