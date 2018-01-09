CREATE PROCEDURE [dbo].[FormatError]
	@errorMessage NVARCHAR(4000) OUTPUT
AS
	DECLARE 
				@errorNumber     INT,
				@errorSeverity   INT,
				@errorState      INT,
				@errorLine       INT,
				@errorProcedure  NVARCHAR(200);

			 SELECT 
				@errorNumber = ERROR_NUMBER(),
				@errorSeverity = ERROR_SEVERITY(),
				@errorState = ERROR_STATE(),
				@errorLine = ERROR_LINE(),
				@errorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

				SET @errorMessage = 'Error ' + CONVERT(varchar(50), @errorNumber) +
						  ', Severity ' + CONVERT(varchar(5), @errorSeverity) +
						  ', State ' + CONVERT(varchar(5), @errorState) + 
						  ', Procedure ' + ISNULL(@errorProcedure, '-') + 
						  ', Line ' + CONVERT(varchar(5), @errorLine) + 
						  ', Message ' + ERROR_MESSAGE();
RETURN 0;
