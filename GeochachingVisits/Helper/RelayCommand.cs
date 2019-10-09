using System;
using System.Windows.Input;

namespace GeochachingVisits.Helper
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action<object> m_execute;
        private readonly Predicate<object> m_canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
            ;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.m_execute = execute;
            this.m_canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return this.m_canExecute?.Invoke(parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }

        public void Execute(object parameter)
        {
            this.m_execute(parameter);
        }

        #endregion // ICommand Members
    }
}
