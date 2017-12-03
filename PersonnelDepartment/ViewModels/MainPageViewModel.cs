using PersonnelDepartment.Core.Services.DataProvider;
using PersonnelDepartment.Models;
using PersonnelDepartment.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PersonnelDepartment.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        EmployeeContext db;
        IEmployeeService employeeService;
        Employee _itemEmployee;
        string _search;

        ICommand _addCommand;
        ICommand _saveCommand;
        ICommand _removeCommand;

        public MainPageViewModel()
        {
            db = new EmployeeContext();
            employeeService = new EmployeeService(db);
            Employees = new ObservableCollection<Employee>();

            GetEmployee();
        }

        public ObservableCollection<Employee> Employees { get; set; }
        public Employee ItemEmployee { get => _itemEmployee; set => Set(ref _itemEmployee, value); }
        public string Search
        {
            get => _search;
            set
            {
                Set(ref _search, value);
                TextChanged();
            }
        }
        
        public ICommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(() => AddEmployee()));
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(() => SaveEmployee()));
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new DelegateCommand(() => RemoveEmployee()));

        private async void GetEmployee()
        {
            var employeeList = await employeeService.GetAsync();

            foreach (var employee in employeeList)
            {
                Employees.Add(employee);
            }
            ItemEmployee = Employees.First();
        }
        private void AddEmployee()
        {
            Employees.Add(new Employee());
            ItemEmployee = Employees.Last();
        }
        private async void SaveEmployee()
        {
            if (!await employeeService.UpdateAsync(ItemEmployee))
            {
                await employeeService.AddAsync(ItemEmployee);
            }
            
        }
        private void RemoveEmployee()
        {
            employeeService.RemoveAsync(ItemEmployee);
            var employee = Employees.SingleOrDefault(e => e.Id == ItemEmployee.Id);
            Employees.Remove(employee);
            ItemEmployee = Employees.First();
        }
        private void TextChanged()
        {
            if (Search != "")
            {
                using(EmployeeContext _db = new EmployeeContext())
                {
                    System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@search", $"%{Search}%");
                    var employees = _db.Database.SqlQuery<Employee>("SELECT * FROM Employees WHERE FirstSurname LIKE @search OR Name LIKE @search OR Patronymic LIKE @search OR RegistrationNumber LIKE @search", param);
                    Employees.Clear();
                    foreach (var employee in employees)
                    {
                        Employees.Add(employee);
                    }
                }
            }
            else
            {
                Employees.Clear();
                GetEmployee();
            }
        }
    }
}
