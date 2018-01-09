using FluentAssertions;
using Xunit;

namespace TokenApi.Security.UnitTests
{
    public class Sha256HashingProviderUnitTests
    {
        [Fact]
        public void Hash_HashesTheValue()
        {
            var provider = new Sha256HashingProvider();
            var valueToHash = "HelloWorld!!!";
            var hash = provider.Hash(valueToHash);
            var hashCaseInsensitive = provider.Hash(hash);

            hash.Should().NotBe(hashCaseInsensitive);
        }

        [Fact]
        public void Hash_CaseInsensitive_HashesTheValue()
        {
            var provider = new Sha256HashingProvider();
            var valueToHash = "HelloWorld!!!";
            var hash = provider.Hash(valueToHash, false);
            var valueToHashDifferentCase = "HELLOWOrld!!!";

            var hashCaseInsensitive = provider.Hash(valueToHashDifferentCase, false);

            hash.Should().Be(hashCaseInsensitive);
        }
    }
}