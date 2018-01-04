using System.Threading.Tasks;
using TokenApi.Security.Common;

namespace TokenApi.Security.ActiveDirectory
{
    public class ActiveDirectoryCredentialValidator : ICredentialValidator
    {
        public async Task<bool> ValidateAsync(string username, string password)
        {
            return true;
        }
    }
}