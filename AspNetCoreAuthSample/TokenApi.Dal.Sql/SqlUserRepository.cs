using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TokenApi.Dal.Common;

namespace TokenApi.Dal.Sql
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly SqlUserRepositorySettings _settings;

        public SqlUserRepository(SqlUserRepositorySettings settings)
        {
            _settings = settings;
        }

        public async Task<UserDto> GetUserAsync(string username, Guid executedByUserId)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                await connection.OpenAsync();

                var userDto = (await connection.QueryAsync<UserDto>(
                    "[dbo].[GetUser]",
                    new {userName = username, executedByUserId},
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: (int) _settings.QueryTimeout.TotalSeconds)).SingleOrDefault();

                return userDto;
            }
        }

        public Task<RegisteredApplicationDto> GetRegisteredApplicationAsync(string username, string applicationName)
        {
            throw new NotImplementedException();
        }
    }
}