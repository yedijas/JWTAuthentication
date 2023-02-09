using JWTAuthentication.Models;
using LiteDB;

namespace JWTAuthentication.Databases.Audiences
{
    public class AudienceService : IAudienceService
    {
        private LiteDatabase _myLiteDB;

        public AudienceService(ILiteDbContext liteDbContext)
        {
            _myLiteDB = liteDbContext.Database;
        }

        public bool Delete(int audienceID)
        {
            return _myLiteDB.GetCollection
                <TokenAudience>("TokenAudience")
                .Delete(audienceID);
        }

        public int DeleteByHostname(string hostName)
        {
            return _myLiteDB.GetCollection
                <TokenAudience>("TokenAudience")
                .DeleteMany(o => o.Hostname.Equals(hostName));
        }

        public int DeleteBySystemName(string systemName)
        {
            return _myLiteDB.GetCollection
                <TokenAudience>("TokenAudience")
                .DeleteMany(o => o.SystemName.Equals(systemName));
        }

        public IEnumerable<TokenAudience> GetAll()
        {
            var result = _myLiteDB.GetCollection<TokenAudience>
                            ("TokenAudience").FindAll();
            return result;
        }

        public IEnumerable<TokenAudience> GetByHostname(string hostName)
        {
            var result = _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Find(o =>
                o.Hostname.Equals(hostName));
            return result;
        }

        public TokenAudience GetById(int audienceID)
        {
            var result = _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Find(o =>
                o.AudienceID == audienceID)
                .FirstOrDefault();
            return result;
        }

        public IEnumerable<TokenAudience> GetBySystemName(string systemName)
        {
            var result = _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Find(o =>
                o.SystemName.Equals(systemName));
            return result;
        }

        public TokenAudience GetOne(TokenAudience audience)
        {
            var result = _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Find(o =>
                o.Equals(audience))
                .FirstOrDefault();
            return result;
        }

        public int Insert(TokenAudience tokenAudience)
        {
            return _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Insert(tokenAudience);
        }

        public bool Update(TokenAudience tokenAudience)
        {
            return _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Update(tokenAudience);
        }

        public bool CheckHostExists(string hostName)
        {
            return _myLiteDB.GetCollection<TokenAudience>
                ("TokenAudience").Exists(o => o.Hostname.Equals(hostName));
        }
    }
}
