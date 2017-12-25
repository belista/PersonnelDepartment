using PersonnelDepartment.Core.Services.DataProvider;
using PersonnelDepartment.Core.Services.Printing;
using PersonnelDepartment.Models;
using PersonnelDepartment.ViewModels;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PersonnelDepartment.Views
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private EmployeeContext _db => EmployeeContext.Instance;

        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
        }

        /// <summary>
        /// Печать работающих на данный момент.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var emloyeeList = _db.Employees.Where(emp => emp.DateOfDismissal == null);

            var employees = new List<Employee>();

            foreach (var item in emloyeeList)
            {
                employees.Add(item);
            }

            if (!(employees.Count > 0))
            {
                MessageBox.Show("Количество записей равно нулю");
                return;
            }

            PrintService.Print(employees);
        }

        /// <summary>
        /// Печать работника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var employees = new List<Employee>();

            employees.Add((Employee)EmployeeList.SelectedItem);

            if (employees.First() == null)
            {
                MessageBox.Show("Выберите работника");
                return;
            }

            PrintService.PrintEmployee(employees);
        }
    }
}
