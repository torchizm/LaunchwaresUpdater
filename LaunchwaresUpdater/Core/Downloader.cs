using LaunchwaresUpdater.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace LaunchwaresUpdater.Core
{
    public class Downloader
    {
        WebClient Client = new WebClient();
        UpdateFiles FilesWillDownload = new UpdateFiles();
        int DownloadedFiles = 0;
        int DownloadPercentage = 0;

        public Downloader()
        {
            Client.DownloadProgressChanged += Client_DownloadProgressChanged;
            Client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }

        public async void GetFiles()
        {
            HttpClient HttpClient = new HttpClient();
            var Request = await HttpClient.GetAsync($"https://launchwares.com/storage/updates/{Utils.UpdateToken}/files.json");
            var Response = Request.Content.ReadAsStringAsync().Result;

            if (Response == null || Response == "")
            {
                MessageBox.Show($"{Application.Current.Resources["application.noupdate"]}", $"{Application.Current.Resources["application.title"]}", MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(0);
            }

            FilesWillDownload = JsonConvert.DeserializeObject<UpdateFiles>(Response);
            Download();
        }

        public void Download()
        {
            Download(new Uri(FilesWillDownload.List[DownloadedFiles]), FilesWillDownload.List[DownloadedFiles].Split('/').LastOrDefault());
        }

        public void Download(Uri Link, string Path)
        {
            Client.DownloadFileAsync(Link, $"{AppDomain.CurrentDomain.BaseDirectory}{Path}");
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadPercentage = e.ProgressPercentage;
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                MainWindow.Main.DownloadText.Text = $"{Application.Current.Resources["application.downloadingfiles"].ToString().Replace("%percentage%", DownloadPercentage.ToString()).Replace("%downloaded%", DownloadedFiles.ToString()).Replace("%total%", FilesWillDownload.List.Count.ToString())}";
            });
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadedFiles++;
            DownloadPercentage = 0;
            Application.Current.Dispatcher.Invoke(delegate ()
            {
                MainWindow.Main.DownloadText.Text = $"{Application.Current.Resources["application.downloadingfiles"].ToString().Replace("%percentage%", DownloadPercentage.ToString()).Replace("%downloaded%", DownloadedFiles.ToString()).Replace("%total%", FilesWillDownload.List.Count.ToString())}";
            });

            if (DownloadedFiles < FilesWillDownload.List.Count)
            {
                Download();
            }
            else if (DownloadedFiles == FilesWillDownload.List.Count)
            {
                Application.Current.Dispatcher.Invoke(delegate ()
                {
                    MainWindow.Main.DownloadText.Text = $"{Application.Current.Resources["application.finished"]}";
                });
                MainWindow.Main.CloseTimer.Start();
            }
        }
    }
}
