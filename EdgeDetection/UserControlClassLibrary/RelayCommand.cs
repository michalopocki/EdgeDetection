using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canexecute;
    public RelayCommand(Action<object> execute) : this(execute, null) 
    { 
        _execute = execute; 
    }
    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this._canexecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add 
        { 
            CommandManager.RequerySuggested += value;
            CanExecuteChangedInternal += value;
        }
        remove 
        { 
            CommandManager.RequerySuggested -= value;
            CanExecuteChangedInternal -= value;
        }
    }

    public bool CanExecute(object parameter)
    {
        return _canexecute == null || _canexecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    private event EventHandler CanExecuteChangedInternal;
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChangedInternal.Raise(this);
    }
}
public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Predicate<T> _canExecute;

    public RelayCommand(Action<T> execute)
       : this(execute, null)
    {
        _execute = execute;
    }

    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}

