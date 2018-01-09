using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TokenApi.Common;
using TokenApi.Security;
using TokenApi.Security.ActiveDirectory;
using TokenApi.Security.Common;
using TokenApi.Security.Jwt;

namespace TokenApi
{
    public static class Extensions
    {
        public static IServiceCollection AddTokenService(this IServiceCollection services)
        {
            services.TryAddScoped<ICredentialValidator, ActiveDirectoryCredentialValidator>();
            services.TryAddScoped<ISecretsProvider, SecretsProvider>();
            services.TryAddScoped<IClaimsProvider, ClaimsProvider>();
            services.TryAddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.TryAddScoped<ISecurityKeyProvider, SecurityKeyProvider>();
            services.TryAddScoped<ITokenService, JwtTokenService>();

            services.TryAddSingleton(
                provider => new TokenServiceSettings {Issuer = "http://www.techniqly.com/token/v1"});

            services.TryAddSingleton(provider =>
            {
                return new MapperConfiguration(ex => { ex.CreateMap<CreateTokenResult, PostTokenResponseModel>(); })
                    .CreateMapper();
            });

            return services;
        }
    }
}