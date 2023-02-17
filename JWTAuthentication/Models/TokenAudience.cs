using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class TokenAudience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AudienceID { get; set; }
        public string Hostname { get; set; }
        public string SystemName { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TokenAudience audience &&
                   AudienceID == audience.AudienceID &&
                   Hostname == audience.Hostname &&
                   SystemName == audience.SystemName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AudienceID, Hostname, SystemName);
        }
    }
}
