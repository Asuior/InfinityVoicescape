using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Flurl.Http;
using System.Windows.Media;
using Flurl;

namespace Voice_Synthesis;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static ObservableCollection<string> ServerInfo;
    
    protected override void OnStartup(StartupEventArgs e)
    {
        ServerInfo = new ObservableCollection<string>();
        ServerInfo.Add("127.0.0.1");
        ServerInfo.Add("8899");
        ServerInfo.Add("closed");
        this.Exit += App_Exit;
        base.OnStartup(e);
        

        // 启动你的应用程序主

    }

    public static void SetInfo(ObservableCollection<string> info)
    {
        ServerInfo = info;
    }

    private async void App_Exit(object? sender,EventArgs e)
    {
        if (ServerInfo[3] == "running")
        {
            try
            {
                var response = await $"http://{ServerInfo[0]}:{ServerInfo[1]}".AppendPathSegment("/close").GetAsync();
            }
            catch (Exception exception)
            {
            
            }
        }
    }

}
    