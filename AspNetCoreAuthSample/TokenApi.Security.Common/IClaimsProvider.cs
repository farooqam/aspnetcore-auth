using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface IClaimsProvider
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync(string username);
    }
}