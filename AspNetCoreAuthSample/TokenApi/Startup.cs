using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using TokenApi.Common;
using TokenApi.Security;
using TokenApi.Security.ActiveDirectory;
using TokenApi.Security.Common;
using TokenApi.Security.Jwt;

namespace TokenApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<ICredentialValidator, ActiveDirectoryCredentialValidator>();
            services.TryAddScoped<ISecretsProvider, SecretsProvider>();
            services.TryAddScoped<IClaimsProvider, ClaimsProvider>();
            services.TryAddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.TryAddScoped<ISecurityKeyProvider, SecurityKeyProvider>();
            services.TryAddScoped<ITokenService, JwtTokenService>();

            services.TryAddSingleton(provider => new TokenServiceSettings {Issuer = "http://www.techniqly.com/token/v1" });
            
            services.TryAddSingleton(provider =>
            {
                return new MapperConfiguration(ex =>
                {
                    ex.CreateMap<PostTokenRequestModel, CreateTokenOptions>();
                    ex.CreateMap<CreateTokenResult, PostTokenResponseModel>();
                }).CreateMapper();
            });

            services.AddMvc();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Token API",
                    Version = "v1",
                    Contact = new Contact { Name = "Techniqly LLC", Url = "http://www.techniqly.com" }
                });

                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "TokenApi.xml"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiAuthenticationHandler.SchemeName;
                options.DefaultChallengeScheme = ApiAuthenticationHandler.SchemeName;
            }).AddCustomAuth(options => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseAuthentication();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Token API V1"); });
            app.UseMvc();
        }
    }
}