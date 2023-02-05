using JWTAuthentication.Models;
using LiteDB;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthentication.Databases.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private LiteDatabase _myLiteDB;

        public EmployeeService(ILiteDbContext liteDbContext)
        {
            _myLiteDB = liteDbContext.Database;
        }

        public bool Delete(int employeeID)
        {
            return _myLiteDB.GetCollection<Employee>
                ("Employee").Delete(employeeID);
        }

        public IEnumerable<Employee> GetAll()
        {
            var result = _myLiteDB.GetCollection<Employee>
                ("Employee").FindAll();
            return result;
        }

        public Employee GetById(int employeeID)
        {
            var result = _myLiteDB.GetCollection<Employee>
                ("Employee").Find(o => 
                o.EmployeeID == employeeID)
                .FirstOrDefault();
            return result;
        }

        public Employee GetByLoginId(string loginID)
        {
            var result = _myLiteDB.GetCollection<Employee>
                ("Employee").Find(o => 
                !o.LoginID.IsNullOrEmpty() 
                && o.LoginID.Equals(loginID))
                .FirstOrDefault();
            return result;
        }

        public Employee GetOne(Employee employee)
        {
            var result = _myLiteDB.GetCollection<Employee>
                ("Employee").Find(o => 
                o.Equals(employee))
                .FirstOrDefault();
            return result;
        }

        public int Insert(Employee singleEmployee)
        {
            return _myLiteDB.GetCollection<Employee>
                ("Employee").Insert(singleEmployee);
        }

        public bool Update(Employee singleEmployee)
        {
            return _myLiteDB.GetCollection<Employee>
                ("Employee").Update(singleEmployee);
        }
    }
}
