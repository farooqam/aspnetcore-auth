using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using TokenApi.Dal.Common;
using Xunit;

namespace TokenApi.Dal.Sql.IntegrationTests
{
    public class SqlUserRepositoryIntegrationTests
    {
        private readonly SqlUserRepositorySettings _settings;
        private readonly IUserRepository _userRepository;
        private readonly Guid _userId = Guid.Parse("1a8c5089-af61-4ab1-a942-55f08d6e2e13");

        public SqlUserRepositoryIntegrationTests()
        {
            _settings = new SqlUserRepositorySettings
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString,
                QueryTimeout = TimeSpan.Parse(ConfigurationManager.AppSettings["QueryTimeout"])
            };

            _userRepository = new SqlUserRepository(_settings);
        }

        [Fact]
        public async Task GetUser_GetsTheUser()
        {
            var expectedDto = new UserDto {Username = Guid.NewGuid().ToString("N")};

            try
            {
                await AddUserAsync(expectedDto);
                var actualDto = await _userRepository.GetUserAsync(expectedDto.Username, _userId);

                actualDto.Username.Should().Be(expectedDto.Username);
                actualDto.Id.Should().NotBe(Guid.Empty);
            }
            finally
            {
                await DeleteUserAsync(expectedDto.Username);
            }
        }

        [Fact]
        public async Task GetUser_WhenNotFound_ReturnsNull()
        {
            var userDto = await _userRepository.GetUserAsync(Guid.NewGuid().ToString("N"), _userId);
            userDto.Should().BeNull();
        }

        private async Task AddUserAsync(UserDto userDto)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
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
                    commandTimeout: (int) _settings.QueryTimeout.TotalSeconds);
            }
        }

        private async Task DeleteUserAsync(string userName)
        {
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                await connection.OpenAsync();

                connection.Query(
                    "DELETE FROM [dbo].[Users] WHERE Username = @username",
                    new
                    {
                        username = userName
                    },
                    commandTimeout: (int) _settings.QueryTimeout.TotalSeconds);
            }
        }
    }
}