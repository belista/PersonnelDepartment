using PersonnelDepartment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    /// <summary>
    /// Сервис для работы с БД.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private EmployeeContext _db;
        public EmployeeService(EmployeeContext db)
        {
            _db = db;
            db.Employees.Load();
        }

        /// <summary>
        /// Возвращает всех работников.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает работника по id.
        /// </summary>
        /// <param name="id">Идентификатор работника.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаляет работника.
        /// </summary>
        /// <param name="employee">Работник.</param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

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

        /// <summary>
        /// Сохраняет либо редактирует работника.
        /// </summary>
        /// <param name="employee">Работник.</param>
        /// <returns></returns>
        public Task<bool> SaveOrUpdateAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            return Task.Run(() =>
            {
                try
                {
                    var updatedEmployee = _db.Employees.SingleOrDefault(e => e.Id == employee.Id);
                    if (updatedEmployee == null)
                    {
                        _db.Employees.Add(employee);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var employeeType = updatedEmployee.GetType();
                        foreach (var item in employeeType.GetProperties().Skip(1).Where(i => i.Name != "FullName" && i.Name != "WorkDays" && i.Name != "Dismissed"))
                        {
                            item.SetValue(updatedEmployee, employeeType.GetProperty(item.Name).GetValue(employee));
                        }
                        _db.SaveChanges();
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
