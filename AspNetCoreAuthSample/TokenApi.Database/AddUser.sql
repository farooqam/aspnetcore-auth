CREATE PROCEDURE [dbo].[AddUser]
	@username NVARCHAR(20),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'Users-Add [' + @username + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO [dbo].[Users] (Username) VALUES (@username);

			INSERT INTO [dbo].AuditEvents (
				[EventName],
				[EventDescription],
				[UserId])
			VALUES (
				@eventName,
				'A user was added.',
				@executedByUserId);

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			DECLARE 
				@errorMessage    NVARCHAR(4000),
				@errorNumber     INT,
				@errorSeverity   INT,
				@errorState      INT,
				@errorLine       INT,
				@errorProcedure  NVARCHAR(200);

			 SELECT 
				@errorNumber = ERROR_NUMBER(),
				@errorSeverity = ERROR_SEVERITY(),
				@errorState = ERROR_STATE(),
				@errorLine = ERROR_LINE(),
				@errorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

				SELECT @errorMessage = 'Error ' + CONVERT(varchar(50), @errorNumber) +
						  ', Severity ' + CONVERT(varchar(5), @errorSeverity) +
						  ', State ' + CONVERT(varchar(5), @errorState) + 
						  ', Procedure ' + ISNULL(@errorProcedure, '-') + 
						  ', Line ' + CONVERT(varchar(5), @errorLine) + 
						  ', Message ' + ERROR_MESSAGE();


			INSERT INTO [dbo].AuditEvents (
				[EventName],
				[EventDescription],
				[EventData],
				[UserId])
			VALUES (
				@eventName,
				'A user was attempted to be added but failed.',
				@errorMessage,
				@executedByUserId);

			RAISERROR 
			(
				@errorMessage, 
				@errorSeverity, 
				1,               
				@errorNumber,    
				@errorSeverity,  
				@errorState,     
				@errorProcedure, 
				@errorLine    
			);
		END;
	END CATCH;
RETURN 0
