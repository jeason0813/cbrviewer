#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBR_Viewer.ViewModel
{
    public class SettingsWrapper
    {
        private static readonly SettingsWrapper instance = new SettingsWrapper();

        public int Height { get; set; }
        public int Width { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public System.Windows.WindowState WindowState { get; set; }
        public bool IsUseDynamicCommandBar { get; set; }
        public bool IsSaveMainWindowState { get; set; }
        public bool IsSaveScaling { get; set; }
        public int NumberOfRecent { get; set; }
        public CBR_Viewer.Model.ScaleType Scaling { get; set; }

        private SettingsWrapper()
        {
            //global::CBR_Viewer.Properties.Settings.Default.Reload();
            CBR_Viewer.Model.SettingsTools.ReadSettings();
            this.IsSaveMainWindowState=global::CBR_Viewer.Properties.Settings.Default.IsSaveMainWindowState;
            if (this.IsSaveMainWindowState)
            {
                this.Height = global::CBR_Viewer.Properties.Settings.Default.MainWindowHeight;
                this.Width = global::CBR_Viewer.Properties.Settings.Default.MainWindowWidth;
                this.Left = global::CBR_Viewer.Properties.Settings.Default.MainWindowLeft;
                this.Top = global::CBR_Viewer.Properties.Settings.Default.MainWindowTop;
                this.WindowState = global::CBR_Viewer.Properties.Settings.Default.MainWindowState;
            }
            else
            {
                ResetWindow();
            }
            this.IsUseDynamicCommandBar = global::CBR_Viewer.Properties.Settings.Default.IsUseDynamicCommandBar;
            this.IsSaveScaling = global::CBR_Viewer.Properties.Settings.Default.IsSaveScaling;
            int i = global::CBR_Viewer.Properties.Settings.Default.Scaling;
            switch (i)
            {
                case 0:
                    {
                        this.Scaling = Model.ScaleType.ScaleOne;
                        break;
                    }
                case 1:
                    {
                        this.Scaling = Model.ScaleType.ScaleFit;
                        break;
                    }
                case 2:
                    {
                        this.Scaling = Model.ScaleType.ScaleWidth;
                        break;
                    }
                case 3:
                    {
                        this.Scaling = Model.ScaleType.ScaleHeight;
                        break;
                    }
                default:
                    {
                        this.Scaling = Model.ScaleType.ScaleOne;
                        break;
                    }
            }
            this.NumberOfRecent = global::CBR_Viewer.Properties.Settings.Default.NumberOfRecent;
        }

        public void ResetWindow()
        {
            this.Height = 600;
            this.Width = 900;
            this.Left = 50;
            this.Top = 50;
            this.WindowState = System.Windows.WindowState.Normal;
        }

        public void SaveSettings()
        {
            if (!this.IsSaveMainWindowState)
            {
                ResetWindow();
            }
            global::CBR_Viewer.Properties.Settings.Default.MainWindowHeight = this.Height;
            global::CBR_Viewer.Properties.Settings.Default.MainWindowWidth = this.Width;
            global::CBR_Viewer.Properties.Settings.Default.MainWindowLeft = this.Left;
            global::CBR_Viewer.Properties.Settings.Default.MainWindowTop = this.Top;
            global::CBR_Viewer.Properties.Settings.Default.MainWindowState = this.WindowState;
            global::CBR_Viewer.Properties.Settings.Default.IsUseDynamicCommandBar = this.IsUseDynamicCommandBar;
            global::CBR_Viewer.Properties.Settings.Default.IsSaveMainWindowState = this.IsSaveMainWindowState;
            if (!this.IsSaveScaling)
            {
                this.Scaling = Model.ScaleType.ScaleOne;
            }
            switch (this.Scaling)
            {
                case Model.ScaleType.ScaleOne:
                    {
                        global::CBR_Viewer.Properties.Settings.Default.Scaling = 0;
                        break;
                    }
                case Model.ScaleType.ScaleFit:
                    {
                        global::CBR_Viewer.Properties.Settings.Default.Scaling = 1;
                        break;
                    }
                case Model.ScaleType.ScaleWidth:
                    {
                        global::CBR_Viewer.Properties.Settings.Default.Scaling = 2;
                        break;
                    }
                case Model.ScaleType.ScaleHeight:
                    {
                        global::CBR_Viewer.Properties.Settings.Default.Scaling = 3;
                        break;
                    }
                default:
                    {
                        global::CBR_Viewer.Properties.Settings.Default.Scaling = 0;
                        break;
                    }
            }
            global::CBR_Viewer.Properties.Settings.Default.IsSaveScaling = this.IsSaveScaling;
            global::CBR_Viewer.Properties.Settings.Default.NumberOfRecent = this.NumberOfRecent;
            //global::CBR_Viewer.Properties.Settings.Default.Save();
            CBR_Viewer.Model.SettingsTools.WriteSettings();
        }

        public static SettingsWrapper Instance
        {
            get
            {
                return instance;
            }
        }

        public string Title
        {
            get
            {
                string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string[] version = ver.Split('.');
                StringBuilder sb = new StringBuilder();
                sb.Append("Gazillion-Bytes CBReader [");
                sb.Append(version[0]);
                sb.Append('.');
                sb.Append(version[1]);
                sb.Append(']');
                return sb.ToString();
            }
        }

    }
}
