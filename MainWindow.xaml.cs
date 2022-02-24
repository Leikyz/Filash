
using Filash;
//using Filash.Components.SystemTray;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Filash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Components.SystemTray sTray;
        private bool inUpdate = false;
        public MainWindow()
        {
            
            InitializeComponent();
            MouseDown += Window_MouseDown;
            sTray = new Components.SystemTray();
            infoUpdateSecond.Visibility = Visibility.Hidden;
            VerifyGame();
        }
        #region button
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        public void Download(string file, string downloadTo)
        {
            infoUpdate.Content = "Vérification des mises à jours... ";
           // Thread.sus(1000);
            WebClient dl = new WebClient();
            dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.dl_DownloadProgressChanged);
            dl.DownloadFileCompleted += new AsyncCompletedEventHandler(dlCompleted);
            dl.DownloadFileAsync(new Uri(file), downloadTo);
            dl.Dispose();


        }
        public DateTimeOffset ReadRemoteFile()
        {
            DateTimeOffset result = new DateTimeOffset();
            var wc = new WebClient();
            var stream = wc.OpenRead("http://localhost/dofus/onvatest.zip");
            var zip = new ZipArchive(stream);
            foreach (ZipArchiveEntry x in zip.Entries.Where(x => x.Name == "launcherVersion.txt"))
            {
                if (x != null)
                    result = x.LastWriteTime;
            }
            return result;
        }


        public void VerifyUpdate()
        {
            int nb = 0;
            infoUpdateSecond.Visibility = Visibility.Visible;
            DateTimeOffset version = new DateTimeOffset();
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\app");
            FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);
            Task.Run(() =>
            {
                foreach (var entry in fiArr)
                {
                    if (entry.Name == "launcherVersion.txt")
                        version = entry.LastWriteTime;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        infoUpdate.Content = "Vérification du fichier : " + entry.Name + nb;
                        infoUpdateSecond.Content = nb + " / " + (fiArr.Length - 1) + " effectués";
                    }), DispatcherPriority.Render);
                    Thread.Sleep(1);
                    nb += 1;  
                }
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    infoUpdate.Content = "Verification de mises à jours ";
                    infoUpdateSecond.Visibility = Visibility.Hidden;
                    if (version.Hour == ReadRemoteFile().Hour && version.Minute == ReadRemoteFile().Minute)
                    {
                        infoUpdate.Content = "Vous êtes à jour";
                        downloadButton.Content = "Jouer";
                        inUpdate = false;
                    }
                    else
                        infoUpdate.Content = "Une mise à jours est disponible";
                }), DispatcherPriority.Render);
                Thread.Sleep(10);
                inUpdate = false;
            });     

        }
        private void CheckUpdate()
        {
            DateTimeOffset version = new DateTimeOffset();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                infoUpdate.Content = "Verification de mises à jours ";
                infoUpdateSecond.Visibility = Visibility.Hidden;
                if (version.Hour == ReadRemoteFile().Hour && version.Minute == ReadRemoteFile().Minute)
                {
                    infoUpdate.Content = "Vous êtes à jour";
                    downloadButton.Content = "Jouer";
                    inUpdate = false;
                }
                else
                    infoUpdate.Content = "Une mise à jours est disponible";
            }), DispatcherPriority.Render);
            Thread.Sleep(10);
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\app\\client_renouveau\\Dofus.exe"))
            {
                Process.Start(@"app\client_renouveau/Dofus.exe");
            }
            else
            {
                downloadButton.Content = "Téléchargement e...";
                downloadButton.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEFFD8"));
                infoUpdate.Content = "Vérification des mises à jours... ";
                Download(@"C:\xampp\htdocs\dofus/onvatest.zip", @"C:\Users\Matéo\source\repos\Filash\bin\Debug\net6.0-windows\app/onvatest.zip");
                //Unzip(@"C:\Users\Matéo\source\repos\Filash\bin\Debug\net6.0-windows\app/onvatest.zip", @"C:\Users\Matéo\source\repos\Filash\bin\Debug\net6.0-windows\app");
            }
        }
        private void parameterButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            SettingWindow close = new SettingWindow();
            close.Show();
            MessageBox.Show(Settings.Default.chkClose.ToString());
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            CloseWindow close = new CloseWindow();
            close.Show();
            //nSystem.ShowNotify(5000, "Information", "Le launcher a été placer dans la barre des tâches", System.Windows.Forms.ToolTipIcon.Info);
            sTray.ShowIcon();
        }
        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region download

        private void dlCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            Unzip(@"C:\Users\Matéo\source\repos\Filash\bin\Debug\net6.0-windows\app/onvatest.zip", @"C:\Users\Matéo\source\repos\Filash\bin\Debug\net6.0-windows\app");
            downloadButton.Content = "Jouer";
            infoUpdate.Content = "Votre jeu est à jours";
            dlPercent.Visibility = Visibility.Hidden;
        }

        private void VerifyGame()
        {
            if (File.Exists(@"app\client_renouveau/version.txt"))
            {
                downloadButton.Content = "Jouer";
            }
            else
            {
                downloadButton.Content = "Télécharger Dofus";
            }
        }
        //public bool 
        public static double InternetSpeed(string url)
        {
            WebClient wc = new WebClient();
            DateTime dt1 = DateTime.Now;
            byte[] data = wc.DownloadData("https://www.google.com/");
            DateTime dt2 = DateTime.Now;
            return ((data.Length * 8) / (dt2 - dt1).TotalSeconds) / 1024;
        }
        public void Unzip(string file, string path)
        {
            ZipFile.ExtractToDirectory(file, path, true);
           // MessageBox.Show("Terminé");
        }
       
        void dl_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            progressBar.Value = e.ProgressPercentage;
            dlPercent.Content = (int)percentage + "%";
            infoUpdate.Content = "Téléchargement en cours : " + Math.Round(bytesIn / 1000000, 2) + " Mo / " + Math.Round(totalBytes / 1000000000, 2)  + " Go";
        }
        #endregion

        private void btnPar_Copy_Click(object sender, RoutedEventArgs e)
        {
            inUpdate = true;
            downloadButton.Content = "Vérification en cours";
            VerifyUpdate();
        }
    }
}