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
using System.Windows;
using PersonnelDepartment.Views;
using System.Windows.Controls;

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
        private IExcelService _excelService;
        private IRootPasswordService _rootPasswordService;

        private DelegateCommand _addCommand;
        private DelegateCommand<Employee> _saveCommand;
        private DelegateCommand _removeCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _exportCommand;
        private DelegateCommand _importCommand;
        private DelegateCommand _openLoginPopupCommand;
        private DelegateCommand _loginCommand;
        private DelegateCommand _PopupCancelCommand;
        private DelegateCommand _openPrintPageCommand;
        private DelegateCommand _openChangePasswordPopupCommand;
        private DelegateCommand _popupCancelCommand;
        private DelegateCommand _changePasswordCommand;
        #endregion


        public MainPageViewModel()
        {
            _db = new EmployeeContext();
            _employeeService = new EmployeeService(_db);
            _excelService = new ExcelService();
            _rootPasswordService = new RootPasswordService(_db);
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

        /// <summary>
        /// Popup isOpen.
        /// </summary>
        public bool LoginPopup { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool ChangePasswordPopup { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public Visibility AdminVisibility { get; set; } = Visibility.Hidden;

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PopupEnabled { get; set; } = true;
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

        /// <summary>
        /// Команда открытия окна логина.
        /// </summary>
        public DelegateCommand OpenLoginPopupCommand => _openLoginPopupCommand ??
            (_openLoginPopupCommand = new DelegateCommand(() => OpenLoginPopup()));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand LoginCommand => _loginCommand ??
            (_loginCommand = new DelegateCommand(() => Login()));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand PopupCancelCommand => _popupCancelCommand ??
            (_popupCancelCommand = new DelegateCommand(() => PopupCancel()));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OpenPrintPageCommand => _openPrintPageCommand ??
            (_openPrintPageCommand = new DelegateCommand(() => OpenPrintPage()));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OpenChangePasswordPopupCommand => _openChangePasswordPopupCommand ??
            (_openChangePasswordPopupCommand = new DelegateCommand(() => OpenChangePasswordPopup()));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ??
            (_changePasswordCommand = new DelegateCommand(() => ChangePassword()));
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

        private void OpenLoginPopup()
        {
            LoginPopup = true;
            PopupEnabled = false;
        }
        private void PopupCancel()
        {
            LoginPopup = false;
            ChangePasswordPopup = false;
            PopupEnabled = true;
        }
        private async void Login()
        {
            if (await _rootPasswordService.Login(Password))
            {
                LoginPopup = false;
                AdminVisibility = Visibility.Visible;
                PopupEnabled = true;
            }
        }
        private void OpenPrintPage()
        {

        }
        private void OpenChangePasswordPopup()
        {
            ChangePasswordPopup = true;
            PopupEnabled = false;
        }
        private void ChangePassword()
        {
            _rootPasswordService.Сhange(OldPassword, NewPassword);
            ChangePasswordPopup = false;
            PopupEnabled = true;
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}