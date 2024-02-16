using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Voice_Synthesis.command;

public class PageRouter:ICommand
{
    private Frame mw;
    private Dictionary<string, Page> pgs;
    public PageRouter(Frame mainWindow,Dictionary<string, Page> pages)
    {
        mw = mainWindow;
        pgs = pages;
    }
    
    public event EventHandler? CanExecuteChanged;
    
    public bool CanExecute(object? parameter)
    {
        try {
            return pgs.ContainsKey(parameter as string);
        }catch { return false; }
    }

    public void Execute(object? parameter)
    {
        
        mw.Navigate(pgs[parameter as string]);
        

    }
}