using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface ISecretsProvider
    {
        Task<string> GetSecretAsync();
    }
}