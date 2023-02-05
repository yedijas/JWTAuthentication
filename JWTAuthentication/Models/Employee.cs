namespace JWTAuthentication.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public int EmployeeName { get; set; }
        public string? LoginID { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Employee employee &&
                   EmployeeID == employee.EmployeeID &&
                   EmployeeName == employee.EmployeeName &&
                   LoginID == employee.LoginID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EmployeeID, EmployeeName, LoginID);
        }
    }
}
