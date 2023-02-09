using JWTAuthentication.Models;

namespace JWTAuthentication.Databases.Users
{
    public interface IUserService
    {
        public int Insert(User singleUser);
        public bool Update(User singleUser);
        public bool Delete(int userID);
        public int DeleteByEmail(string userEmail);
        public IEnumerable<User> GetAll();
        public User GetById(int userID);
        public User GetByEmail(string userEmail);
        public User GetByUsername(string userName);
        public User GetOne(User user);
    }
}
