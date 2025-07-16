using System.Windows.Input;

namespace CommonWpf
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<object?> _whatToDo;
        private readonly Predicate<object?>? _canExecute;

        public SimpleCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _whatToDo = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            else
            {
                return _canExecute.Invoke(parameter);
            }
        }

        public void Execute(object? parameter)
        {
            _whatToDo?.Invoke(parameter);
        }

        public void RaiseCanExecuteChangedEvent()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
