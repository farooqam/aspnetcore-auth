﻿CREATE TABLE [dbo].[Secrets]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[Name] NVARCHAR(256) NOT NULL UNIQUE,
	[Value] NVARCHAR(256) NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT getutcdate(), 
    [LastUpdated] DATETIME NOT NULL DEFAULT getutcdate()
)
