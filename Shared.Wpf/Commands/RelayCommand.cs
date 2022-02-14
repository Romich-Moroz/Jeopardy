using System;
using System.Windows.Input;

namespace Shared.Wpf.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecuteAction;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action executeAction, Func<bool> canExecuteAction)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecuteAction = canExecuteAction;
        }

        public bool CanExecute(object? parameter)
        {

            return _canExecuteAction == null || _canExecuteAction();
        }

        public void Execute(object? parameter)
        {
            _executeAction();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _executeAction;
        private readonly Func<T?, bool> _canExecuteAction;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<T?> executeAction, Func<T?, bool> canExecuteAction)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecuteAction = canExecuteAction;
        }

        public bool CanExecute(object? parameter)
        {

            return _canExecuteAction == null || _canExecuteAction((T?)parameter);
        }

        public void Execute(object? parameter)
        {
            _executeAction((T?)parameter);
        }
    }
}