using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Filash
{
    /// <summary>
    /// Logique d'interaction pour SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            this.nbGameWindows.SelectedIndex = Settings.Default.nbWindowGame;
            this.chkClose.IsChecked = Settings.Default.chkClose;
            this.chkOpen.IsChecked = Settings.Default.chkOpen;
        }

        private void CheckBoxClose_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.chkClose = (bool)chkClose.IsChecked;
            Settings.Default.Save();
        }

        private void CheckBoxClose_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.chkClose = (bool)chkClose.IsChecked;
            Settings.Default.Save();
        }
        private void CheckBoxOpen_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.chkOpen = (bool)chkOpen.IsChecked;
            Settings.Default.Save();
        }

        private void CheckBoxOpen_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.chkOpen = (bool)chkOpen.IsChecked;
            Settings.Default.Save();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.nbWindowGame = nbGameWindows.SelectedIndex;
            Settings.Default.Save();
            //MessageBox.Show((Settings1.Default.nbWindowGame).ToString());
        }

        private void folderButton_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "C:\\teste\\",
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
