
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
        private bool inUpdate;
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            sTray = new Components.SystemTray();
            VerifyGame();
            //nSystem = new NotifySystem();
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
            //dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.dl_DownloadProgressChanged);
            //dl.DownloadFileCompleted += new AsyncCompletedEventHandler(dlCompleted);
            //dl.DownloadFileAsync(new Uri(file), downloadTo);


        }

        public void VerifyFiles()
        {
            int nb = 0;
            string currentFile;
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\app");
            FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);
            ZipArchive zip = ZipFile.Open(@"C:\xampp\htdocs\dofus/Zira 2.10.zip", ZipArchiveMode.Read);
            // progressBar.ValueChanged += verify_DownoadProgressChanged;
            Task.Run(() =>
            {
                foreach (var entry in fiArr)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (ZipArchiveEntry x in zip.Entries.Where(x => x.Name == entry.Name))
                        {
                            if (x != null)
                                if (x.LastWriteTime != entry.LastWriteTime)
                                    nb += 1;
                        }
                        infoUpdate.Content = "Vérification du fichier : " + entry.Name;
                        infoUpdateSecond.Content = nb + " / " + (fiArr.Length - 1) + " effectués";
                    }), DispatcherPriority.Render);
                    Thread.Sleep(20);
                   // nb += 1;
                }
            });
        }
      
        
        //    var length =
        //    1000;

        //        Task.Run(() =>
        //        {
        //            for (int i = 0; i <= length; i++)
        //            {
        //                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
        //                    infoUpdate.Content = "Test" + i;
        //                }), DispatcherPriority.Render);
        //                Thread.Sleep(100);
        //            }
        //        });
        //        //continue;
        //    }
        //}
                
            

        private void verify_DownoadProgressChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progressBar.Value = e.NewValue; 
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            VerifyFiles();
            //infoUpdate.Content = "Vérification des mises à jours... ";
            //Download(@"C:\xampp\htdocs\dofus/onvatest.zip", @"C:\Users\Matéo\source\repos\Filash\bin\Debug\net6.0-windows\app/onvatest.zip");
            // MessageBox.Show(InternetSpeed("https://www.google.com/") + " kb");
        }
        private void parameterButton_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.Shutdown();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            // this.Visibility = Visibility.Hidden;
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
        }

        private void VerifyGame()
        {
            if (File.Exists(@"app/auth.sql"))
            {
                playButton.Text = "Jouer";
            }
            else
            {
                playButton.Text = "Télécharger Filash";
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
            string zipFile = file;
            using (ZipArchive archive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
            {
                archive.ExtractToDirectory(path, true);
                MessageBox.Show("fini");
            }
        }
        private void researchAnotherVersion(object sender, RoutedEventArgs e)
        {
            //if (httpDownloader != null)
            //    httpDownloader.Pause();
        }
       
        void dl_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            progressBar.Value = e.ProgressPercentage;
            dlPercent.Content = percentage + "%";
            infoUpdate.Content = "Téléchargement en cours : " + Math.Round(bytesIn / 1000000, 2) + " Mo / " + Math.Round(totalBytes / 1000000000, 2)  + " Go";
        }
        #endregion

    }
}