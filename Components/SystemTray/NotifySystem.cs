//using Launcher;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Text;
//using System.Windows;
//using System.Windows.Forms;

//namespace Filash.Components.SystemTray
//{
//    public class NotifySystem
//    {
//        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();

//        public NotifySystem()
//        {
//          nIcon.Icon = new Icon(Config.pathIconNotif);
//            //nIcon. = true;
//           // nIcon.ShowBalloonTip(5000, "Title", "Text", System.Windows.Forms.ToolTipIcon.Info);
//            nIcon.Click += NIcon_Click;
//        }
//        void NIcon_Click(object sender, EventArgs e)
//        {         
//            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
//            mainWindow.Visibility = Visibility.Visible;
//            mainWindow.WindowState = WindowState.Normal;
//        }
//        public void ShowNotify(int time, string title, string text, ToolTipIcon type)
//        {
//            nIcon.Visible = true;
//            nIcon.ShowBalloonTip(time, title, text, type);
//        }
//    }
//}
