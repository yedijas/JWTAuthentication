using JWTAuthentication.Databases.Audiences;
using JWTAuthentication.Databases.Users;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Helper
{
    public class TokenHelper : ITokenHelper
    {
        private IAudienceService _audienceService;
        private IOptions<JWTOptions> _options;
        private readonly IUserService _userService;

        public TokenHelper(IOptions<JWTOptions> options, IAudienceService audienceService, IUserService userService)
        {
            _options = options;
            _audienceService = audienceService;
            _userService = userService;
        }

        public string GenerateJSONWebToken(TokenInfo tokenInfo, User singleUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenInfo.HashedSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, tokenInfo.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", singleUser.UserName),
                        new Claim("UserEmail", singleUser.UserEmail)
                    };

            var token = new JwtSecurityToken(
                issuer: tokenInfo.Issuer,
                audience: tokenInfo.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenInfo.TokenLife),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }

        public bool ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            bool retval = false;
            foreach (string singleAudience in audiences)
            {
                retval = _audienceService.CheckHostExists(singleAudience);
                if (retval)
                    break;
            }

            return retval;
        }

        public JwtSecurityToken GetTokenFromString(string tokenString)
        {
            JwtSecurityToken token = null;

            var jwtHandler = new JwtSecurityTokenHandler();
            var signinigkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenInfo.HashThisString(_options.Value.SecretKey)));
            var validationParam = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _options.Value.Issuer,
                IssuerSigningKey = signinigkey,
                AudienceValidator = ValidateAudience
            };

            jwtHandler.ValidateToken(tokenString, validationParam, out SecurityToken validatedToken);
            token = (JwtSecurityToken)validatedToken;

            return token;
        }

        public bool ValidateToken(string tokenString)
        {
            bool result = false;
            
            JwtSecurityToken jwtsectoken = GetTokenFromString(tokenString);
            if (jwtsectoken is not null)
            {
                bool isValidAlgorithm = jwtsectoken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                bool isValidUserClaim = ValidateUserNameInClaim(jwtsectoken);
            }
            else
            {
                return false;
            }
            return result;
        }

        public bool ValidateToken(JwtSecurityToken token)
        {
            bool result = false;
            if (token is not null)
            {
                bool isValidAlgorithm = token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                bool isValidUserClaim = ValidateUserNameInClaim(token);
            }
            else
            {
                return false;
            }
            return result;
        }

        public bool ValidateUserNameInClaim(JwtSecurityToken tokenToValidate)
        {
            bool result = false;
            string username = tokenToValidate.Claims.FirstOrDefault(o => o.Type == "UserName").Value;
            string useremail = tokenToValidate.Claims.FirstOrDefault(o => o.Type == "UserEmail").Value;
            var user = _userService.GetByUsername(username);
            if (user is not null)
                result = true;
            else
                result = false;
            if (user.UserEmail.Equals(useremail))
                result = true;
            else
                result = false;
            return result;
        }
    }
}
