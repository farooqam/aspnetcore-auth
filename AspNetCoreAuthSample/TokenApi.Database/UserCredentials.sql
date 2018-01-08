CREATE TABLE [dbo].[UserCredentials]
(
	[UserId] UNIQUEIDENTIFIER NOT NULL ,
	[Password] NVARCHAR(256) NOT NULL, 
    CONSTRAINT [FK_UserCredentials_UserId_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]), 
    PRIMARY KEY ([UserId], [Password])
)
