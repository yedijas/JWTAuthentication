using JWTAuthentication.Models;

namespace JWTAuthentication.Databases.Tokens
{
    public interface ITokenService
    {
        public int Insert(Token entity);
        public bool Update(Token entity);
        public bool DeleteByID(int tokenID);
        public int DeleteByToken(string tokenString);
        public int DeleteByUsername(string issuedFor);
        public IEnumerable<Token> GetAll();
        public Token GetByID(int tokenID);
        public Token GetByToken(string tokenString);
        public Token GetOne(Token entity);
    }
}
