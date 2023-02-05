using JWTAuthentication.Options;
using LiteDB;
using Microsoft.Extensions.Options;

namespace JWTAuthentication.Databases
{
    public class DatabaseContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public DatabaseContext(IOptions<LiteDBOptions> options)
        {
            if (!Directory.Exists(options.Value.DatabaseDirectory))
            {
                Directory.CreateDirectory(options.Value.DatabaseDirectory);
            }
            Database = new LiteDatabase(options.Value.DatabaseLocation);
        }
    }
}
