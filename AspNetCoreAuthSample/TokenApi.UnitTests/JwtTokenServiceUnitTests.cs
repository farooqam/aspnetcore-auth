using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TokenApi.Security.Common;
using Xunit;

namespace TokenApi.UnitTests
{
    public class JwtTokenServiceUnitTests
    {
        [Fact]
        public async Task CreateTokenAsync_Creates_Token()
        {
            // Arrange
            var createTokenOptions = new CreateTokenOptions
            {
                Audience = "aud",
                Username = "usr",
                Password = "pwd"
            };

            var tokenServiceSettings = new TokenServiceSettings {Issuer = "issuer"};
            
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            var mockUserRepository = new Mock<IClaimsProvider>();
            var mockJwtTokenProvider = new Mock<IJwtTokenProvider>();
            var mockSecurityKeyProvider = new Mock<ISecurityKeyProvider>();
            
            var service = new JwtTokenService(
                mockSecretsProvider.Object,
                mockUserRepository.Object,
                mockJwtTokenProvider.Object,
                mockSecurityKeyProvider.Object,
                tokenServiceSettings);


            // Act
            await service.CreateTokenAsync(createTokenOptions);

            // Assert
            mockJwtTokenProvider.Verify(
                m => m.CreateTokenAsync(It.IsAny<IEnumerable<Claim>>(), It.IsAny<SecurityKey>(),
                    tokenServiceSettings.Issuer, createTokenOptions.Audience), Times.Exactly(1));
        }

        [Fact]
        public async Task CreateTokenAsync_When_Credentials_Not_Valid_Returns_Null()
        {
            // Arrange
            var createTokenOptions = new CreateTokenOptions
            {
                Audience = "aud",
                Username = "usr",
                Password = "pwd"
            };

            var tokenServiceSettings = new TokenServiceSettings {Issuer = "issuer"};

            var mockSecretsProvider = new Mock<ISecretsProvider>();
            var mockUserRepository = new Mock<IClaimsProvider>();
            var mockJwtTokenProvider = new Mock<IJwtTokenProvider>();
            var mockSecurityKeyProvider = new Mock<ISecurityKeyProvider>();
            
            var service = new JwtTokenService(
                mockSecretsProvider.Object,
                mockUserRepository.Object,
                mockJwtTokenProvider.Object,
                mockSecurityKeyProvider.Object,
                tokenServiceSettings);


            // Act
            var createTokenResult = await service.CreateTokenAsync(createTokenOptions);

            // Assert
            createTokenResult.Should().BeNull();
        }
    }
}