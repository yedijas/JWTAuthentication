using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace JWTAuthentication.Helper
{
    public interface ITokenHelper
    {
        public string GenerateJSONWebToken(TokenInfo tokenInfo, User singleUser);
        public bool ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken,
           TokenValidationParameters validationParameters);
        public bool ValidateToken(string tokenString);
        public bool ValidateToken(JwtSecurityToken token);
        public bool ValidateUserNameInClaim(JwtSecurityToken tokenToValidate);
        public JwtSecurityToken GetTokenFromString(string tokenString);
    }
}
