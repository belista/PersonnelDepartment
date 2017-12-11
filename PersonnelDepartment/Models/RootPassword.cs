using System.ComponentModel;
using System.Diagnostics;

namespace PersonnelDepartment.Models
{
    [DebuggerDisplay("Id - {Id}, Password - {Password}")]
    public class RootPassword : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
