using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class SecurityKeyFactory : ISecurityKeyFactory
    {
        public async Task<SecurityKey> CreateSecurityKeyAsync(string secret)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            return secretKey;
        }
    }
}