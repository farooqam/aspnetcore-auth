using System.Threading.Tasks;
using FluentAssertions;
using TokenApi.Security;
using Xunit;

namespace Token.Api.Security.UnitTests
{
    public class SecurityKeyProviderUnitTests
    {
        [Fact]
        public async Task CreateSecurityKeyAsync_Creates_Key()
        {
            var secret = "123";
            var provider = new SecurityKeyProvider();
            var key = await provider.CreateSecurityKeyAsync(secret);

            key.Should().NotBeNull();
        }
    }
}