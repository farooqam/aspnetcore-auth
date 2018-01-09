CREATE PROCEDURE [dbo].[RaiseError]
	@errorMessage NVARCHAR(4000)
AS
	DECLARE 
				@errorNumber     INT,
				@errorSeverity   INT,
				@errorState      INT,
				@errorLine       INT,
				@errorProcedure  NVARCHAR(200);

	RAISERROR 
			(
				@errorMessage, 
				@errorSeverity, 
				1,               
				@errorNumber,    
				@errorSeverity,  
				@errorState,     
				@errorProcedure, 
				@errorLine    
			);
RETURN 0
