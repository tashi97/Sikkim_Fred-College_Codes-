SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS P_READ_USER_BY_USER_NAME
GO

CREATE PROCEDURE [P_READ_USER_BY_USER_NAME]
	@USER_NAME NVARCHAR(50)	
AS
BEGIN TRY
	SELECT [USER_ID]
      ,[USER_NAME]
      ,[PASSWORD]
      ,[IS_LOGGED_IN]
      ,[LAST_LOGIN_DATE]
      ,[ACTIVE]
      ,[CREATE_DATE]
      ,[LAST_SCH_NO]
      ,[STEP]
      ,[TYPE]
      ,[IS_ADMIN]
      ,[DEPT_ID]
      ,[IS_DDO]
      ,[IS_SUPER]
      ,[Email]
      ,[IS_RCO]
      ,[ddocode]
  FROM [TBL_USER]
  WHERE
		[USER_NAME] = @USER_NAME
END TRY 

BEGIN CATCH
	DECLARE @ERR  VARCHAR(1000)
	SELECT @ERR=ERROR_MESSAGE()
	IF	ERROR_SEVERITY () > 12
		BEGIN
			RAISERROR (@ERR, 16, 20)
		END
		--
		ELSE
		--
		BEGIN
			RAISERROR (@ERR, 12, 2)
		END
END CATCH

GO
