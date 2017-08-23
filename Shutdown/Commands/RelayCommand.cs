using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shutdown.Commands
{
    /// <summary>
    /// Defines a command with delegate for execute and can execute
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        /// <summary>
        /// The method to be called when the command is invoked.
        /// </summary>
        Action _execute;

        /// <summary>
        /// The method that determines whether the command can execute in its current state.
        /// </summary>
        Func<bool> _canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Construct a relay command
        /// </summary>
        /// <param name="execute">the method to be called when the command is invoked.</param>
        /// <param name="canExecute">the method that determines whether the command can execute in its current state.</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.</param>
        public void Execute(object parameter) => _execute();
    }

    /// <summary>
    /// Defines a command with delegate for execute and can execute
    /// </summary>
    /// <typeparam name="T">The type of object to use with execute and can execute methods</typeparam>
    public sealed class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// The method to be called when the command is invoked.
        /// </summary>
        Action<T> _execute;

        /// <summary>
        /// The method that determines whether the command can execute in its current state.
        /// </summary>
        Func<T,bool> _canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Construct a relay command
        /// </summary>
        /// <param name="execute">the method to be called when the command is invoked.</param>
        /// <param name="canExecute">the method that determines whether the command can execute in its current state.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.</param>
        public void Execute(object parameter) => _execute((T)parameter);
    }
}
