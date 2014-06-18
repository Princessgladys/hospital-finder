-- FUNCTION TO GENERATE C# ENTITIES CLASS
-- SONNX
IF OBJECT_ID('[FU_AUTO_GENERATE_ENTITIES_CLASS]', 'F') IS NOT NULL
	DROP FUNCTION [FU_AUTO_GENERATE_ENTITIES_CLASS]
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
		REPLACE(@TableName, '_', '') + 'Entity' + CHAR(13) + '	{' + CHAR(13)

	SET @result = @result + '		#region ' + @TableName + ' Properties' + CHAR(13) 

	SELECT @result = @result + CHAR(13) +
		   '		/// <summary>' + CHAR(13) +
		   '		/// Property for ' + orgColName + ' attribute' + CHAR(13) +
		   '		/// <summary>' + CHAR(13) +
		   '		public ' +
		   ColumnType + ' ' + ColumnName + ' { get; set; }' + CHAR(13)
	FROM
	(
		SELECT REPLACE(col.name, '_', '') ColumnName,
			   col.name orgColName, column_id,
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