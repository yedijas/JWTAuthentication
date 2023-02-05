namespace JWTAuthentication.Options
{
    public class LiteDBOptions
    {
        public const string LiteDB = "LiteDB";

        public string DatabaseLocation { get; set; } = string.Empty;
        public string DatabaseDirectory { get; set; } = string.Empty;
    }
}
