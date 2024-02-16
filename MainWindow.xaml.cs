using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PageInterface;
using Voice_Synthesis.view;

namespace Voice_Synthesis;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
///


public partial class MainWindow : Window
{
    
    public Mainview viewModel;
    private Button? _activeMenu;
    public Button? ActiveMenu
    {
        get => _activeMenu;
        set
        {
            if (_activeMenu != null)
            {
                _activeMenu.SetResourceReference(Button.StyleProperty, "MenuButton");
            }

            value.SetResourceReference(Button.StyleProperty, "MenuButtonActive");
            _activeMenu = value;
        }
    }
    
    public MainWindow()
    {
        InitializeComponent();
        Init();
    }

    public void Init()
    {
        viewModel = new Mainview(this);
        this.DataContext = viewModel;
        //ActiveMenu = DefaultMenu;
    }

    private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        DragMove();
        
    }
    private void WindowMinimized(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void WindowMaximized(object sender, MouseButtonEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
        }
        else
        {
            WindowState = WindowState.Maximized;
        }
    }
    private void WindowClosed(object sender, MouseButtonEventArgs e)
    {
        Close();
    }

    public void MenuOnClick(object sender, RoutedEventArgs e)
    {
        /*var dict = new Dictionary<string, string>();
        dict["Host"] = "666";
        dict["Port"] = "888";*/

        var but = sender as Button;
        ActiveMenu = but;
    }

    private void intercept(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
    }
}