namespace JWTAuthentication.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? UserEmail { get; set;}
        public DateTime? CreatedAt { get; set; } = default(DateTime?);

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
