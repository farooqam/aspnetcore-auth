using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using TokenApi.Dal.Common;
using TokenApi.Dal.Sql;
using TokenApi.Security.Common;
using Xunit;

namespace TokenApi.Security.Sql.IntegrationTests
{
    public class SqlCredentialValidatorIntegrationTests
    {
        private readonly SqlCredentialValidatorSettings _sqlCredentialValidatorSettings;
        private readonly IHashingProvider _hashingProvider;
        private readonly ICredentialValidator _sqlCredentialValidator;
        private readonly IUserRepository _userRepository;
        private readonly SqlUserRepositorySettings _sqlUserRepositorySettings;
        private readonly Guid _userId = Guid.Parse("70e37905-c2db-4138-8412-73c489206837");

        public SqlCredentialValidatorIntegrationTests()
        {
            _sqlCredentialValidatorSettings = new SqlCredentialValidatorSettings
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString,
                QueryTimeout = TimeSpan.Parse(ConfigurationManager.AppSettings["QueryTimeout"])
            };

            _hashingProvider = new Sha256HashingProvider();
            _sqlCredentialValidator = new SqlCredentialValidator(_sqlCredentialValidatorSettings, _hashingProvider);

            _sqlUserRepositorySettings = new SqlUserRepositorySettings
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString,
                QueryTimeout = TimeSpan.Parse(ConfigurationManager.AppSettings["QueryTimeout"])
            };

            _userRepository = new SqlUserRepository(_sqlUserRepositorySettings);
        }

        [Fact]
        public async Task ValidateAsync_WhenCredentialsValid_Returns_True()
        {
            var userDto = new UserDto { Username = Guid.NewGuid().ToString("N") };
            var password = _hashingProvider.Hash("Hello123!!", true);

            try
            {
                await AddUserAsync(userDto);
                var addedUserDto = await _userRepository.GetUserAsync(userDto.Username, _userId);
                await AddCredentialAsync(addedUserDto.Id, password);
                var isValid = await _sqlCredentialValidator.ValidateAsync(addedUserDto.Id, password, _userId);

                isValid.Should().BeTrue();
            }
            finally
            {

            }
        }

        private async Task GetUserIdAsync(string username)
        {
            using (var connection = new SqlConnection(_sqlCredentialValidatorSettings.ConnectionString))
            {
                await connection.OpenAsync();

                connection.Query(
                    "[dbo].[AddUser]",
                    new
                    {
                        username = userDto.Username,
                        executedByUserId = _userId
                    },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: (int)_sqlCredentialValidatorSettings.QueryTimeout.TotalSeconds);
            }
        }

        private async Task AddUserAsync(UserDto userDto)
        {
            using (var connection = new SqlConnection(_sqlCredentialValidatorSettings.ConnectionString))
            {
                await connection.OpenAsync();

                connection.Query(
                    "[dbo].[AddUser]",
                    new
                    {
                        username = userDto.Username,
                        executedByUserId = _userId
                    },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: (int)_sqlCredentialValidatorSettings.QueryTimeout.TotalSeconds);
            }
        }

        private async Task AddCredentialAsync(Guid userId, string password)
        {
            using (var connection = new SqlConnection(_sqlCredentialValidatorSettings.ConnectionString))
            {
                await connection.OpenAsync();

                connection.Query(
                    "[dbo].[AddUserCredential]",
                    new
                    {
                        userId = userId,
                        password = password,
                        executedByUserId = _userId
                    },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: (int)_sqlCredentialValidatorSettings.QueryTimeout.TotalSeconds);
            }
        }

    }
}