using System;
using System.Threading.Tasks;
using TokenApi.Security.Common;

namespace TokenApi.Security.ActiveDirectory
{
    public class ActiveDirectoryCredentialValidator : ICredentialValidator
    {
        public Task<bool> ValidateAsync(Guid userId, string password, Guid executedByUserId)
        {
            return Task.FromResult(true);
        }
    }
}