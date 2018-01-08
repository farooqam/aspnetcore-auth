using System.Threading.Tasks;

namespace TokenApi.Dal.Common
{
    public interface IUserRepository
    {
        Task<RegisteredApplicationDto> GetRegisteredApplicationAsync(string username, string applicationName);
    }
}