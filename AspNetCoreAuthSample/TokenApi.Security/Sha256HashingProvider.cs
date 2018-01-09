using System.Security.Cryptography;
using System.Text;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class Sha256HashingProvider : IHashingProvider
    {
        public string Hash(string value, bool caseSensitive = true)
        {
            var sha256 = SHA256.Create();
            var valueToHash = value;

            if (!caseSensitive)
            {
                valueToHash = value.ToLowerInvariant();
            }

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
            var hash = new StringBuilder();

            foreach (var b in bytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}