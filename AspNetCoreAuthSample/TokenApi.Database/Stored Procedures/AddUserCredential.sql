CREATE PROCEDURE [dbo].[AddUserCredential]
	@userId UNIQUEIDENTIFIER,
	@password NVARCHAR(256),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'Credential-Add [' +  CONVERT(NVARCHAR(36), @userId) + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO [dbo].[UserCredentials] (
				[UserId],
				[Password])
			VALUES (
				@userId,
				@password);

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A credential for a user was added.',
				@executedByUserId = @executedByUserId;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			SET @eventName = 'Credential-Add-Failed [' +  CONVERT(NVARCHAR(36), @userId) + ']';

			DECLARE @errorMessageOutput NVARCHAR(256);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A credential for a user was attempted to be added but failed.',
				@eventData = @errorMessageOutput,
				@executedByUserId = @executedByUserId;
			
			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
		END;
	END CATCH;
RETURN 0
