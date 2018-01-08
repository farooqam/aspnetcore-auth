﻿CREATE TABLE [dbo].[AuditEvents]
(
	[EventId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[EventName] NVARCHAR(256) NOT NULL,
	[Timestamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[EventDescription] NVARCHAR(256) NOT NULL,
	[EventData] NVARCHAR(MAX) NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL
)
