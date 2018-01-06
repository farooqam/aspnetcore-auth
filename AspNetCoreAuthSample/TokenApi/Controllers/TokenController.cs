using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TokenApi.Common;
using TokenApi.Security.Common;

namespace TokenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public TokenController(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a token for the given username, password, and audience.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /token
        ///     {
        ///         "username":"farooq",
        ///         "password":"HelloWorld123!!",
        ///         "audience":"My mobile app"
        ///     }
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>The token and issuer name.</returns>
        /// <response code="200">Returns the token and issuer name.</response>
        /// <response code="400">If the username, password, and audience could not be verified.</response>      
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PostTokenResponseModel))]
        [ProducesResponseType(400, Type = typeof(ApiErrors))]
        public async Task<IActionResult> Post([FromBody] PostTokenRequestModel request)
        {
            var createTokenOptions = _mapper.Map<CreateTokenOptions>(request);
            var token = await _tokenService.CreateTokenAsync(createTokenOptions);

            if (token == null)
            {
                return BadRequest(new ApiErrors { Errors = new[] { ApiError.CreateTokenAuthFailure(request)}});
            }

            return new OkObjectResult(new PostTokenResponseModel {Token = token, Issuer = "http://www.techniqly.com/token/v1"});
        }
    }
}