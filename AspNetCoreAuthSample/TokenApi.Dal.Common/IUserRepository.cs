using System;
using System.Threading.Tasks;

namespace TokenApi.Dal.Common
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserAsync(string username, Guid executedByUserId);
        Task<RegisteredApplicationDto> GetRegisteredApplicationAsync(string username, string applicationName);
    }
}