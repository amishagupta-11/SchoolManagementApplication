using System.Windows.Input;
namespace SchoolManagementApplication.ViewModel;
public class RelayCommand : ICommand
{
    private Action<object> execute;
    private  Predicate<object> canExecute;  
    
    /// <summary>
    /// contructor to set up command with the logic to executed and optionally determines whether the command can be executed on not.                                                          
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute=null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;

    }
   
    /// <summary>
    /// Event method to check whether a particular event can be 
    /// performed on property change and accordingly changing the respective values 
    /// </summary>

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }


    /// <summary>
    /// method to check whether a command can be executed or not.
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object parameter)
    {
        return canExecute == null || canExecute(parameter);
    }

    /// <summary>
    /// method to perform command execution.
    /// </summary>
    /// <param name="parameter"></param>

    public void Execute(object parameter)
    {
        execute(parameter);
    }
   
}
