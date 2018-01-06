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
        public Task<CreateTokenResult> CreateTokenAsync(
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
            
            return Task.FromResult(new CreateTokenResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Issuer = issuer,
                ValidTo = new DateTimeOffset(token.ValidTo).ToUnixTimeSeconds(),
                ValidFrom = new DateTimeOffset(token.ValidFrom).ToUnixTimeSeconds()
            }
        );
        }
    }
}