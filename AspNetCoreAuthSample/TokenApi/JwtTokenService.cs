using System.Threading.Tasks;
using TokenApi.Dal.Common;
using TokenApi.Security.Common;

namespace TokenApi
{
    public class JwtTokenService : ITokenService
    {
        private readonly ICredentialValidator _credentialValidator;
        private readonly ISecretsProvider _secretsProvider;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly ISecurityKeyProvider _securityKeyProvider;

        public JwtTokenService(
            ICredentialValidator credentialValidator,
            ISecretsProvider secretsProvider,
            IUserRepository userRepository,
            IJwtTokenProvider jwtTokenProvider,
            ISecurityKeyProvider securityKeyProvider)
        {
            _credentialValidator = credentialValidator;
            _secretsProvider = secretsProvider;
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _securityKeyProvider = securityKeyProvider;
        }

        public async Task<string> CreateTokenAsync(CreateTokenOptions options)
        {
            var credentialsAreValid = await _credentialValidator.ValidateAsync(options.Username, options.Password);

            if (!credentialsAreValid)
            {
                return null;
            }

            var serverSecret = await _secretsProvider.GetSecretForSymmtericKeyAsync();
            var secretKey = await _securityKeyProvider.CreateSecurityKeyAsync(serverSecret);
            var claims = await _userRepository.GetUserClaimsAsync(options.Username);
            var token = await _jwtTokenProvider.CreateTokenAsync(claims, secretKey, options.Issuer, options.Audience);

            return token;
        }
    }
}