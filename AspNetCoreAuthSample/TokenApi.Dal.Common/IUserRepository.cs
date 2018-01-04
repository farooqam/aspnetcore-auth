using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenApi.Dal.Common
{
    public interface IUserRepository
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync(string username);
    }
}