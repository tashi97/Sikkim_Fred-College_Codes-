SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS P_RCO_REG_UPDATE_STATUS
GO
  
CREATE PROCEDURE [dbo].[P_RCO_REG_UPDATE_STATUS]  
 @RCO_REG_ID BIGINT,
 @STATUS BIT,
 @UPDATEDBY INT NULL

AS  
BEGIN TRY

 SET NOCOUNT ON;

 UPDATE [RCO_REGISTRATION] 
	SET [CUR_STATUS] = @STATUS,
		[PASSED_BY] = @UPDATEDBY,
		[PASSING_TIME] = GETDATE()
	WHERE
		[REG_ID] = @RCO_REG_ID 
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


