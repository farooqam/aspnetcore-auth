using System.Threading.Tasks;
using TokenApi.Security.Common;

namespace TokenApi.Security.ActiveDirectory
{
    public class ActiveDirectoryCredentialValidator : ICredentialValidator
    {
        public Task<bool> ValidateAsync(string username, string password)
        {
            return Task.FromResult(true);
        }
    }
}