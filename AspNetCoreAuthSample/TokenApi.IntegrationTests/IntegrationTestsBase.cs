using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using TokenApi.Dal.Common;
using TokenApi.Security.Common;

namespace TokenApi.IntegrationTests
{
    public abstract class IntegrationTestsBase
    {
        protected Mock<ICredentialValidator> MockCredentialValidator { get; set; }
        protected Mock<IUserRepository> MockUserRepository { get; set; }
        public HttpClient HttpClient { get; }

        protected IntegrationTestsBase()
        {
            MockCredentialValidator = new Mock<ICredentialValidator>();
            MockUserRepository = new Mock<IUserRepository>();

            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(
                    services =>
                    {
                        services.TryAddTransient(provider => MockCredentialValidator.Object);
                        services.TryAddTransient(provider => MockUserRepository.Object);

                    }));

            HttpClient = server.CreateClient();
        }

        protected StringContent Stringify<T>(T obj) where T : class
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}