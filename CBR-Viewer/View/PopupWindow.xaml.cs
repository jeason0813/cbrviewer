#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CBR_Viewer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public PopupWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, ReplyToMessage);
        }

        private void ReplyToMessage(NotificationMessage msg)
        {
            
            switch ((SendType)msg.Target)
            {
                case SendType.ClosePopup:
                    {
                        CBR_Viewer.ViewModel.ViewModelLocator.ClearViewModelPopup();
                        Messenger.Default.Unregister(this);
                        this.Close();
                        if (!string.IsNullOrEmpty(msg.Notification))
                        {
                            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenFileAnsw, msg.Notification));
                        }
                        return;
                    }
            }
        }

        
    }
}
