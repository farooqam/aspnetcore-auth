using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using TokenApi.Security.Common;

namespace TokenApi.Security.Sql
{
    public class SqlCredentialValidator : ICredentialValidator
    {
        private readonly SqlCredentialValidatorSettings _settings;
        private readonly IHashingProvider _hashingProvider;

        public SqlCredentialValidator(
            SqlCredentialValidatorSettings settings,
            IHashingProvider hashingProvider)
        {
            _settings = settings;
            _hashingProvider = hashingProvider;
        }

        public async Task<bool> ValidateAsync(Guid userId, string password, Guid executedByUserId)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("userName", userId);
                parameters.Add("password", _hashingProvider.Hash(password, true));
                parameters.Add("executedByUserId", executedByUserId);
                parameters.Add("found", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await connection.OpenAsync();

                await connection.QueryAsync(
                    "[dbo].[VerifyPassword]",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: (int) _settings.QueryTimeout.TotalSeconds);

                var found = parameters.Get<bool>("found");
                return found;
            }
        }
    }
}