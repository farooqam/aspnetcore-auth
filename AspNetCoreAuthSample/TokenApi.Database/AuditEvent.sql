CREATE PROCEDURE [dbo].[AuditEvent]
	@eventName NVARCHAR(256),
	@eventDescription NVARCHAR(256),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @errorMessageOutput NVARCHAR(4000);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			INSERT INTO [dbo].AuditEvents (
				[EventName],
				[EventDescription],
				[EventData],
				[UserId])
			VALUES (
				@eventName,
				@eventDescription,
				@errorMessageOutput,
				@executedByUserId);

			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
RETURN 0
