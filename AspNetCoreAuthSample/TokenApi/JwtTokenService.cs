using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TokenApi.Dal.Common;
using TokenApi.Security.Common;

namespace TokenApi
{
    public class JwtTokenService : ITokenService
    {
        private readonly ICredentialValidator _credentialValidator;
        private readonly ISecretsRepository _secretsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ISecurityKeyFactory _securityKeyFactory;

        public JwtTokenService(
            ICredentialValidator credentialValidator,
            ISecretsRepository secretsRepository,
            IUserRepository userRepository,
            IJwtTokenFactory jwtTokenFactory,
            ISecurityKeyFactory securityKeyFactory)
        {
            _credentialValidator = credentialValidator;
            _secretsRepository = secretsRepository;
            _userRepository = userRepository;
            _jwtTokenFactory = jwtTokenFactory;
            _securityKeyFactory = securityKeyFactory;
        }

        public async Task<string> CreateTokenAsync(CreateTokenOptions options)
        {
            var credentialsAreValid = await _credentialValidator.ValidateAsync(options.Username, options.Password);

            if (!credentialsAreValid)
            {
                return null;
            }

            var serverSecret = await _secretsRepository.GetSecretForSymmtericKeyAsync();
            var secretKey = await _securityKeyFactory.CreateSecurityKeyAsync(serverSecret);
            var claims = await _userRepository.GetUserClaimsAsync(options.Username);
            var token = await _jwtTokenFactory.CreateTokenAsync(claims, secretKey, options.Issuer, options.Audience);

            return token;
        }
    }
}