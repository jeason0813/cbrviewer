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
    public class MessageViewModel : ViewModelBase
    {
        public RelayCommand OKCommand { get; private set; }

        public string CaptionText { get; set; }
        public const string CaptionTextPropertyName = "CaptionText";
        public string ContentText { get; set; }
        public const string ContentTextPropertyName = "ContentText";


        /// <summary>
        /// Initializes a new instance of the MessageViewModel class.
        /// </summary>
        public MessageViewModel()
        {
            this.OKCommand = new RelayCommand(() =>
            {
            });
        }

    }
}