using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                Password = "hello!"
            };

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
        public async Task Post_WhenCredentialsNotValid_Returns_BadRequest_Status()
        {
            // Arrange
            MockCredentialValidator.Setup(m => m.ValidateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var requestModel = new PostTokenRequestModel
            {
                Username = "farooq",
                Audience = "integration test",
                Password = "hello!"
            };

            // Act
            var response = await HttpClient.PostAsync("api/token", Stringify(requestModel));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResponseModel = JsonConvert.DeserializeObject<ApiErrors>(content);
            errorResponseModel.ErrorCount.Should().Be(1);

            var errorModel = errorResponseModel.Errors.Single();
            errorModel.Message.Should().Be("Could not create token. The username, password, and audience could not be verified.");
            errorModel.Code.Should().Be(2000);

            var errorResponseModelData = ((JObject)errorModel.Data).ToObject<PostTokenRequestModel>();
            errorResponseModelData.Audience.Should().Be(requestModel.Audience);
            errorResponseModelData.Username.Should().Be(requestModel.Username);
            errorResponseModelData.Password.Should().Be(requestModel.Password);


        }
    }
}