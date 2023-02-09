using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthentication.Helper
{
    public interface ITokenHelper
    {
        public string GenerateJSONWebToken(TokenInfo tokenInfo, User singleUser);
    }
}
