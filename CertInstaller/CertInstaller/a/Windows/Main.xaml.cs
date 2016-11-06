using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CertInstaller.a.Model;

namespace CertInstaller.a.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        // Some private variables we use in the application.
        private bool IsAdmin = false;
        private bool FoundMsi = false;
        private Certificate cert = new Certificate();
        private List<string> Log = new List<string>();

        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// All the inirial loading and startup happens here.
        /// It also gets all the stuff needed for the program to run and checks if everything is OK.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get Status
            IsAdmin = a.Logic.Privileges.IsAdmin();
            FoundMsi = a.Logic.IO.foundMSI();
            cert = a.Logic.Certs.GetCert();

            // Set Admin
            lbl_Admin.Content = IsAdmin ? "YES" : "NO";
            lbl_Admin.Foreground = IsAdmin ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            // Set Admin
            lbl_MSI.Content = FoundMsi ? "YES" : "NO";
            lbl_MSI.Foreground = FoundMsi ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            if (cert.Success)
            {
                lbl_Certificate.Content = cert.Name;
                lbl_Thumbprint.Content = cert.Thumbprint;
                lbl_Before.Content = cert.Before.Date;
                lbl_Expires.Content = cert.Expires.Date;
                lbl_Certificate.Foreground = new SolidColorBrush(Colors.Green);
                lbl_Thumbprint.Foreground = new SolidColorBrush(Colors.Green);
                lbl_Before.Foreground = new SolidColorBrush(Colors.Green);
                lbl_Expires.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                lbl_Certificate.Content = "None found!";
                lbl_Thumbprint.Content = "-";
                lbl_Before.Content = "-";
                lbl_Expires.Content = "-";
                lbl_Certificate.Foreground = new SolidColorBrush(Colors.Red);
                lbl_Thumbprint.Foreground = new SolidColorBrush(Colors.Red);
                lbl_Before.Foreground = new SolidColorBrush(Colors.Red);
                lbl_Expires.Foreground = new SolidColorBrush(Colors.Red);
            }

            bool isReady = IsAdmin && FoundMsi && cert.Success;
            lbl_Ready.Foreground = isReady ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            lbl_Ready.Content = isReady ? "Ready" : "Not Ready!";
            btn_Install.IsEnabled = isReady;

        }

        /// <summary>
        /// Button for starting the CMD command installer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Install_Click(object sender, RoutedEventArgs e)
        {
            bool success = Install();
            lbl_Ready.Content = success ? "Completed OK" : "Error!";
            lbl_Ready.Foreground = success ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// Runs the CMD command to execute the install and passes the thumbprint to the command.
        /// </summary>
        /// <returns></returns>
        private bool Install()
        {
            // Options
            string filename = "JabraWebSocketServiceSetup.msi";
            string secure = "yes"; // yes|no
            string thumbprint = cert.Thumbprint;

            // Starting
            pBar.Value = 0;
            try
            {
                // Set Arguments for command
                pBar.Value = 25;
                string args = String.Format("/i {0} SECURESERVER={1} CERTIFICATETHUMBPRINT={2}", filename, secure, thumbprint);

                // Start installer
                pBar.Value = 50;
                Process psi = new Process();
                psi.StartInfo.UseShellExecute = false;
                psi.StartInfo.CreateNoWindow = false;
                psi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                psi.StartInfo.RedirectStandardOutput = true;
                psi.StartInfo.FileName = @"msiexec.exe";
                psi.StartInfo.Arguments = args;

                // Execute
                pBar.Value = 75;
                psi.Start();
                string output = psi.StandardOutput.ReadToEnd();
                psi.WaitForExit();
                Log.Insert(0, output);
                pBar.Value = 100;
                return true;
            }
            catch (Exception ex)
            {
                Log.Insert(0, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// View the log entries made by the Install Function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Log_Click(object sender, RoutedEventArgs e)
        {
            string result = "Logfile\n\n";
            foreach (string s in Log)
            {
                result += s + Environment.NewLine;
            }
            MessageBox.Show(result, "Logs", MessageBoxButton.OK);
        }

        private void btn_ListCerts_Click(object sender, RoutedEventArgs e)
        {
            var f = new ListCerts();
            f.ShowDialog();
        }
    }
}
