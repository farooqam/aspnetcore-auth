CREATE PROCEDURE [dbo].[GetUser]
	@userName NVARCHAR(256),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'User-Lookup [' +  CONVERT(NVARCHAR(36), @userName) + ']';

	BEGIN TRY
		SELECT [Id], [Username] FROM [dbo].[Users]	
		WHERE [Username] = @userName
		AND [IsActive] = 1;

		EXEC [dbo].[AuditEvent] 
					@eventName = @eventName,
					@eventDescription = 'A user lookup occurred.',
					@executedByUserId = @executedByUserId;
	END TRY
	BEGIN CATCH
		SET @eventName = 'User-Lookup-Failed [' +  CONVERT(NVARCHAR(36), @userName) + ']';

		DECLARE @errorMessageOutput NVARCHAR(256);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'An user lookup was attempted but failed.',
				@eventData = @errorMessageOutput,
				@executedByUserId = @executedByUserId;
			
			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
	END CATCH;
RETURN 0
