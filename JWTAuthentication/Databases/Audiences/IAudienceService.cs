using JWTAuthentication.Models;

namespace JWTAuthentication.Databases.Audiences
{
    public interface IAudienceService
    {
        public int Insert(TokenAudience tokenAudience);
        public bool Update(TokenAudience tokenAudience);
        public bool Delete(int audienceID);
        public int DeleteByHostname(string hostName);
        public int DeleteBySystemName(string systemName);
        public IEnumerable<TokenAudience> GetAll();
        public TokenAudience GetById(int audienceID);
        public IEnumerable<TokenAudience> GetByHostname(string hostName);
        public IEnumerable<TokenAudience> GetBySystemName(string systemName);
        public TokenAudience GetOne(TokenAudience audience);
        public bool CheckHostExists(string hostName);
    }
}
