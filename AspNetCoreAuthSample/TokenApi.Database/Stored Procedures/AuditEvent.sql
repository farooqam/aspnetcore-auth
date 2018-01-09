CREATE PROCEDURE [dbo].[AuditEvent]
	@eventName NVARCHAR(256),
	@eventDescription NVARCHAR(256),
	@eventData NVARCHAR(MAX) = null,
	@executedByUserId UNIQUEIDENTIFIER
AS
			INSERT INTO [dbo].AuditEvents (
				[EventName],
				[EventDescription],
				[EventData],
				[UserId])
			VALUES (
				@eventName,
				@eventDescription,
				@eventData,
				@executedByUserId);
RETURN 0
