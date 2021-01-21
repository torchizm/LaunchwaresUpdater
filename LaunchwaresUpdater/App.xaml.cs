using LaunchwaresUpdater.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace LaunchwaresUpdater
{
    /// <summary>
    /// App.xaml etkileşim mantığı
    /// </summary>
    public partial class App : Application
    {
        private List<string> _Args { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _Args = e.Args.ToList();

            //if (!WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
            //{
            //    MessageBox.Show("Program yönetici izni dışında çalıştırılamaz.");
            //    Current.Shutdown(0);
            //}

            if (_Args.Count == 0)
            {
                MessageBox.Show("Program düzgün şekilde başlatılamadı.");
                Current.Shutdown(0);
            }
            else
            {
                Utils.UpdateToken = GetValue("-token");
                Utils.LauncherName = GetValue("-name");
                Utils.LauncherLogo = GetValue("-photopath");
                Utils.Lang = GetValue("-lang");
            }

            MainWindow main = new MainWindow(e);
            main.Show();
        }

        public string GetValue(string key)
        {
            if (!_Args.Contains(key)) return "Girdi bulunamadı.";

            string raw = "";
            _Args.ForEach(x =>
            {
                raw += $"{x} ";
            });

            return Regex.Match(raw, $"(?<={key} ).*?(?= -)").Value ?? "Girdi bulunamadı.";
        }
    }
}
