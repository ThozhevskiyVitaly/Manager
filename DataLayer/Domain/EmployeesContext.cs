using DataLayer.Models;
using System.Data.Entity;

namespace DataLayer.Domain
{
    public class EmployeesContext:DbContext
    {
        public EmployeesContext(){}
        public EmployeesContext(string connectionString):base(connectionString)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
