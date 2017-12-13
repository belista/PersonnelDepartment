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
        private EmployeeContext _db => EmployeeContext.Instance;
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
        private DelegateCommand _openPrintPageCommand;
        private DelegateCommand _openChangePasswordPopupCommand;
        private DelegateCommand _popupCancelCommand;
        private DelegateCommand _changePasswordCommand;
        private DelegateCommand _openExitPopupCommand;
        private DelegateCommand _logGuestCommand;
        private DelegateCommand _openRemovePopupCommand;
        #endregion

        #region Ctors
        public MainPageViewModel()
        {
            _employeeService = new EmployeeService();
            _excelService = new ExcelService();
            _rootPasswordService = new RootPasswordService();
            Employees = new ObservableCollection<Employee>();

            GetEmployee();
        } 
        #endregion

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
        /// Popup Логина.
        /// </summary>
        public bool LoginPopup { get; set; } = false;

        /// <summary>
        /// Popup смена пароля.
        /// </summary>
        public bool ChangePasswordPopup { get; set; } = false;

        /// <summary>
        /// Режим админа.
        /// </summary>
        public Visibility AdminVisibility { get; set; } = Visibility.Hidden;

        /// <summary>
        /// Режим гостя.
        /// </summary>
        public Visibility GuestVisibility { get; set; } = Visibility.Visible;

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Старый пароль.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Новый пароль.
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Блокировка Grid.
        /// </summary>
        public bool PopupEnabled { get; set; } = true;

        /// <summary>
        /// Popup Выхода.
        /// </summary>
        public bool ExitPopup { get; set; } = false;

        /// <summary>
        /// Popup удаления.
        /// </summary>
        public bool RemovePopup { get; set; } = false;
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
        /// Команда логина.
        /// </summary>
        public DelegateCommand LoginCommand => _loginCommand ??
            (_loginCommand = new DelegateCommand(() => Login()));

        /// <summary>
        /// Команда отмены в Popup.
        /// </summary>
        public DelegateCommand PopupCancelCommand => _popupCancelCommand ??
            (_popupCancelCommand = new DelegateCommand(() => PopupCancel()));

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OpenPrintPageCommand => _openPrintPageCommand ??
            (_openPrintPageCommand = new DelegateCommand(() => OpenPrintPage()));

        /// <summary>
        /// Команда открытия окна смены пароля.
        /// </summary>
        public DelegateCommand OpenChangePasswordPopupCommand => _openChangePasswordPopupCommand ??
            (_openChangePasswordPopupCommand = new DelegateCommand(() => OpenChangePasswordPopup()));

        /// <summary>
        /// Команда смены пароля.
        /// </summary>
        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ??
            (_changePasswordCommand = new DelegateCommand(() => ChangePassword()));

        /// <summary>
        /// Команда открытия окна выхода.
        /// </summary>
        public DelegateCommand OpenExitPopupCommand => _openExitPopupCommand ??
            (_openExitPopupCommand = new DelegateCommand(() => OpenExitPopup()));

        /// <summary>
        /// Команда выхода.
        /// </summary>
        public DelegateCommand LogGuestCommand => _logGuestCommand ??
            (_logGuestCommand = new DelegateCommand(() => LogGuest()));

        /// <summary>
        /// Команда открытия окна удаления.
        /// </summary>
        public DelegateCommand OpenRemovePopupCommand => _openRemovePopupCommand ??
            (_openRemovePopupCommand = new DelegateCommand(() => OpenRemovePopup()));
        #endregion

        #region Non-Public Methods
        /// <summary>
        /// Получение работников.
        /// </summary>
        private async void GetEmployee()
        {
            var employeeList = await _employeeService.GetAsync();

            foreach (var employee in employeeList)
            {
                Employees.Add(employee);
            }

            Employees = new ObservableCollection<Employee>(Employees.OrderBy(i => i.FirstSurname));
        }

        private void AddEmployee()
        {
            var empl = new Employee();
            Employees.Add(empl);
            SelectedEmployee = empl;
        }


        private async void SaveEmployee(Employee empl)
        {
            if (!await _employeeService.SaveOrUpdateAsync(empl))
            {
                SelectedEmployee = null;
            }
        }

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

        private async void ImportExcel()
        {
            var dt = _excelService.OpenExcel();

            if (dt.Rows != null)
            {
                Employees.Clear();
                _db.Employees.RemoveRange(_db.Employees);
                _db.SaveChanges();
                await _employeeService.Reset();
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
            ExitPopup = false;
            RemovePopup = false;
            PopupEnabled = true;
        }

        private async void Login()
        {
            if (!await _rootPasswordService.Login(Password))
            {
                Password = "Не правильный пароль";
                return;
            }
            LoginPopup = false;
            AdminVisibility = Visibility.Visible;
            GuestVisibility = Visibility.Hidden;
            PopupEnabled = true;
            Password = null;
        }

        private void OpenPrintPage()
        {

        }

        private void OpenChangePasswordPopup()
        {
            ChangePasswordPopup = true;
            PopupEnabled = false;
        }

        private async void ChangePassword()
        {
            if (!await _rootPasswordService.Сhange(OldPassword, NewPassword))
            {
                OldPassword = "Не правильный пароль";
                return;
            }
            ChangePasswordPopup = false;
            PopupEnabled = true;
            OldPassword = null;
            NewPassword = null;
        }

        private void OpenExitPopup()
        {
            ExitPopup = true;
            PopupEnabled = false;
        }

        private void LogGuest()
        {
            ExitPopup = false;
            PopupEnabled = true;
            AdminVisibility = Visibility.Hidden;
            GuestVisibility = Visibility.Visible;
        }

        private void OpenRemovePopup()
        {
            RemovePopup = true;
            PopupEnabled = false;
        }
        #endregion

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}