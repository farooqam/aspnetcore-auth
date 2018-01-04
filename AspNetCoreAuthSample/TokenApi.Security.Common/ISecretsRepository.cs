using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface ISecretsRepository
    {
        Task<string> GetSecretForSymmtericKeyAsync();
    }
}