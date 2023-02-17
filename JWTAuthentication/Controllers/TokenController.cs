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
        private readonly IOptions<JWTOptions> _options;
        private readonly ITokenHelper _tokenHelper;
        private readonly ITokenService _tokenService;
        private readonly IAudienceService _audienceService;
        private readonly IUserService _userService;

        public TokenController(ILogger<UserController> logger, IOptions<JWTOptions> options,
            ITokenService tokenService, ITokenHelper tokenHelper, IAudienceService audienceService,
            IUserService userService)
        {
            _logger = logger;
            _tokenHelper = tokenHelper;
            _tokenService = tokenService;
            _options = options;
            _userService = userService;
            _audienceService = audienceService;
        }

        [HttpGet("{tokenString}")]
        public async Task<IActionResult> ValidateToken(string tokenString)
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

        [HttpPost("{tokenString}")]
        public async Task<IActionResult> RefreshToken(string tokenString)
        {
            var token = _tokenHelper.GetTokenFromString(tokenString);

            if (_tokenHelper.ValidateToken(token))
            {
                var singleUser = _userService.GetByUsername(token.Claims.FirstOrDefault(o => o.Type == "UserName").Value);

                StringValues requestorHostName = "";
                Request.Headers.TryGetValue("Host", out requestorHostName);
                var tokenInfo = new TokenInfo(_options.Value.Issuer, _options.Value.Subject, requestorHostName, _options.Value.TokenLife, _options.Value.SecretKey);
                if (singleUser != null)
                {
                    string resultedToken = _tokenHelper.GenerateJSONWebToken(tokenInfo, singleUser);
                    _tokenService.Insert(new Token
                    {
                        ActualToken = resultedToken,
                        CreatedDate = DateTime.Now,
                        IssuedFor = singleUser.UserName,
                        RequestorURL = requestorHostName,
                        IsValid = true
                    });
                    return Ok(resultedToken); // return token here
                }
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }
    }
}
