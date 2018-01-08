CREATE TABLE [dbo].[RegisteredApplications]
(
	[Id] UNIQUEIDENTIFIER NOT NULL  DEFAULT NEWID(),
	[Name] NVARCHAR(256) NOT NULL,
	[RegisteredTo] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY ([Id], [RegisteredTo]), 
    CONSTRAINT [FK_RegisteredApplications_UserId_Users_Id] FOREIGN KEY ([RegisteredTo]) REFERENCES [Users]([Id])
)

GO

CREATE UNIQUE INDEX [IX_RegisteredApplications_Name_RegisteredTo] ON [dbo].[RegisteredApplications] ([Name], [RegisteredTo])
