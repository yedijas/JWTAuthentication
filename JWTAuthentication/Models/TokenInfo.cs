using System.Text;
using System.Security.Cryptography;


namespace JWTAuthentication.Models
{
    public class TokenInfo
    {
        public string Subject { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string HashedSecretKey { get; set; } = string.Empty;
        public string UnHashedSecretKey { get; set; } = "";
        public string Issuer { get; set; }

        public TokenInfo(string _issuer, string _subject, string _plainKey = "")
        {
            Subject = _subject;
            Issuer = _issuer;
            UnHashedSecretKey = _plainKey;
            HashTheKey();
        }

        public TokenInfo()
        {
            throw new NotImplementedException();
        }

        public void SetKeyHashed(string plainKey)
        {
            var crypt = SHA256.Create();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(plainKey));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            HashedSecretKey = hash.ToString();
        }

        public void HashTheKey()
        {
            SetKeyHashed(UnHashedSecretKey);
        }
    }
}
