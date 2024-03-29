﻿CREATE PROCEDURE [dbo].[AddSecret]
	@name NVARCHAR(256),
	@value NVARCHAR(256),
	@executedByUserId UNIQUEIDENTIFIER
AS
	DECLARE @eventName NVARCHAR(256);
	SET @eventName = 'Secret-Add [' +  CONVERT(NVARCHAR(36), @executedByUserId) + ']';

	BEGIN TRY
		BEGIN TRANSACTION
			INSERT INTO [dbo].[Secrets] (
				[Name],
				[Value])
				VALUES (
					@name,
					@value);

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'A secret was added.',
				@executedByUserId = @executedByUserId;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) <> 0
		BEGIN	
			ROLLBACK TRANSACTION;

			SET @eventName = 'Secret-Add-Failed [' +  CONVERT(NVARCHAR(36), @executedByUserId) + ']';

			DECLARE @errorMessageOutput NVARCHAR(256);

			EXEC [dbo].[FormatError] @errorMessage = @errorMessageOutput OUTPUT;

			EXEC [dbo].[AuditEvent] 
				@eventName = @eventName,
				@eventDescription = 'An secret was attempted to be added but failed.',
				@eventData = @errorMessageOutput,
				@executedByUserId = @executedByUserId;

			EXEC [dbo].[RaiseError] @errorMessage = @errorMessageOutput;
			
		END;
	END CATCH;
RETURN 0
