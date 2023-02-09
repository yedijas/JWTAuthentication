namespace JWTAuthentication.Options
{
    public class JWTOptions
    {
        public const string JWT = "Jwt";

        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenLife { get; set; } = 5;
    }
}
