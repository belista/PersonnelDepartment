using PersonnelDepartment.Models;
using System.Data.Entity;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() 
            :base("DefaultConnection")
        { }

        public DbSet<Employee> Employees { get; set; }
    }
}
