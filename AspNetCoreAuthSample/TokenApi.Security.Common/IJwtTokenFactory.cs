using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TokenApi.Security.Common
{
    public interface IJwtTokenFactory
    {
        Task<string> CreateTokenAsync(IEnumerable<Claim> claims, SecurityKey securityKey, string issuer, string audience);
    }
}