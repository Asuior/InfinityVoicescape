using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.IconPacks;
using System.Windows.Media;
using Vlc.DotNet.Core;
using NAudio.Wave;
using Vlc.DotNet.Core.Interops.Signatures;

namespace Voice_Synthesis.control;

public partial class VideoPlay : UserControl
{
    private Boolean Isplaying = false;
    private Boolean restart = false;
    private DispatcherTimer timer;
    private Boolean ISUpdataSlider = true;
    private string uri;
    private void init()
    {
        AudioPlayer.Source = new Uri(uri);
        AudioSourceUpdated(uri);
        //AudioPlayer.MediaOpened += OnLoad;
        timer = new DispatcherTimer();
        // 设置定时器间隔为0.5秒
        timer.Interval = TimeSpan.FromSeconds(0.1);
        // 绑定定时器事件处理程序
        timer.Tick += UpdataSlider;
        // 启动定时器
        timer.Start();
    }
    
    private WaveStream? GetAudioFileReader(string filePath)
    {
        if (filePath.ToLower().EndsWith(".wav"))
        {
            return new WaveFileReader(filePath);
        }
        else if (filePath.ToLower().EndsWith(".mp3"))
        {
            return new Mp3FileReader(filePath);
        }

        return null;
    }
    
    private void AudioSourceUpdated(string path)
    {
        using (WaveStream? audio = GetAudioFileReader(path))
        {
            if (audio != null)
            {
                var sec = audio.TotalTime.TotalSeconds;
                ControlSlider.Maximum = sec;
                EnfTime.Text = ((int)(sec) / 60).ToString().PadLeft(2, '0') + ":" + ((int)(sec) % 60).ToString().PadLeft(2, '0');
                loading.Visibility = Visibility.Hidden; 
                PlayerControl.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("音频为不支持的格式");
            }
        }
        
        
        

    }
    private void OnLoad(object? sender,EventArgs e)
    {
        //ControlSlider.Maximum = AudioPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        //PlayerControl.Visibility = Visibility.Visible;

    }

    private void UpdataSlider(object? sender, EventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            if (ISUpdataSlider)
            {
                var sec = AudioPlayer.Position.TotalSeconds;
                ControlSlider.ValueChanged -= ControlSlider_OnValueChanged;
                ControlSlider.Value = sec;
                ControlSlider.ValueChanged += ControlSlider_OnValueChanged;
                StartTime.Text = ((int)(sec) / 60).ToString().PadLeft(2, '0') + ":" + ((int)(sec) % 60).ToString().PadLeft(2, '0');
            }
        });
    }

    public VideoPlay(string Paths)
    {
        InitializeComponent();
        uri = Paths;
        init();
    }
    


    private void ControlSlider_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ControlSlider_OnValueChanged(object sender, EventArgs e)
    {
        if (ISUpdataSlider)
        {
            if (restart)
            {
                restart = false;
                AudioPlayer.Play();
                Isplaying = true;
                ChangePlayerStatusButtom.Kind = PackIconEntypoKind.ControllerPaus;
            }
            var value = ControlSlider.Value;
            AudioPlayer.Position = TimeSpan.FromSeconds(value);
            

        }

    }
    private void ControlSlider_DragStarted(object sender, EventArgs e)
    {
        ISUpdataSlider = false;
    }
    private void ControlSlider_DragEnd(object sender, EventArgs e)
    {
        AudioPlayer.Position = TimeSpan.FromSeconds(ControlSlider.Value);
        ISUpdataSlider = true;
    }

    private void ChangePlayerStatusButtom_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!restart)
        {
            if (Isplaying)
            {
                AudioPlayer.Pause();
                Isplaying = false;
                ChangePlayerStatusButtom.Kind = PackIconEntypoKind.ControllerPlay;
            }else
            {
                AudioPlayer.Play();
                Isplaying = true;
                ChangePlayerStatusButtom.Kind = PackIconEntypoKind.ControllerPaus;
            }
        }
        else
        {
            restart = false;
            AudioPlayer.Play();
            AudioPlayer.Position = TimeSpan.FromSeconds(0);
            Isplaying = true;
            ChangePlayerStatusButtom.Kind = PackIconEntypoKind.ControllerPaus;
        }


    }

    private void AudioPlayer_OnMediaEnded(object sender, RoutedEventArgs e)
    {
        Isplaying = false;
        restart = true;
        AudioPlayer.Pause();
        ChangePlayerStatusButtom.Kind = PackIconEntypoKind.ControllerPlay;
    }

    private void LeftOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        AudioPlayer.Position = TimeSpan.FromSeconds(AudioPlayer.Position.TotalSeconds - 3);
    }

    private void RightOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        AudioPlayer.Position = TimeSpan.FromSeconds(AudioPlayer.Position.TotalSeconds + 3);
    }
}