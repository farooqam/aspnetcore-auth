using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TokenApi.Dal.Common;
using TokenApi.Security.Common;
using Xunit;

namespace TokenApi.IntegrationTests
{
    public class JwtTokenServiceIntegrationTests
    {
        [Fact]
        public async Task CreateTokenAsync_Creates_Token()
        {
            // Arrange
            var createTokenOptions = new CreateTokenOptions
            {
                Audience = "aud",
                Issuer = "iss",
                Username = "usr",
                Password = "pwd"
            };

            var mockCredentialValidator = new Mock<ICredentialValidator>();
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockJwtTokenProvider = new Mock<IJwtTokenProvider>();
            var mockSecurityKeyProvider = new Mock<ISecurityKeyProvider>();

            mockCredentialValidator
                .Setup(m => m.ValidateAsync(createTokenOptions.Username, createTokenOptions.Password))
                .ReturnsAsync(true);

            var service = new JwtTokenService(
                mockCredentialValidator.Object,
                mockSecretsProvider.Object,
                mockUserRepository.Object,
                mockJwtTokenProvider.Object,
                mockSecurityKeyProvider.Object);


            // Act
            await service.CreateTokenAsync(createTokenOptions);

            // Assert
            mockJwtTokenProvider.Verify(
                m => m.CreateTokenAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<SecurityKey>(),
                    createTokenOptions.Issuer, createTokenOptions.Audience), Times.Exactly(1));
        }

        [Fact]
        public async Task CreateTokenAsync_When_Credentials_Not_Valid_Returns_Null()
        {
            // Arrange
            var createTokenOptions = new CreateTokenOptions
            {
                Audience = "aud",
                Issuer = "iss",
                Username = "usr",
                Password = "pwd"
            };

            var mockCredentialValidator = new Mock<ICredentialValidator>();
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockJwtTokenProvider = new Mock<IJwtTokenProvider>();
            var mockSecurityKeyProvider = new Mock<ISecurityKeyProvider>();

            mockCredentialValidator
                .Setup(m => m.ValidateAsync(createTokenOptions.Username, createTokenOptions.Password))
                .ReturnsAsync(false);

            var service = new JwtTokenService(
                mockCredentialValidator.Object,
                mockSecretsProvider.Object,
                mockUserRepository.Object,
                mockJwtTokenProvider.Object,
                mockSecurityKeyProvider.Object);


            // Act
            var token = await service.CreateTokenAsync(createTokenOptions);

            // Assert
            token.Should().BeNullOrEmpty();
        }
    }
}