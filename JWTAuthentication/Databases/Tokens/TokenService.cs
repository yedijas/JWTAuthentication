using JWTAuthentication.Models;
using LiteDB;

namespace JWTAuthentication.Databases.Tokens
{
    public class TokenService : ITokenService
    {
        private LiteDatabase _myLiteDB;

        public TokenService(ILiteDbContext liteDbContext)
        {
            _myLiteDB = liteDbContext.Database;
        }

        public bool DeleteByID(int tokenID)
        {
            return _myLiteDB.GetCollection<Token>("Token")
                .Delete(tokenID);
        }

        public int DeleteByToken(string tokenString)
        {
            return _myLiteDB.GetCollection<Token>("Token")
                .DeleteMany(o => 
                o.ActualToken.Equals(tokenString));
        }

        public int DeleteByUsername(string issuedFor)
        {
            return _myLiteDB.GetCollection<Token>("Token")
                .DeleteMany(o =>
                o.IssuedFor.Equals(issuedFor));
        }

        public IEnumerable<Token> GetAll()
        {
            var result = _myLiteDB.GetCollection<Token>
                            ("Token").FindAll();
            return result;
        }

        public Token GetByID(int tokenID)
        {
            var result = _myLiteDB.GetCollection<Token>("Token")
                .Find(o =>
                o.Id == tokenID).FirstOrDefault();
            return result;
        }

        public Token GetByToken(string tokenString)
        {
            var result = _myLiteDB.GetCollection<Token>("Token")
                .Find(o =>
                o.ActualToken.Equals(tokenString)).FirstOrDefault();
            return result;
        }

        public Token GetOne(Token entity)
        {
            var result = _myLiteDB.GetCollection<Token>("Token")
                .Find(o =>
                o.Equals(entity)).FirstOrDefault();
            return result;
        }

        public int Insert(Token entity)
        {
            return _myLiteDB.GetCollection<Token>
                ("Token").Insert(entity);
        }

        public bool Update(Token entity)
        {
            return _myLiteDB.GetCollection<Token>
                ("Token").Update(entity);
        }
    }
}
