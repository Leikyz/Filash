
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

            if (!VerifyGame())
                downloadButton.Content = "Télécharger Dofus";
            else if (NeedUpdate())
                downloadButton.Content = "Faire la mise à jour";
            else
                downloadButton.Content = "Jouer";
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
        public bool NeedUpdate()
        {
            bool result = false;
            var wc = new WebClient();
            var stream = wc.OpenRead("http://localhost/dofus/update.zip");
            var zip = new ZipArchive(stream);
            FileInfo file = new FileInfo(Directory.GetCurrentDirectory() + "\\app\\version.txt");

            if (file != null)
            {
                foreach (ZipArchiveEntry x in zip.Entries.Where(x => x.Name == "version.txt"))
                {
                    if (x != null)
                    {
                        if (file.LastWriteTime.Hour == x.LastWriteTime.Hour && x.LastWriteTime.Minute == file.LastWriteTime.Minute)
                            result = false;
                        else
                            result = true;
                    }
                    else
                        result = true;
                }
            }
            return result;
        }


        public void VerifyUpdate()
        {
            int nb = 0;
            infoUpdateSecond.Visibility = Visibility.Visible;
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\app");
            FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);
            var wc = new WebClient();
            var stream = wc.OpenRead("http://localhost/dofus/update.zip");         
            downloadButton.Content = "Réparation des fichiers";
            Task.Run(() =>
            {
                foreach (var entry in fiArr)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        infoUpdate.Content = "Vérification du fichier : " + entry.Name + nb + "            " + nb + " / " + (fiArr.Length - 1) + " effectués";
                       // infoUpdateSecond.Content = nb + " / " + (fiArr.Length - 1) + " effectués";
                    }), DispatcherPriority.Render);
                    Thread.Sleep(1);
                    nb += 1;  
                }
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    infoUpdate.Content = "Connexion au serveur de mises à jours";
                    infoUpdateSecond.Visibility = Visibility.Hidden;
                    if (!NeedUpdate())
                    {
                        infoUpdate.Content = "Vous êtes à jour";
                        downloadButton.Content = "Jouer";
                        inUpdate = false;
                    }
                    else
                        infoUpdate.Content = "Une mise à jours est disponible";
                        downloadButton.Content = "Télécharger la mise à jours";
                }), DispatcherPriority.Render);
                Thread.Sleep(10);
                inUpdate = false;
            });     

        }
       
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {

            if (downloadButton.Content == "Jouer")
                Process.Start(@"app/Dofus.exe");
            else
            {
                if (downloadButton.Content == "Télécharger Dofus")
                    Download(@"C:\xampp\htdocs\dofus/client.zip", @"C:\Users\Matéo\source\repos\Moi\bin\Debug\net6.0-windows/update.zip");
                else
                    Download(@"C:\xampp\htdocs\dofus/update.zip", @"C:\Users\Matéo\source\repos\Moi\bin\Debug\net6.0-windows/update.zip");
                downloadButton.Content = "Téléchargement en cours";
                downloadButton.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEFFD8"));
                infoUpdate.Content = "Vérification des mises à jours... ";
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
            Unzip(@"C:\Users\Matéo\source\repos\Moi\bin\Debug\net6.0-windows/update.zip", @"C:\Users\Matéo\source\repos\Moi\bin\Debug\net6.0-windows/app");
            downloadButton.Content = "Jouer";
            infoUpdate.Content = "Votre jeu est à jours";
            dlPercent.Visibility = Visibility.Hidden;
        }

        private bool VerifyGame()
        {
            bool result;
            if (File.Exists(@"app/dofus.exe"))
                result = true;
            else
                result = false;
            return result;
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
            VerifyUpdate();
        }  
    }
}