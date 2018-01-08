using System.Collections.Generic;
using System.Threading.Tasks;

namespace TokenApi.Dal.Common
{
    public interface IUserRepository
    {
        Task<IEnumerable<RegisteredApplicationDto>> GetRegisteredApplicationsAsync(string username);
    }
}