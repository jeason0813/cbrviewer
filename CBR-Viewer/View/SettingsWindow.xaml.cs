#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace CBR_Viewer
{
    /// <summary>
    /// Description for SettingsWindow.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the SettingsWindow class.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, ReplyToMessage);
        }

        private void ReplyToMessage(NotificationMessage msg)
        {
            switch ((SendType)msg.Target)
            {
                case SendType.CloseSettings:
                    {
                        this.Close();
                        return;
                    }
            }

        }


    }
}