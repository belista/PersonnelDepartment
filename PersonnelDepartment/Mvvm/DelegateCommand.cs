using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonnelDepartment.Mvvm
{
    class DelegateCommand : ICommand
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
}
