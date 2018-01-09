CREATE PROCEDURE [dbo].[DeactivateUser]
	@userId UNIQUEIDENTIFIER,
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'User-Deactivate [' +  CONVERT(NVARCHAR(36), @userId) + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			
			UPDATE [dbo].[Users]
			SET [IsActive] = 0
			WHERE [Id] = @userId;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A user was deactivated.',
				@executedByUserId = @executedByUserId;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			SET @eventName = 'User-Deactivate-Failed [' +  CONVERT(NVARCHAR(36), @userId) + ']';

			DECLARE @errorMessageOutput NVARCHAR(256);

				EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

				EXEC [dbo].[AuditEvent] 
					@eventName = @eventName,
					@eventDescription = 'An user deactivate was attempted but failed.',
					@eventData = @errorMessageOutput,
					@executedByUserId = @executedByUserId;
			
				EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
		END;
	END CATCH;
RETURN 0
