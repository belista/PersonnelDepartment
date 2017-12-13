using PersonnelDepartment.Models;
using System.Data.Entity;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public class EmployeeContext : DbContext
    {
        private static EmployeeContext _instance;

        public EmployeeContext() 
            :base("DefaultConnection")
        { }

        public static EmployeeContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmployeeContext();
                }
                
                return _instance;
            }
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<RootPassword> Passwords { get; set; }
    }
}
