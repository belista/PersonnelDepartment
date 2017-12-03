using PersonnelDepartment.Core.Services.DataProvider;
using PersonnelDepartment.Models;
using PersonnelDepartment.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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

        private DelegateCommand _addCommand;
        private DelegateCommand<Employee> _saveCommand;
        private DelegateCommand<Employee> _removeCommand;
        #endregion


        public MainPageViewModel()
        {
            _db = new EmployeeContext();
            _employeeService = new EmployeeService(_db);
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
    }
}