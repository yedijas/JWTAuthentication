using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class CertificateInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CertLocation { get; set; } = string.Empty;
        public string AppID { get; set; }
        public string Key { get; set; }
        public string Thumbprint { get; set; } 

        public override bool Equals(object? obj)
        {
            return obj is CertificateInfo info &&
                   Thumbprint == info.Thumbprint &&
                   AppID == info.AppID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Thumbprint, AppID);
        }
    }
}
