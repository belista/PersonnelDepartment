using System;
using System.Diagnostics;
using System.Windows.Input;

namespace PersonnelDepartment.Mvvm
{
    /// <summary>
    /// Класс для работы с командами.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Fields
        private Action _action;
        #endregion


        public DelegateCommand(Action action) =>
            _action = action ?? throw new ArgumentNullException(nameof(action));


        #region ICommand implementation
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                _action();
            }
            catch (Exception)
            {
                Debugger.Break();
            }
        }
        #endregion
    }

    public class DelegateCommand<T> : ICommand
    {
        #region Fields
        private Action<T> _action;
        #endregion


        public DelegateCommand(Action<T> action) =>
            _action = action ?? throw new ArgumentNullException(nameof(action));


        #region ICommand implementation
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                _action((T)parameter);
            }
            catch (Exception)
            {
                Debugger.Break();
            }
        }
        #endregion
    }
}
