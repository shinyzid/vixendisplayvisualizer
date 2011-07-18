// --------------------------------------------------------------------------------
// Copyright (c) 2011 Erik Mathisen
// See the file license.txt for copying permission.
// --------------------------------------------------------------------------------
namespace Vixen.PlugIns.VixenDisplayVisualizer
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    /// <summary>
    ///   A command whose sole purpose is to 
    ///   relay its functionality to other
    ///   objects by invoking delegates. The
    ///   default return value for the CanExecute
    ///   method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        ///   The _can execute.
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        ///   The _execute.
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RelayCommand" /> class. 
        ///   Creates a new command.
        /// </summary>
        /// <param name = "execute">
        ///   The execution logic.
        /// </param>
        /// <param name = "canExecute">
        ///   The execution status logic.
        /// </param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }

        /// <summary>
        ///   The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        ///   The can execute.
        /// </summary>
        /// <param name = "parameter">
        ///   The parameter.
        /// </param>
        /// <returns>
        ///   The can execute.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null ? true : this._canExecute(parameter);
        }

        /// <summary>
        ///   The execute.
        /// </summary>
        /// <param name = "parameter">
        ///   The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this._execute(parameter);
        }
    }
}