using JWTAuthentication.Databases.Audiences;
using JWTAuthentication.Databases;
using JWTAuthentication.Databases.Tokens;
using JWTAuthentication.Helper;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JWTAuthentication.Databases.Users;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenService _tokenService;
        private readonly IOptions<JWTOptions> _options;
        private readonly IAudienceService _audienceService;

        public TokenController(ILogger<UserController> logger, IOptions<JWTOptions> options,
            ITokenService tokenService, ITokenHelper tokenHelper, IAudienceService audienceService)
        {
            _logger = logger;
            _tokenHelper = tokenHelper;
            _tokenService = tokenService;
            _options = options;
            _audienceService = audienceService;
        }

        [HttpGet("{tokenString}")]
        public ActionResult<Token> Validate(string tokenString)
        {
            bool isValid = false;
            var singleToken = _tokenService.GetByToken(tokenString);
            isValid = _tokenHelper.ValidateToken(tokenString);
            if (singleToken != default && isValid)
            {
                StringValues requestorHostName = "";
                Request.Headers.TryGetValue("Host", out requestorHostName);
                singleToken.UsedByURL += (@"|" + requestorHostName);
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
