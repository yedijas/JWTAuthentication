using JWTAuthentication.Options;
using LiteDB;
using Microsoft.Extensions.Options;

namespace JWTAuthentication.Databases
{
    /// <summary>
    /// Datbase context that can be called manually or using injected dependency.
    /// Implementas IDisposable implicitly because I am too lazy to think on token audience validation method.
    /// </summary>
    public class DatabaseContext : ILiteDbContext, IDisposable
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

        public DatabaseContext(string dblocation)
        {
            if (File.Exists(dblocation))
                Database = new LiteDatabase(dblocation);
            else
                throw new Exception("Database is not created, yet!");
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Database.Dispose();
            }
        }
    }
}
