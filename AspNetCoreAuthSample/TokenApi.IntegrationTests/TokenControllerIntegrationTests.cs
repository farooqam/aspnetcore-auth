using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenApi.Common;
using TokenApi.Dal.Common;
using TokenApi.Security.Common;
using Xunit;

namespace TokenApi.IntegrationTests
{
    public class TokenControllerIntegrationTests : IntegrationTestsBase
    {
        [Fact]
        public async Task Post_Returns_Ok_Status_And_Token()
        {
            // Arrange
            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Password = "hello!"
            };
            
            var mockUserDto = new UserDto {Id = Guid.NewGuid()};
            MockUserRepository.Setup(m => m.GetUserAsync(requestModel.Username, WellKnownUserIds.TokenApi)).ReturnsAsync(mockUserDto);
            MockUserRepository.Setup(m => m.GetRegisteredApplicationAsync(requestModel.Username, requestModel.Audience)).ReturnsAsync(new RegisteredApplicationDto {RegisteredOwner = "userId"});
            MockCredentialValidator.Setup(m => m.ValidateAsync(mockUserDto.Id, requestModel.Password, It.IsAny<Guid>())).ReturnsAsync(true);

            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));
        
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<PostTokenResponseModel>(content);
            responseModel.Token.Should().NotBeNullOrEmpty();
            responseModel.Issuer.Should().Be("http://www.techniqly.com/token/v1");

            DateTimeOffset.FromUnixTimeSeconds(responseModel.ValidFrom).LocalDateTime.Should().BeBefore(DateTime.Now);
            DateTimeOffset.FromUnixTimeSeconds(responseModel.ValidTo).LocalDateTime.Should().BeAfter(DateTime.Now).And.BeBefore(DateTime.Now.AddDays(30));
        }

        [Fact]
        public async Task Post_WhenCredentialsNotValid_Returns_Unauthorized_Status()
        {
            // Arrange
            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Password = "hello!"
            };

            var mockUserDto = new UserDto { Id = Guid.NewGuid() };
            MockUserRepository.Setup(m => m.GetUserAsync(requestModel.Username, WellKnownUserIds.TokenApi)).ReturnsAsync(mockUserDto);
            MockCredentialValidator.Setup(m => m.ValidateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            
            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Post_WhenAudienceNotValid_Returns_Unauthorized_Status()
        {
            // Arrange
            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Password = "hello!"
            };

            var mockUserDto = new UserDto { Id = Guid.NewGuid() };
            MockUserRepository.Setup(m => m.GetUserAsync(requestModel.Username, WellKnownUserIds.TokenApi)).ReturnsAsync(mockUserDto);
            MockCredentialValidator.Setup(m => m.ValidateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);
            MockUserRepository.Setup(m => m.GetRegisteredApplicationAsync(requestModel.Username, requestModel.Audience)).ReturnsAsync(null as RegisteredApplicationDto);

            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Post_WhenUserNotValid_Returns_Unauthorized_Status()
        {
            // Arrange
            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Password = "hello!"
            };

            MockUserRepository.Setup(m => m.GetUserAsync(requestModel.Username, WellKnownUserIds.TokenApi)).ReturnsAsync(null as UserDto);

            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}