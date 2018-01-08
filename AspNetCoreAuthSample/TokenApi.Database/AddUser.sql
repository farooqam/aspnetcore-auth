﻿CREATE PROCEDURE [dbo].[AddUser]
	@username NVARCHAR(20),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'Users-Add [' + @username + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO [dbo].[Users] (Username) VALUES (@username);

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A user was added.',
				@executedByUserId = @executedByUserId;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A user was attempted to be added but failed.',
				@executedByUserId = @executedByUserId;
			
		END;
	END CATCH;
RETURN 0
