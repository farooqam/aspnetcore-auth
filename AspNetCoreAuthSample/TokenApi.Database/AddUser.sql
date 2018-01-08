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
			
			DECLARE @errorMessageOutput NVARCHAR(4000);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			INSERT INTO [dbo].AuditEvents (
				[EventName],
				[EventDescription],
				[EventData],
				[UserId])
			VALUES (
				@eventName,
				'A user was attempted to be added but failed.',
				@errorMessageOutput,
				@executedByUserId);

			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
		END;
	END CATCH;
RETURN 0
