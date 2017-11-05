#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using CBR_Viewer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;

namespace CBR_Viewer.ViewModel
{
    public class PopupViewModel : ViewModelBase
    {
        bool isClosing;
        public RelayCommand LostKeyboardFocusCommand { get; private set; }
        public RelayCommand GotKeyboardFocusCommand { get; private set; }

        public RelayCommand ActivatedCommand { get; private set; }
        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public PopupViewModel()
        {
            this.isClosing = false;
            this.LostKeyboardFocusCommand = new RelayCommand(() =>
            {
                if (!this.isClosing)
                {
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ClosePopup, ""));
                }
            });

            this.GotKeyboardFocusCommand = new RelayCommand(() =>
            {
                this.isClosing = true;
            });

            this.ActivatedCommand = new RelayCommand(() =>
            {
            });
            
        }

        private List<IndexedMRUItem> items = MRU.Instance.GetMRUItems();
        public List<IndexedMRUItem> MyRecentItems
        {
            get
            {
                return items;
            }
        }

        private int _selectedIndex = -1;
        public int SelIndex
        {
            get
            {
                return this._selectedIndex;
            }
            set
            {
                this._selectedIndex = value;
                if (value >= 0)
                {
                    System.Diagnostics.Debug.WriteLine("this._selectedIndex {0}", value);

                    //Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenFileAnsw, this.items[value].FullFileName));
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ClosePopup, this.items[value].FullFileName));
                }
            }
        }



		public override void Cleanup()
        {

            base.Cleanup();
        }
    }
}