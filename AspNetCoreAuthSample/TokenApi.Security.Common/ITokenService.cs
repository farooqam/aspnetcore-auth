using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface ITokenService
    {
        Task<CreateTokenResult> CreateTokenAsync(CreateTokenOptions options);
    }
}