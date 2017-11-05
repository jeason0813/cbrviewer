#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System.Windows;
using CBR_Viewer.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace CBR_Viewer
{
    struct OffsetTime
    {
        public double VerticalOffset;
        public System.DateTime Time;
    }
    /// <summary>
    /// This application's main window.
    /// </summary>
    public partial class MainWindow : Window
    {
        private OffsetTime ot;
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.ot.VerticalOffset = double.NaN;
            this.ot.Time = System.DateTime.Now;
            ViewModelLocator.SetAdornerToMainViewModel(this.LoadingAdorner);
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<NotificationMessage>(this, ReplyToMessage);

            //System.Windows.Media.ImageBrush ib = new System.Windows.Media.ImageBrush();
            //ib.Viewbox = new Rect();
            //ib.Viewport = new Rect();
            
        }

        private void ReplyToMessage(NotificationMessage msg)
        {
            Window win = null;
            if(!(msg.Target is SendType))
            {
                return;
            }
            switch ((SendType)msg.Target)
            {
                case SendType.DoScrollHome:
                    {
                        this.sv.ScrollToHome();
                        return;
                    }
                case SendType.ResetScroll:
                    {
                        this.ot.VerticalOffset = double.NaN;
                        this.ot.Time = System.DateTime.Now;
                        return;
                    }
                case SendType.DoScrollLineDown:
                    {
                        if (this.ot.VerticalOffset != this.sv.ContentVerticalOffset)
                        {
                            this.sv.LineDown();
                            this.ot.VerticalOffset = this.sv.ContentVerticalOffset;
                            this.ot.Time = System.DateTime.Now;
                        }
                        else
                        {
                            if((System.DateTime.Now-this.ot.Time)>System.TimeSpan.FromMilliseconds(500))
                            {
                                // page down
                                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollPageDown, "DoScroll"));
                            }
                        }

                        return;
                    }
                case SendType.DoScrollLineUp:
                    {
                        if (this.ot.VerticalOffset != this.sv.ContentVerticalOffset)
                        {
                            this.sv.LineUp();
                            this.ot.VerticalOffset = this.sv.ContentVerticalOffset;
                            this.ot.Time = System.DateTime.Now;
                        }
                        else
                        {
                            if ((System.DateTime.Now - this.ot.Time) > System.TimeSpan.FromMilliseconds(500))
                            {
                                // page up
                                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollPageUp, "DoScroll"));
                            }
                        }

                        return;
                    }
                case SendType.OpenAboutReq:
                    {
                        win = new AboutWindow();
                        break;
                    }
                case SendType.OpenFileReq:
                    {
                        DoOpenFile();
                        return;
                    }
                case SendType.OpenSettingsReq:
                    {
                        win = new SettingsWindow();
                        break;
                    }
                case SendType.LoadFinished:
                    {
                        return;
                    }
                case SendType.OpenPopup:
                    {
                        var transform = this.menuButton.TransformToVisual(this as FrameworkElement);
                        Point absolutePosition = transform.Transform(new Point(0, 0));
                        win = new PopupWindow();
                        win.Owner = this;
                        win.Icon = this.Icon;
                        win.Left = absolutePosition.X + this.menuButton.Width;
                        win.Top = absolutePosition.Y;
                        win.Show();
                        return;
                    }
                case SendType.FocusListBox:
                    {
                        this.listBox1.Focus();
                        return;
                    }
                case SendType.ShowMessage: 
                    {
                        win = new MessageWindow();
                        break;
                    }
            }


            if (win != null)
            {
                win.Icon = this.Icon;
                win.Owner = this;
                win.ShowDialog();
            }
        }

        private void DoOpenFile()
        {
            Microsoft.Win32.OpenFileDialog win = new Microsoft.Win32.OpenFileDialog();
            win.Title = "Select CBR to Open";
            win.Multiselect = false;
            win.Filter = "CBR Files (*.cbr;*.cbz)|*.cbr;*.cbz|All Files (*.*)|*.*";
            if (win.ShowDialog() == true)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenFileAnsw, win.FileName));
            }
        }

        private void listBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // scroll into view

            if ((!(sender is ListBox)) || (sender == null))
            {
                return;
            }
            var listBox = sender as ListBox;
            if ((listBox.Items == null) || (listBox.Items.Count == 0))
            {
                return;
            }
            //listBox.ScrollIntoView(listBox.Items[0]);
            //listBox.ScrollIntoView(listBox.SelectedItem);
            if (e.AddedItems.Count > 0)
            {
                listBox.ScrollIntoView(e.AddedItems[0]);
                //listBox.Focus();
            }
            //item.Focus();
        }

        // binding is too slow (no first time at window loading)
        private void Rectangle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            string s = e.NewSize.Width.ToString(culture) + "," + e.NewSize.Height.ToString(culture);
            
            //System.Diagnostics.Debug.WriteLine("sw: {0}, sh: {1} ", e.NewSize.Width, e.NewSize.Height);
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ResizeAnsw, s));
        }
    }
}
