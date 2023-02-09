using JWTAuthentication.Models;
using LiteDB;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthentication.Databases.Users
{
    public class UserService : IUserService
    {
        private LiteDatabase _myLiteDB;

        public UserService(ILiteDbContext liteDbContext)
        {
            _myLiteDB = liteDbContext.Database;
        }

        public bool Delete(int userID)
        {
            return _myLiteDB.GetCollection<User>("User")
                .Delete(userID);
        }

        public int DeleteByEmail(string userEmail)
        {
            return _myLiteDB.GetCollection<User>("User")
                .DeleteMany(o => o.UserEmail.IsNullOrEmpty()
                && o.UserEmail.Equals(userEmail));
        }

        public IEnumerable<User> GetAll()
        {
            var result = _myLiteDB.GetCollection<User>
                ("User").FindAll();
            return result;
        }

        public User GetByEmail(string userEmail)
        {
            var result = _myLiteDB.GetCollection<User>
                ("User").Find(o =>
                o.UserEmail.Equals(userEmail))
                .FirstOrDefault();
            return result;
        }

        public User GetByUsername(string userName)
        {
            var result = _myLiteDB.GetCollection<User>
                ("User").Find(o =>
                o.UserName.Equals(userName))
                .FirstOrDefault();
            return result;
        }

        public User GetById(int userID)
        {
            var result = _myLiteDB.GetCollection<User>
                ("User").Find(o => o.UserID == userID)
                .FirstOrDefault();
            return result;
        }

        public User GetOne(User user)
        {
            var result = _myLiteDB.GetCollection<User>
                ("User").Find(o => o.Equals(user))
                .FirstOrDefault();
            return result;
        }

        public int Insert(User singleUser)
        {
            return _myLiteDB.GetCollection<User>
                ("User").Insert(singleUser);
        }

        public bool Update(User singleUser)
        {
            return _myLiteDB.GetCollection<User>
                ("User").Update(singleUser);
        }
    }
}
