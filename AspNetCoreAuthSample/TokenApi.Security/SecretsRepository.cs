using System.Threading.Tasks;
using TokenApi.Security.Common;

namespace TokenApi.Security
{
    public class SecretsRepository : ISecretsRepository
    {
        public async Task<string> GetSecretForSymmtericKeyAsync()
        {
            return "This is the secret key";
        }
    }
}