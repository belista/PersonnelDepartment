using PersonnelDepartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAsync();
        Task<bool> RemoveAsync(Employee user);
        Task<bool> AddAsync(Employee user);
        Task<Employee> GetById(int id);
        Task<bool> UpdateAsync(Employee employee);
    }
}
