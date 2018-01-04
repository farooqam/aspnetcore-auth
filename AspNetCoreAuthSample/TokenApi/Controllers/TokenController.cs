using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenApi.Common;
using TokenApi.Dal.Common;
using TokenApi.Security.Common;

namespace TokenApi.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostTokenRequestModel request)
        {
            var createTokenOptions = _mapper.Map<CreateTokenOptions>(request);
            var token = await _tokenService.CreateTokenAsync(createTokenOptions);
            return new OkObjectResult(token);
        }
    }
}
