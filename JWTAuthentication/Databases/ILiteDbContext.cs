using LiteDB;

namespace JWTAuthentication.Databases
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}
