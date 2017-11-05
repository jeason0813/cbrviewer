#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CBR_Viewer.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        SettingsWrapper settings;

        public RelayCommand OKCommand { get; private set; }

        public bool IsDynamic { get; set; }
        public const string IsDynamicPropertyName = "IsDynamic";
        public bool IsSavePosition { get; set; }
        public const string IsSavePositionPropertyName = "IsSavePosition";
        public bool IsSaveScaling { get; set; }
        public const string IsSaveScalingPropertyName = "IsSaveScaling";
        public int NumberOfRecent { get; set; }
        public const string NumberOfRecentPropertyName = "NumberOfRecent";


        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            this.settings = SettingsWrapper.Instance;

            this.OKCommand = new RelayCommand(() =>
            {
                SaveValues();
                //Close();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.CloseSettings, ""));
            });

            InitValues();
        }

        public void InitValues()
        {
            this.IsDynamic = this.settings.IsUseDynamicCommandBar;
            RaisePropertyChanged(IsDynamicPropertyName);
            this.IsSavePosition = this.settings.IsSaveMainWindowState;
            RaisePropertyChanged(IsSavePositionPropertyName);
            this.IsSaveScaling = this.settings.IsSaveScaling;
            RaisePropertyChanged(IsSaveScalingPropertyName);
            this.NumberOfRecent = this.settings.NumberOfRecent;
            RaisePropertyChanged(NumberOfRecentPropertyName);
        }

        public void SaveValues()
        {
            this.settings.IsUseDynamicCommandBar = this.IsDynamic;
            this.settings.IsSaveMainWindowState = this.IsSavePosition;
            this.settings.IsSaveScaling = this.IsSaveScaling;
            this.settings.NumberOfRecent = this.NumberOfRecent;
        }
    }
}