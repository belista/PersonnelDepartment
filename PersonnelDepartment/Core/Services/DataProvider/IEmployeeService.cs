using PersonnelDepartment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    /// <summary>
    /// Интерфейс для EmployeeService.
    /// </summary>
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAsync();


        Task<bool> RemoveAsync(Employee user); 


        Task<Employee> GetById(int id);
         

        Task<bool> SaveOrUpdateAsync(Employee employee); 
    }
}
