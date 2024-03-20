using System;
using System.Windows.Input;

namespace LpakViewClient.ModelView

{
    /// <summary>
    /// Реализация интерфейса ICommand для создания команд
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;
        /// <summary>
        /// Событие, которое вызывается, когда изменилась возможность выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        
 
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }
        /// <summary>
        /// Определяет, может ли команда выполняться.
        /// </summary>
        /// <param name="parameter">Параметр для оценки возможности выполнения команды.</param>
        /// <returns>True - команда может быть выполнена в противном случае - false.</returns>
 
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }
        /// <summary>
        /// Выполняет логику команды
        /// </summary>
        /// <param name="parameter">Параметр используемый при выполнении команды.</param>
        public void Execute(object parameter)
        {
            this._execute(parameter);
        }
    }
}