using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace TokenApi.Security.Jwt.UnitTests
{
    public class JwtTokenProviderUnitTests
    {
        [Fact]
        public async Task CreateTokenAsync_Creates_Token()
        {
            var provider = new JwtTokenProvider();
            var secret = "1234567812345678";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var token = await provider.CreateTokenAsync(null, secretKey, null, null);

            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task CreateTokenAsync_When_Key_Too_Short_Throws_Exception()
        {
            var minLength = 16;
            var tooShortKey = new String('a', minLength - 1);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tooShortKey));
            var provider = new JwtTokenProvider();

            Func<Task> createToken = async () => await provider.CreateTokenAsync(null, secretKey, null, null);

            createToken.ShouldThrow<ArgumentOutOfRangeException>();

        }
    }
}