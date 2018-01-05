using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class ClaimsProvider : IClaimsProvider
    {
        public Task<IEnumerable<Claim>> GetUserClaimsAsync(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, "farooq_am@hotmail.com"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, "Developer")
            };

            return Task.FromResult(claims.AsEnumerable());
        }
    }
}