using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HandyControl.Controls;
using MahApps.Metro.Controls;
using Voice_Synthesis.command;
using PageInterface;
using MessageBox = System.Windows.MessageBox;

namespace Voice_Synthesis.view;



public struct Infomation
{
    public string Name { get; set; }
    public BitmapImage Cover { get; set; }
    public string url { get; set; }
}


public class Mainview:INotifyPropertyChanged
{
    private List<Infomation> pageInfos;
    
    
    private ICommand menuChangePageCommand;
    
    public ICommand MenuChangePageCommand
    {
        get => menuChangePageCommand;
        set => menuChangePageCommand = value;
    }
    private Dictionary<string, Page> pageDict;
    public Dictionary<string, Page> PageDict
    {
        get => pageDict;
        set => pageDict = value;
    }
    private ObservableCollection<string> serverInfo;
    public ObservableCollection<string> ServerInfo
    {
        get => serverInfo;
        set
        {
            serverInfo = value;
            OnPropertyChanged(nameof(ServerInfo));
            
        }
    }

    public List<Infomation> PageInfos
    {
        get => pageInfos;
        set
        {
            pageInfos = value;
            OnPropertyChanged(nameof(PageInfos));
        }
    }

    private MainWindow mv;

    


    public event PropertyChangedEventHandler? PropertyChanged;
    
    
    
    public Mainview(MainWindow mainWindow)
    {
        mv = mainWindow;
        PageInfos = new List<Infomation>();
        PageDict = new Dictionary<string, Page>();
        ServerInfo = new ObservableCollection<string>();
        ServerInfo.Add("127.0.0.1"); 
        ServerInfo.Add("8866");
        ServerInfo.Add("仅启动控制界面");
        ServerInfo.Add("closed");
        ServerInfo.Add("");
        ServerInfo.CollectionChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(ServerInfo));
        };
        App.SetInfo(ServerInfo);
        string[] files = Directory.GetFiles("pages", $"*.dll");
        var first = true;
        foreach (var file in files)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(file);
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (typeof(VsPage).IsAssignableFrom(type) && typeof(Page).IsAssignableFrom(type) && !type.IsInterface)
                    {

                        VsPage temp  = Activator.CreateInstance(type) as VsPage;
                        temp.SetInfo(ServerInfo);
                        var pageinfo = temp.GetInfo();
                        var info = new Infomation() {Name=pageinfo.Name,Cover=new BitmapImage(new Uri(pageinfo.Cover)),url=pageinfo.url };
                        PageInfos.Add(info);
                        Page pT = temp as object as Page;
                        PageDict[pageinfo.url] = pT;
                        if (first)
                        {
                            mainWindow.MainFrame.Navigate(pT);
                            first = false;
                        }
                        else
                        {
                        
                        }
                    }
                }

                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        mv.Menu.Loaded += Menuinit;
        
        MenuChangePageCommand = new PageRouter(mainWindow.MainFrame,PageDict);
        
    }



    private void Menuinit(object? sender, EventArgs e)
    {
        if (PageInfos.Count > 0)
        {
            mv.ActiveMenu = mv.Menu.FindChild<Button>();
        }
        mv.Menu.Initialized -= Menuinit;
    }
    
    protected virtual void OnPropertyChanged( string? propertyName = null)
    {

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}