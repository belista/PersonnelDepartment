using PersonnelDepartment.Core.Services.DataProvider;
using PersonnelDepartment.Models;
using PersonnelDepartment.ViewModels;
using System.Windows.Controls;

namespace PersonnelDepartment.Views
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
        }
    }
}
