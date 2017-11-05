using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.IO;
using CBR_Viewer.ViewModel;

namespace CBR_Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {            
            if ((e.Args != null) && (e.Args.Length > 0))
            {
                ViewModelLocator.MainStatic.StartupPath = e.Args[0];                
                //MessageBox.Show("startup ... " + StartPath);
            }
            else
            {
                ViewModelLocator.MainStatic.StartupPath = "";
            }
        }

    }

    public enum SendType
    {
        DoScrollHome,
        DoScrollLineDown,
        DoScrollPageDown,
        DoScrollLineUp,
        DoScrollPageUp,
        ResetScroll,
        RequestMRU,
        OpenFileReq,
        OpenPopup,
        OpenSettingsReq,
        OpenAboutReq,
        OpenFileAnsw,
        ResizeAnsw,
        LoadFinished,
        MenuReload,
        FocusListBox,
        CloseSettings,
        ClosePopup,
        ShowMessage,
    }
}
