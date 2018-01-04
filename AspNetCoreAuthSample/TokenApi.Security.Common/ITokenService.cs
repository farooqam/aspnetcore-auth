using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(CreateTokenOptions options);
    }
}