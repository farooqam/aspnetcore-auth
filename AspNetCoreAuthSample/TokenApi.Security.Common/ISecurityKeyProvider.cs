using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TokenApi.Security.Common
{
    public interface ISecurityKeyProvider
    {
        Task<SecurityKey> CreateSecurityKeyAsync(string secret);
    }
}