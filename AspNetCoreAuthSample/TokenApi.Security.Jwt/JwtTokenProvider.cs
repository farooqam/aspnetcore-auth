using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TokenApi.Security.Common;

namespace TokenApi.Security.Jwt
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        public Task<string> CreateTokenAsync(
            IEnumerable<Claim> claims,
            SecurityKey securityKey,
            string issuer,
            string audience)
        {
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }
    }
}