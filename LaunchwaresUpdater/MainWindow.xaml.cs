using LaunchwaresUpdater.AnimationHelpers;
using LaunchwaresUpdater.Core;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace LaunchwaresUpdater
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow Main;
        public Downloader _Client = new Downloader();
        public Timer CloseTimer = new Timer()
        {
            Interval = 2000
        };

        public MainWindow(StartupEventArgs e)
        {
            InitializeComponent();
            CloseTimer.Elapsed += CloseTimer_Elapsed;
            Main = this;

            var dict = new ResourceDictionary();
            try
            {
                if (Utils.Lang != null && Utils.Lang == "tr-TR")
                    dict.Source = new Uri($@"..\Resources\Locales\Locale.tr-TR.xaml", UriKind.Relative);
                else
                    dict.Source = new Uri($@"..\Resources\Locales\Locale.en-US.xaml", UriKind.Relative);
            }
            catch (Exception)
            {
                dict.Source = new Uri($@"..\Resources\Locales\Locale.en-US.xaml", UriKind.Relative);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
            Application.Current.Resources["application.title"] = Utils.LauncherName;
            LauncherLogo.ImageSource = ImageHelper.UriToBitmap(Utils.LauncherLogo);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlurHelper.MakeBlur(this);
            DownloadText.Text = $"{Application.Current.Resources["application.downloadingdatas"]}";

            Timer StartTimer = new Timer()
            {
                Interval = 1000
            };
            StartTimer.Start();
            StartTimer.Elapsed += (Sender, args) =>
            {
                var proc = Process.GetProcessesByName("Launchwares");
                if (proc.Length == 0)
                {
                    _Client.GetFiles();
                    StartTimer.Stop();
                }
            };
            StartTimer.Start();
        }

        private void TopPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        private void Main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        private void MinimizeProgram_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            this.WindowState = WindowState.Minimized;
        }

        private void CloseProgram_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            Application.Current.Shutdown();
        }

        private void CloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(delegate ()
            {
                Application.Current.Shutdown();
            });
        }
    }
}
