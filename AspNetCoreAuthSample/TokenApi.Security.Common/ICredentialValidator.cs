using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface ICredentialValidator
    {
        Task<bool> ValidateAsync(string username, string password);
    }
}