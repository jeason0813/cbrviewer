#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:CBR_Viewer.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
  
  OR (WPF only):
  
  xmlns:vm="clr-namespace:CBR_Viewer.ViewModel"
  DataContext="{Binding Source={x:Static vm:ViewModelLocatorTemplate.ViewModelNameStatic}}"
*/

using System;
namespace CBR_Viewer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// Use the <strong>mvvmlocatorproperty</strong> snippet to add ViewModels
    /// to this locator.
    /// </para>
    /// <para>
    /// In Silverlight and WPF, place the ViewModelLocatorTemplate in the App.xaml resources:
    /// </para>
    /// <code>
    /// &lt;Application.Resources&gt;
    ///     &lt;vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:CBR_Viewer.ViewModel"
    ///                                  x:Key="Locator" /&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// <para>
    /// Then use:
    /// </para>
    /// <code>
    /// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
    /// </code>
    /// <para>
    /// You can also use Blend to do all this with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// <para>
    /// In <strong>*WPF only*</strong> (and if databinding in Blend is not relevant), you can delete
    /// the Main property and bind to the ViewModelNameStatic property instead:
    /// </para>
    /// <code>
    /// xmlns:vm="clr-namespace:CBR_Viewer.ViewModel"
    /// DataContext="{Binding Source={x:Static vm:ViewModelLocatorTemplate.ViewModelNameStatic}}"
    /// </code>
    /// </summary>
    [CLSCompliant(false)]
    public class ViewModelLocator
    {
        private static MainViewModel _main;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view models
            ////}
            ////else
            ////{
            ////    // Create run time view models
            ////}

            CreateMain();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public static MainViewModel MainStatic
        {
            get
            {
                if (_main == null)
                {
                    CreateMain();
                }

                return _main;
            }
        }

        public static void SetAdornerToMainViewModel(AdornedControl.AdornedControl control)
        {
            MainStatic.LoadingAdorner = control;
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return MainStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the Main property.
        /// </summary>
        public static void ClearMain()
        {
            _main.Cleanup();
            _main = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the Main property.
        /// </summary>
        public static void CreateMain()
        {
            if (_main == null)
            {
                _main = new MainViewModel();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearMain();
            ClearViewModelSettings();
            ClearViewModelMessage();
            ClearAbout();
        }

        private static SettingsViewModel _vmSettings;

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        public static SettingsViewModel SettingsStatic
        {
            get
            {
                if (_vmSettings == null)
                {
                    CreateViewModelSettings();
                }

                return _vmSettings;
            }
        }

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsViewModel Settings
        {
            get
            {
                return SettingsStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the ViewModelPropertyName property.
        /// </summary>
        public static void ClearViewModelSettings()
        {
            if (_vmSettings == null)
            {
                return;
            }
            _vmSettings.Cleanup();
            _vmSettings = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the ViewModelPropertyName property.
        /// </summary>
        public static void CreateViewModelSettings()
        {
            if (_vmSettings == null)
            {
                _vmSettings = new SettingsViewModel();
            }
        }


        private static MessageViewModel _vmMessage;

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        public static MessageViewModel MessageStatic
        {
            get
            {
                if (_vmMessage == null)
                {
                    CreateViewModelMessage();
                }

                return _vmMessage;
            }
        }

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MessageViewModel Message
        {
            get
            {
                return MessageStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the ViewModelPropertyName property.
        /// </summary>
        public static void ClearViewModelMessage()
        {
            if (_vmMessage == null)
            {
                return;
            }
            _vmMessage.Cleanup();
            _vmMessage = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the ViewModelPropertyName property.
        /// </summary>
        public static void CreateViewModelMessage()
        {
            if (_vmMessage == null)
            {
                _vmMessage = new MessageViewModel();
            }
        }


        private static AboutViewModel _about;

        /// <summary>
        /// Gets the About property.
        /// </summary>
        public static AboutViewModel AboutStatic
        {
            get
            {
                if (_about == null)
                {
                    CreateAbout();
                }

                return _about;
            }
        }

        /// <summary>
        /// Gets the About property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AboutViewModel About
        {
            get
            {
                return AboutStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the About property.
        /// </summary>
        public static void ClearAbout()
        {
            if (_about == null)
            {
                return;
            }
            _about.Cleanup();
            _about = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the About property.
        /// </summary>
        public static void CreateAbout()
        {
            if (_about == null)
            {
                _about = new AboutViewModel();
            }
        }


        private static PopupViewModel _vmPopup;

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        public static PopupViewModel PopupStatic
        {
            get
            {
                if (_vmPopup == null)
                {
                    CreateViewModelPopup();
                }

                return _vmPopup;
            }
        }

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PopupViewModel Popup
        {
            get
            {
                return PopupStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the ViewModelPropertyName property.
        /// </summary>
        public static void ClearViewModelPopup()
        {
            if (_vmPopup == null)
            {
                return;
            }
            _vmPopup.Cleanup();
            _vmPopup = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the ViewModelPropertyName property.
        /// </summary>
        public static void CreateViewModelPopup()
        {
            if (_vmPopup == null)
            {
                _vmPopup = new PopupViewModel();
            }
        }



    }
}