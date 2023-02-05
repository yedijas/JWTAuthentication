using JWTAuthentication.Models;

namespace JWTAuthentication.Databases.Employees
{
    public interface IEmployeeService
    {
        int Insert(Employee singleEmployee);
        bool Update(Employee singleEmployee);
        bool Delete(int employeeID);
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        Employee GetByLoginId(string loginID);
        Employee GetOne(Employee employee);
    }
}
