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
    public class MainPageViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Fields
        private EmployeeContext _db;
        private IEmployeeService _employeeService;
        private ExcelService _excelService;

        private DelegateCommand _addCommand;
        private DelegateCommand<Employee> _saveCommand;
        private DelegateCommand _removeCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _exportCommand;
        private DelegateCommand _importCommand;
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
        /// Коллекция работников.
        /// </summary>
        public ObservableCollection<Employee> Employees { get; set; }

        /// <summary>
        /// Выбранный работник.
        /// </summary>
        public Employee SelectedEmployee { get; set; }

        /// <summary>
        /// Строка поиска.
        /// </summary>
        public string QueryString { get; set; }
        #endregion


        #region Commands
        /// <summary>
        /// Команда добавления работника.
        /// </summary>
        public DelegateCommand AddCommand => _addCommand ??
            (_addCommand = new DelegateCommand(AddEmployee));

        /// <summary>
        /// Команда сохранения в БД.
        /// </summary>
        public DelegateCommand<Employee> SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand<Employee>(SaveEmployee));

        /// <summary>
        /// Команда удаления работника.
        /// </summary>
        public DelegateCommand RemoveCommand => _removeCommand ??
            (_removeCommand = new DelegateCommand(() => RemoveEmployee()));

        /// <summary>
        /// Команда отмены.
        /// </summary>
        public DelegateCommand CancelCommand => _cancelCommand ??
            (_cancelCommand = new DelegateCommand(() => Cancel()));

        /// <summary>
        /// Команда экспорта.
        /// </summary>
        public DelegateCommand ExportCommand => _exportCommand ??
            (_exportCommand = new DelegateCommand(() => GetData()));

        /// <summary>
        /// Команда импорта.
        /// </summary>
        public DelegateCommand ImportCommand => _importCommand ??
            (_importCommand = new DelegateCommand(() => ImportExcel()));
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


        private async void RemoveEmployee()
        {
            if (await _employeeService.RemoveAsync(SelectedEmployee))
            {
                Employees.Remove(SelectedEmployee);
            }
        }


        private void OnQueryStringChanged()
        {
            if (!string.IsNullOrWhiteSpace(QueryString))
            {
                var str = "%" + QueryString + "%";

                var param = new System.Data.SqlClient.SqlParameter("@queryString", $"%{QueryString}%");

                var employees = _db.Database.SqlQuery<Employee>(
                    $"SELECT * FROM Employees WHERE FirstSurname LIKE @queryString OR Name LIKE @queryString OR Patronymic LIKE @queryString OR RegistrationNumber LIKE @queryString", param);

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
            Employees.Clear();
            GetEmployee();
            SelectedEmployee = null;
        }

        private void GetData()
        {
            var dataTable = new DataTable();
            var empl = _db.Employees.ToList();
            if (empl.Any())
            {
                dataTable = _excelService.ToDataTable(empl);
                ExportExcel(dataTable);
            }
        }

        private void ExportExcel(DataTable dt)
        {
            _excelService.SaveExcel(dt);
        }

        private void ImportExcel()
        {
            var dt = _excelService.OpenExcel();

            if (dt != null)
            {
                Employees.Clear();
                _db.Database.ExecuteSqlCommand("TRUNCATE TABLE Employees");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                var employee = new Employee
                {
                    FirstSurname = dr["FirstSurname"].ToString(),
                    SecondSurname = dr["SecondSurname"].ToString(),
                    ThirdSurname = dr["ThirdSurname"].ToString(),
                    Name = dr["Name"].ToString(),
                    Patronymic = dr["Patronymic"].ToString(),
                    PassportInfo = dr["PassportInfo"].ToString(),
                    Registration = dr["Registration"].ToString(),
                    RegistrationNumber = dr["RegistrationNumber"].ToString(),
                    Phone = Convert.ToInt32(dr["Phone"]),
                    FirstPosition = dr["FirstPosition"].ToString(),
                    SecondPosition = dr["SecondPosition"].ToString(),
                    ThirdPosition = dr["ThirdPosition"].ToString(),
                    FirstOrder = dr["FirstOrder"].ToString(),
                    Dismissed = Convert.ToBoolean(dr["Dismissed"]),
                    SecondOrder = dr["SecondOrder"].ToString(),
                    Additionally = dr["Additionally"].ToString(),
                    EmploymentDate = Convert.ToDateTime(dr["EmploymentDate"]),
                    DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"])
                };

                if (DateTime.TryParse(dr["DateOfDismissal"].ToString(), out DateTime dod))
                {
                    employee.DateOfDismissal = dod;
                }

                Employees.Add(employee);

                _db.Employees.Add(employee);
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}