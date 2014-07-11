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

    DECLARE @RT NVARCHAR(4000)
    DECLARE @DIACRITIC_CHARS NCHAR(136)
    DECLARE @NON_DIACRITIC_CHARS NCHAR (136)
 
    SET @DIACRITIC_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý' +
						   N'ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +
						   NCHAR(272) + NCHAR(208)

    SET @NON_DIACRITIC_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyy' +
							   N'AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 
    DECLARE @COUNTER INT
    DECLARE @COUNTER1 INT
    SET @COUNTER = 1
 
    WHILE (@COUNTER <= LEN(@strInput))
    BEGIN  
		SET @COUNTER1 = 1
		WHILE (@COUNTER1 <= LEN(@DIACRITIC_CHARS) + 1)
		BEGIN
			IF UNICODE(SUBSTRING(@DIACRITIC_CHARS, @COUNTER1, 1)) =
			   UNICODE(SUBSTRING(@strInput,@COUNTER ,1))
				BEGIN          
					IF @COUNTER = 1
						SET @strInput = SUBSTRING(@NON_DIACRITIC_CHARS, @COUNTER1, 1) +
										SUBSTRING(@strInput, @COUNTER + 1,LEN(@strInput) - 1)                  
					ELSE
						SET @strInput = SUBSTRING(@strInput, 1, @COUNTER - 1) +
										SUBSTRING(@NON_DIACRITIC_CHARS, @COUNTER1, 1) +
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