CREATE PROCEDURE [dbo].[AddApplication]
	@name NVARCHAR(256),
	@registerTo UNIQUEIDENTIFIER,
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'Application-Add [' +  CONVERT(NVARCHAR(36), @registerTo) + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO [dbo].[RegisteredApplications] (
				[Name],
				[RegisteredTo])
			VALUES (
				@name,
				@registerTo
			);

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'An application was added.',
				@executedByUserId = @executedByUserId;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'An application was attempted to be added but failed.',
				@executedByUserId = @executedByUserId;
			
		END;
	END CATCH;
RETURN 0
