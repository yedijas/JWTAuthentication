using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ActualToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsValid { get; set; }
        public string IssuedFor { get; set; }
        public string RequestorURL { get; set; }
        public string UsedByURL { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Token token &&
                ActualToken == token.ActualToken &&
                CreatedDate == token.CreatedDate &&
                RequestorURL== token.RequestorURL &&
                IssuedFor == token.IssuedFor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IssuedFor);
        }
    }
}
