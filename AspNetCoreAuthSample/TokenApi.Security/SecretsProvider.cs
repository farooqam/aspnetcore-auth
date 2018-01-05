using System.Threading.Tasks;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class SecretsProvider : ISecretsProvider
    {
        public Task<string> GetSecretAsync()
        {
            return Task.FromResult("This is the secret key");
        }
    }
}