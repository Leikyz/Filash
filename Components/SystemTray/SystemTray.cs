using Filash;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace Filash.Components
{
    public class SystemTray 
    {
        private NotifyIcon _notifyIcon;

        public SystemTray()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(IconPath()),
                Text = "Leikyz"
            };
            _notifyIcon.DoubleClick += NotifyIcon_Click;
            //_notifyIcon.Visible = true;
            //_notifyIcon.ShowBalloonTip(5000, "titre", "text", System.Windows.Forms.ToolTipIcon.Info);
            //_notifyIcon.Click += nIcon_Click;
        }

        //private void nIcon_Click(object sender, EventArgs e)
        //{
        //    var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        //    mainWindow.Visibility = Visibility.Visible;
        //    mainWindow.WindowState = WindowState.Normal;
        //}

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            OpenApplication();
        }
        private void OpenApplication()
        {
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            try
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.Visibility = Visibility.Visible;
                mainWindow.Activate();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Un problème est survenu lors de l'ouverture du launcher, veuillez relancer le launcher");
            }
        }
        public void ContextMenuStrip()
        {
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Ouvrir", Image.FromFile(IconPath()), OnOpenClicked);
            _notifyIcon.ContextMenuStrip.Items.Add("Jouer", Image.FromFile(IconPath()), OnPlayClicked);
            _notifyIcon.ContextMenuStrip.Items.Add("Quitter", Image.FromFile(IconPath()), OnLeaveClicked);
        }

        private void OnLeaveClicked(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnOpenClicked(object sender, EventArgs e)
        {
            OpenApplication();
        }

        public string IconPath()
        {
            return Config.pathIconNotif;
        }
        public void ShowIcon()
        {
            ContextMenuStrip();
            _notifyIcon.Visible = true;
        }

        public void HideIcon()
        {
            _notifyIcon.Dispose();
        }
    }
}
