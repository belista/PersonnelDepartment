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

        public void Print()
        {
            var printDialog = new PrintDialog();
            printDialog.PrintVisual(ContentPresenter,"Printing...");
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Print();
        }
    }
}
