using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonnelDepartment.Mvvm
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected void Set<T>(ref T prop, T value, [CallerMemberName]string name = null)
        {
            if (Equals(prop, value))
            {
                return;
            }

            prop = value;
            OnPropertyChanged(name);
        }

        protected void Set<T>(ref T prop, T value, Action callback, [CallerMemberName]string name = null)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (Equals(prop, value))
            {
                return;
            }

            prop = value;
            OnPropertyChanged(name);
            callback.Invoke();
        }
    }
}
