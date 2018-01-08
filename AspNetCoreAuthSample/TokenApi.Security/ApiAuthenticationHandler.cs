﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class ApiAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ICredentialValidator _credentialValidator;
        public static readonly string SchemeName = "TokenApi-v1";

        public ApiAuthenticationHandler(
            ICredentialValidator credentialValidator,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _credentialValidator = credentialValidator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var credentials = await GetCredentialsFromBody();

            if (credentials == null)
            {
                return AuthenticateResult.Fail("Authentication failed.");
            }

            var credentialsValid = await _credentialValidator.ValidateAsync(
                credentials.Username, 
                credentials.Password);

            if (!credentialsValid)
            {
                return AuthenticateResult.Fail("Authentication failed.");
            }

            var identity = CreateIdentity(credentials);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), null, SchemeName);

            return AuthenticateResult.Success(ticket);
        }

        private static ClaimsIdentity CreateIdentity(dynamic credentials)
        {
            var identity = new ClaimsIdentity(SchemeName);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, credentials.Username));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, credentials.Audience));
            return identity;
        }

        private async Task<dynamic> GetCredentialsFromBody()
        {
            var initialBody = Context.Request.Body; // Workaround

            Context.Request.EnableRewind();
            var buffer = new byte[initialBody.Length];
            await Context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var json = Encoding.UTF8.GetString(buffer);

            Context.Request.Body = initialBody; // Workaround

            var credentialsDef = new {Username = string.Empty, Password = string.Empty, Audience = string.Empty};
            var credentials = JsonConvert.DeserializeAnonymousType(json, credentialsDef);
            return credentials;
        }
    }
}