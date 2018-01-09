CREATE PROCEDURE [dbo].[AddUser]
	@username NVARCHAR(256),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'User-Add [' + @username + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO [dbo].[Users] (Username) VALUES (@username);

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A user was added.',
				@executedByUserId = @executedByUserId;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			SET @eventName = 'User-Add-Failed [' + @username + ']';

			DECLARE @errorMessageOutput NVARCHAR(256);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A user was attempted to be added but failed.',
				@eventData = @errorMessageOutput,
				@executedByUserId = @executedByUserId;
			
			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
		END;
	END CATCH;
RETURN 0
