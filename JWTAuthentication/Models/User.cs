using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set;} = string.Empty;
        public DateTime CreatedAt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   UserID == user.UserID &&
                   UserName == user.UserName &&
                   UserPassword == user.UserPassword &&
                   UserEmail == user.UserEmail;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserID, UserName, UserPassword, UserEmail, CreatedAt);
        }
    }
}
