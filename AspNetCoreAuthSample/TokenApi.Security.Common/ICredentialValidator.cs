using System;
using System.Threading.Tasks;

namespace TokenApi.Security.Common
{
    public interface ICredentialValidator
    {
        Task<bool> ValidateAsync(
            Guid userId, 
            string password,
            Guid executedByUserId);
    }
}