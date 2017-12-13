using PersonnelDepartment.Core.Services.DataProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PersonnelDepartment
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private EmployeeContext _db => EmployeeContext.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!_db.Passwords.Any())
            {
                _db.Passwords.Add(new Models.RootPassword
                {
                    Password = "admin"
                });

                _db.SaveChanges();
            }
        }
    }
}
