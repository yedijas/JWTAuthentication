using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Helper
{
    public class TokenHelper
    {
        public static string GenerateJSONWebToken(TokenInfo tokenInfo, User singleUser)
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
                tokenInfo.Issuer,
                null,
                claims,
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}
