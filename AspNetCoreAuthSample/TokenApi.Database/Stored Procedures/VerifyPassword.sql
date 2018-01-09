CREATE PROCEDURE [dbo].[VerifyPassword]
	@userId UNIQUEIDENTIFIER,
	@password NVARCHAR(256),
	@executedByUserId UNIQUEIDENTIFIER,
	@found BIT OUTPUT
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'Password-Lookup [' +  CONVERT(NVARCHAR(36), @userId) + ']';
	SET @found = 0;

	BEGIN TRY
		IF EXISTS (SELECT [UserId] FROM [dbo].[UserCredentials]
					WHERE [UserId] = @userId 
					AND
					[Password] = @password)
		BEGIN
			SET @found = 1;
		END

		DECLARE @eventData NVARCHAR(MAX);
		SET @eventData =  'found = ' + CONVERT(NVARCHAR(1), @found);

		EXEC [dbo].[AuditEvent] 
					@eventName = @eventName,
					@eventDescription = 'A password lookup occurred.',
					@eventData = @eventData,
					@executedByUserId = @executedByUserId;
	END TRY
	BEGIN CATCH
			SET @eventName = 'Password-Lookup-Failed [' +  CONVERT(NVARCHAR(36), @userId) + ']';

			DECLARE @errorMessageOutput NVARCHAR(256);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'An password lookup was attempted but failed.',
				@eventData = @errorMessageOutput,
				@executedByUserId = @executedByUserId;
			
			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
	END CATCH;
RETURN 0
