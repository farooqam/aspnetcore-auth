using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using TokenApi.Common;
using Xunit;

namespace TokenApi.IntegrationTests
{
    public class TokenControllerIntegrationTests : IntegrationTestsBase
    {
        [Fact]
        public async Task Post_Returns_Ok_Status_And_Token()
        {
            // Arrange
            MockCredentialValidator.Setup(m => m.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Issuer = "integration test library",
                Password = "hello!"
            };

            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<PostTokenResponseModel>(content);
            responseModel.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Post_WhenCredentialsNotValid_Returns_BadRequest_Status()
        {
            // Arrange
            MockCredentialValidator.Setup(m => m.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Issuer = "integration test library",
                Password = "hello!"
            };

            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}