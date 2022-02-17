
using Filash;
using Filash.Components.SystemTray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Components.SystemTray sTray;
        private NotifySystem nSystem;
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            sTray = new Components.SystemTray();
            nSystem = new NotifySystem();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            //WebClient client = new WebClient();
            //client.DownloadFile(@"C:\xampp\htdocs\dofus\auth.sql", @"C:\dofusintaller");
            //Console.WriteLine("dl..");
            //client.Dispose();
        }
        private void parameterButton_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.Shutdown();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            nSystem.ShowNotify(5000, "Information", "Le launcher a été placer dans la barre des tâches", System.Windows.Forms.ToolTipIcon.Info);
            sTray.ShowIcon();
        }
        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void researchAnotherVersion(object sender, RoutedEventArgs e)
        {
            //if (httpDownloader != null)
            //    httpDownloader.Pause();
        }
    }
}