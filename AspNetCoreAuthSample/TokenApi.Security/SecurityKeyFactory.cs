using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class SecurityKeyFactory : ISecurityKeyProvider
    {
        public Task<SecurityKey> CreateSecurityKeyAsync(string secret)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            return Task.FromResult((SecurityKey)secretKey);
        }
    }
}