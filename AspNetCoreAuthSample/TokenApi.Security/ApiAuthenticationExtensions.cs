using System;
using Microsoft.AspNetCore.Authentication;

namespace TokenApi.Security
{
    public static class ApiAuthenticationExtensions
    {
        public static AuthenticationBuilder AddCustomAuth(
            this AuthenticationBuilder builder,
            Action<AuthenticationSchemeOptions> configureOptions)
        {
            return builder.AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>(
                ApiAuthenticationHandler.SchemeName,
                ApiAuthenticationHandler.SchemeName,
                configureOptions);
        }
    }
}