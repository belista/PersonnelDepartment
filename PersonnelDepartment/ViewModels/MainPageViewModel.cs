using PersonnelDepartment.Core.Services.DataProvider;
using PersonnelDepartment.Models;
using PersonnelDepartment.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System;
using System.Data;
using System.Collections.Generic;
using PersonnelDepartment.Core.Services.Excel;

namespace PersonnelDepartment.ViewModels
{
    /// <summary>
    /// Вью модель для MainPage.xaml
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Fields
        private EmployeeContext _db;
        private IEmployeeService _employeeService;
        private ExcelService _excelService;

        private DelegateCommand _addCommand;
        private DelegateCommand<Employee> _saveCommand;
        private DelegateCommand<Employee> _removeCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _exportCommand;
        #endregion


        public MainPageViewModel()
        {
            _db = new EmployeeContext();
            _employeeService = new EmployeeService(_db);
            _excelService = new ExcelService();
            Employees = new ObservableCollection<Employee>();

            GetEmployee();
        }


        #region INotifyPropertyChanged implementation
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067
        #endregion


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Employee> Employees { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Employee SelectedEmployee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueryString { get; set; }
        #endregion


        #region Commands
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand AddCommand => _addCommand ??
            (_addCommand = new DelegateCommand(AddEmployee));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand<Employee> SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand<Employee>(SaveEmployee));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand<Employee> RemoveCommand => _removeCommand ??
            (_removeCommand = new DelegateCommand<Employee>(RemoveEmployee));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(() => Cancel()));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ExportCommand => _exportCommand ?? (_exportCommand = new DelegateCommand(() => GetData()));
        #endregion


        private async void GetEmployee()
        {
            var employeeList = await _employeeService.GetAsync();

            foreach (var employee in employeeList)
            {
                Employees.Add(employee);
            }
        }

        private void AddEmployee()
        {
            var empl = new Employee();
            Employees.Add(empl);
            SelectedEmployee = empl;
        }


        private async void SaveEmployee(Employee empl) =>
            await _employeeService.SaveOrUpdateAsync(empl);


        private async void RemoveEmployee(Employee employee)
        {
            if (await _employeeService.RemoveAsync(employee))
            {
                Employees.Remove(employee);
            }
        }


        private void OnQueryStringChanged()
        {
            if (!string.IsNullOrWhiteSpace(QueryString))
            {
                var str = "%" + QueryString + "%";

                var param = new System.Data.SqlClient.SqlParameter("@queryString", $"%{QueryString}%");

                var employees = _db.Database.SqlQuery<Employee>(
                    $"SELECT * FROM Employees WHERE FirstSurname LIKE @queryString OR Name LIKE @queryString OR Patronymic LIKE @queryString OR RegistrationNumber LIKE @queryString",param);

                Employees.Clear();

                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }
            }
            else
            {
                Employees.Clear();
                GetEmployee();
            }
        }

        private void Cancel()
        {
            SelectedEmployee = null;
            Employees.Clear();
            GetEmployee();
        }

        private void GetData()
        {
            DataTable dt = new DataTable();
            List<Employee> employeeList = (from employee in _db.Employees select employee).ToList();
            if (employeeList.Any())
            {
                dt = _excelService.ToDataTable(employeeList);
                ExportExcel(dt);
            }
        }

        private void ExportExcel(DataTable dt)
        {
            _excelService.SaveExcel(dt);
        }
    }
}