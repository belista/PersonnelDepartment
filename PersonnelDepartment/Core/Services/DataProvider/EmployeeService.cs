using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonnelDepartment.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public class EmployeeService : IEmployeeService
    {
        private EmployeeContext _db;
        public EmployeeService(EmployeeContext db)
        {
            _db = db;
            db.Employees.Load();
        }

        public Task<bool> AddAsync(Employee employee)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (employee != null)
                    {
                        _db.Employees.Add(employee);
                        _db.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    return false;
                }
            });
        }

        public Task<List<Employee>> GetAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    var employees = _db.Employees.ToList();
                    if (employees == null)
                    {
                        return null;
                    }

                    return employees;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    return null;
                }
            });
        }

        public Task<Employee> GetById(int id)
        {
            return Task.Run(() =>
            {
                try
                {
                    var employee = _db.Employees.SingleOrDefault(u => u.Id == id);

                    if (employee == null)
                    {
                        return null;
                    }

                    return employee;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<bool> RemoveAsync(Employee employee)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (employee != null)
                    {
                        _db.Employees.Remove(employee);
                        _db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    return false;
                }
            });
        }

        public Task<bool> UpdateAsync(Employee employee)
        {
            return Task.Run(() =>
            {
                try
                {
                    var updatedEmployee = _db.Employees.SingleOrDefault(e => e.Id == employee.Id);
                    if (updatedEmployee != null)
                    {
                        var employeeType = updatedEmployee.GetType();
                        foreach (var item in employeeType.GetProperties().Skip(1))
                        {
                            item.SetValue(updatedEmployee, employeeType.GetProperty(item.Name).GetValue(employee));
                        }
                        _db.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    return false;
                }
            });
        }
    }
}
