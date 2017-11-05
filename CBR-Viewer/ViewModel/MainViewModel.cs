#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System;
using CBR_Viewer.Model;

namespace CBR_Viewer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    [CLSCompliant(false)]
    public partial class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="FileName" /> property's name.
        /// </summary>
        /// 

        public AdornedControl.AdornedControl LoadingAdorner { get; set; }
        private ScaleType LastScale { get; set; }

        public const string FileNamePropertyName = "FileName";
        public const string PageNamePropertyName = "PageName";
        public const string ThumbsPropertyName = "MyBookPages";
        public const string SelIndexPropertyName = "SelIndex";
        public const string PathNamePropertyName = "PathName";
        public const string ColumnVisiblePropertyName = "ColumnVisible";
        public const string CommandsVisiblePropertyName = "CommandsVisible";
        //public const string MenuItemsPropertyName = "MenuItems";
        //public const string ExpandedPropertyName = "Expanded";


        protected CalcCbr cbr;
        protected System.Collections.Generic.List<string> bookPages;
        private string startupPath = "";
        private string _fileName = "";
        private string _pageName = "";
        private string _pathName = "";
        private bool _commandsVisible = false;
        private Size reportedSize;
        private bool _columnVisible = true;

        private int mouseWheelHitCount = 0;
        //private bool _expanded = true;

        //protected int selectedIndex;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Messenger.Default.Register<NotificationMessage>(this, ReplyToMessage);

            InitCommands();
            InitImages();

            this.reportedSize = new Size(900, 600);
            SetScale(this.Settings.Scaling);

            if (!this.Settings.IsUseDynamicCommandBar)
            {
                this.CommandsVisible = true;
            }            
            // events
        }

        //public System.Collections.Generic.List<System.Windows.Controls.MenuItem> MenuItems
        //{
        //    get
        //    {
        //        return MRU.Instance.GetMenuItems();
        //    }
        //}

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                if (_fileName == value)
                {
                    return;
                }

                var oldValue = _fileName;
                _fileName = value;

                RaisePropertyChanged(FileNamePropertyName);
            }
        }

        public bool CommandsVisible
        {
            get
            {
                return this._commandsVisible;
            }

            set
            {
                if (this._commandsVisible == value)
                {
                    return;
                }

                var oldValue = _fileName;
                this._commandsVisible = value;

                RaisePropertyChanged(CommandsVisiblePropertyName);
            }
        }


        public string PathName
        {
            get
            {
                return _pathName;
            }

            set
            {
                if (_pathName == value)
                {
                    return;
                }

                var oldValue = _pathName;
                _pathName = value;
                RaisePropertyChanged(PathNamePropertyName);
            }
        }

        public string PageName
        {
            get
            {
                return this._pageName;
            }
            set
            {
                if (_pageName == value)
                {
                    return;
                }

                var oldValue = _pageName;
                _pageName = value;
                RaisePropertyChanged(PageNamePropertyName);
            }
        }

        public int  SelIndex
        {
            get
            {
                if (this.cbr == null)
                {
                    return -1;
                }
                else
                {
                    return this.cbr.SelectedIndex;
                }
            }

            set
            {
                if (this.cbr == null)
                {
                    return;
                }

                var oldValue = this.cbr.SelectedIndex;
                if (this.cbr.SelectedIndex != value)
                {
                    this.cbr.SelectedIndex = value;
                    // Update bindings, no broadcast
                    SetPageName();
                    SetScale(this.cbr.LastScale);
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollHome, "DoScroll"));
                }
            }
        }


        public int ImageWidth
        {
            get
            {
                if (this.cbr != null)
                {
                    return this.cbr.ImageWidth;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int ImageHeight
        {
            get
            {
                if (this.cbr != null)
                {
                    return this.cbr.ImageHeight;
                }
                else
                {
                    return 0;
                }
            }
        }


        public Rect ImageRect
        {
            get
            {
                if (this.cbr != null)
                {
                    return new Rect(new Size(this.cbr.ImageWidth, this.cbr.ImageHeight));
                }
                return new Rect();
            }
        }

        public bool ColumnVisible
        {
            get
            {
                return _columnVisible;
            }

            set
            {
                if (_columnVisible == value)
                {
                    return;
                }

                var oldValue = _columnVisible;
                _columnVisible = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(ColumnVisiblePropertyName);
            }
        }

        public SettingsWrapper Settings
        {
            get
            {
                return SettingsWrapper.Instance;
            }
        }

        protected string title;
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }

        public System.Collections.Generic.List<BitmapImage> MyBookPages
        {
            get
            {
                System.Collections.Generic.List<BitmapImage> result = new System.Collections.Generic.List<BitmapImage>();
                if (this.cbr != null)
                {
                    foreach (ManageImagesForCBR.ImageNameData nd in this.cbr.BookFullNames)
                    {
                        result.Add(nd.Image);
                    }
                }
                //this.bookPages = result;
                return result;
            }
        }

        public string StartupPath 
        {
            get
            {
                return this.startupPath;
            }
            set
            {
                if (value != this.startupPath)
                {                    
                    this.startupPath = value;
                    //OpenNew(this.startupPath);
                }
            }
        }

        //void MenuClick(object sender, RoutedEventArgs e)
        //{
        //    DoOpenNew((string)(((System.Windows.Controls.MenuItem)sender).CommandParameter));            
        //}




        protected void DoClose()
        {
            if (this.cbr != null)
            {
                if (this.MyBookPages.Count > 0)
                {
                    MRU.Instance.AddItem(this.FileName, this.PathName, this.cbr.SelectedIndex + 1, this.MyBookPages[0]);
                }
                this.cbr.CleanUp();
                this.cbr = null;
                this.FileName = "";
                this.PathName = "";
                SetPageName();
                RaisePropertyChanged(ThumbsPropertyName); 
                RaiseImageChanged();
                MRU.Instance.SaveMRU();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.MenuReload, ""));                
            }
        }

        protected void RecalculateObjectPositions(SizeChangedEventArgs action)
        {
            RecalculateObjectPositions(action.NewSize);
        }

        protected void RecalculateObjectPositions(Size size)
        {
            //return action;
            if ((size.Width != this.reportedSize.Width) || (size.Height != this.reportedSize.Height))
            {
                this.reportedSize = size;
                SetScale(this.LastScale);
                RaisePropertyChanged(ImageRectPropertyName);
            }
            this.Title = "w: " + size.Width.ToString() + " h: " + size.Height.ToString();
            //System.Diagnostics.Debug.WriteLine("w: {0} h: {1}", size.Width, size.Height);
        }

        private void ReplyToMessage(NotificationMessage msg)
        {            
            System.Diagnostics.Debug.WriteLine(msg.Notification);
            if(!(msg.Target is SendType))
            {
                return;
            }

            switch ((SendType)msg.Target)
            {
                case SendType.OpenFileAnsw:
                    {
                        System.Diagnostics.Debug.WriteLine("SendType.OpenFileAnsw");
                        DoOpenNew(msg.Notification, MRU.Instance.GetPageNumberForPath(msg.Notification));
                        break;
                    }
                case SendType.ResizeAnsw:
                    {
                        string[] sl = msg.Notification.Split(new char[] { ',' });
                        if (sl.Length != 2)
                        {
                            this.Title = "Return!! > " + msg.Notification + " length: " + sl.Length.ToString();
                            return;
                        }
                        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

                        double w = Convert.ToDouble(sl[0], culture);
                        double h = Convert.ToDouble(sl[1], culture);
                        RecalculateObjectPositions(new Size(w, h)); break;
                    }
                case SendType.CloseSettings:
                    {
                        MRU.Instance.SetNumberOfRecent(this.Settings.NumberOfRecent);
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.MenuReload, ""));
                        if (this.Settings.IsUseDynamicCommandBar)
                        {
                            this.CommandsVisible = false;
                        }
                        else
                        {
                            this.CommandsVisible = true;
                        }
                        break;
                    }
                case SendType.DoScrollPageDown:
                    {
                        this.NextCommand.Execute(null);
                        break;
                    }
                case SendType.DoScrollPageUp:
                    {
                        this.PrevCommand.Execute(null);
                        break;
                    }
                //case SendType.RequestMRU:
                //    {
                //        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, this., ""));
                //        break;
                //    }
            }
        }

        private void DoOpenNew(string file, int page)
        {
            //this.FileName = "page01.jpg";
            bool b = MRU.Instance.CheckFile(file);
            if (b)
            {
                StartStopWait(true);
                DoClose();
                System.ComponentModel.BackgroundWorker worker = BackgroundLoadPages.CreateBackground(worker_Load, worker_ProgressChanged, worker_RunWorkerCompleted);
                worker.RunWorkerAsync(new WorkArguments(worker, file, this.LastScale, page));
            }
            else
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.MenuReload, ""));
                string[] str = new string[2];
                str[0] = "Could not load, file does not exist";
                str[1] = file;
                ViewModelLocator.MessageStatic.CaptionText = "Could not load, file does not exist";
                ViewModelLocator.MessageStatic.ContentText = file;
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ShowMessage,  ""));
                //MessageBox.Show("Could not Load \n" + file + "\nFile not found");
            }
        }

        private void DoOpenNew(string data)
        {
            string[] s = data.Split(',');
            if (s.Length != 2)
            {
                return;
            }
            DoOpenNew(s[0], Convert.ToInt32(s[1]));
        }

        void worker_Load(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            WorkArguments args = ((WorkArguments)e.Argument);
            CalcCbr cbr = new CalcCbr();
            cbr.Worker = args.Worker;
            cbr.LoadBook(args.File);

            e.Result = new ResultArguments(cbr, args);
        }
        
        void worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            (LoadingAdorner.AdornerContent as CBR_Viewer.Extended.LoadingWait).ReportProgress(e.ProgressPercentage.ToString());
        }

        void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    //  cancelled.
                }
                else if (e.Error != null)
                {
                    // An exception was thrown by the DoWork handler.
                    return;
                }
                else
                {
                    ResultArguments result = (ResultArguments)e.Result;
                    if (result.Cbr.BookFullNames.Count <= 0)
                    {
                        return;
                    }

                    this.cbr = result.Cbr;
                    this.FileName = System.IO.Path.GetFileName(result.WorkArguments.File);
                    this.PathName = System.IO.Path.GetDirectoryName(result.WorkArguments.File);
                    SetSelectedIndex(result.WorkArguments.PageNumber);
                    RaiseSelIndexChanged();
                    //RaiseImageChanged();     
                    RaisePropertyChanged(ThumbsPropertyName);
                    RaisePropertyChanged(ImageRectPropertyName);
                    SetPageName();
                    SetScale(result.WorkArguments.ScaleType);
                }
                if (this.cbr.SelectedIndex >= 0)
                {
                    MRU.Instance.AddItem(this.FileName, this.PathName, this.cbr.SelectedIndex + 1, this.MyBookPages[0]);
                }
            }
            finally
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.LoadFinished, ""));
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.MenuReload, ""));
                StartStopWait(false);
            }
        }

        private void ScrollViewer_MouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("Mouse Wheel {0}", e.Delta);
            if (this.cbr == null)
            {
                return;
            }

            e.Handled = true;

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl)
                || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl))
            {
                // resize
                // fix fome settings a little
                if ((this.cbr.LastScale == ScaleType.ScaleFit) ||
                    (this.cbr.LastScale == ScaleType.ScaleHeight) ||
                    (this.cbr.LastScale == ScaleType.ScaleWidth))
                {
                    if (e.Delta > 0)
                    {
                        this.mouseWheelHitCount++;
                    }
                    else
                    {
                        this.mouseWheelHitCount--;
                    }
                    if (Math.Abs(this.mouseWheelHitCount) < 3)
                    {
                        return;
                    }
                }



                if (e.Delta > 0)
                {
                    SetScale(ScaleType.ScaleUp);
                }
                else
                {
                    SetScale(ScaleType.ScaleDown);
                }
            }
            else
            {
                this.mouseWheelHitCount = 0;

                if (e.Delta > 0)
                {
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollLineUp, "DoScroll"));
                }
                else if (e.Delta < 0)
                {
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollLineDown, "DoScroll"));
                }
            }
        }

        

        private void SetScale(ScaleType type)
        {
            this.LastScale = type;
            if (this.cbr != null)
            {
                switch (type)
                {
                    case ScaleType.ScaleFit:
                        {
                            this.cbr.SetScale(ScaleType.ScaleFit, this.reportedSize);
                            break;
                        }
                    case ScaleType.ScaleWidth:
                        {
                            this.cbr.SetScale(ScaleType.ScaleWidth, this.reportedSize.Width);
                            break;
                        }
                    case ScaleType.ScaleHeight:
                        {
                            this.cbr.SetScale(ScaleType.ScaleHeight, this.reportedSize.Height);
                            break;
                        }
                    case ScaleType.ScaleUp:
                        {
                            this.cbr.SetScale(ScaleType.ScaleUp, null);
                            break;
                        }
                    case ScaleType.ScaleDown:
                        {
                            this.cbr.SetScale(ScaleType.ScaleDown, null);
                            break;
                        }
                    case ScaleType.ScaleOne:
                        {
                            this.cbr.SetScale(ScaleType.ScaleOne, null);
                            break;
                        }
                    default:
                        {
                            this.cbr.SetScale(ScaleType.ScaleUse, null);
                            break;
                        }
                }
            }
            this.mouseWheelHitCount = 0;

            RaisePropertyChanged(ImgFitName);
            RaisePropertyChanged(ImgHorName);
            RaisePropertyChanged(ImgVertName);
            RaisePropertyChanged(ImgStatusName);

            
            RaiseImageChanged();
        }

        public void StartStopWait(bool isVisible)
        {
            LoadingAdorner.IsAdornerVisible = isVisible;
            //LoadingAdorner.S
        }

        private void ListBox_SelectionChanged(System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SetSelectedIndex(this.bookPages.IndexOf((string)e.AddedItems[0]));
            System.Diagnostics.Debug.WriteLine("Selection Changed added: {0} removed: {1}", e.AddedItems.Count.ToString(), e.RemovedItems.Count.ToString());            
        }

        protected int NumberOfDigits(int n)
        {
            if (n == 0)
            {
                return 1;
            }

            return (int)(Math.Log10(Math.Abs(n))) + 1;
        }       

    }

    public class WorkArguments
    {
        public string File { get; protected set; }
        public ScaleType ScaleType { get; protected set; }
        public int PageNumber { get; protected set; }
        public System.ComponentModel.BackgroundWorker Worker { get; protected set; }

        public WorkArguments(System.ComponentModel.BackgroundWorker worker, string file, ScaleType scaleType, int page)
        {
            this.File = file;
            this.ScaleType = scaleType;
            this.Worker = worker;
            if (page > 0)
            {
                this.PageNumber = page - 1;
            }
            else
            {
                this.PageNumber = 0;
            }
        }
    }

    public class ResultArguments
    {
        public CalcCbr Cbr { get; private set; }
        public WorkArguments WorkArguments { get; private set; }
        public ResultArguments(CalcCbr cbr, WorkArguments args)
        {
            this.Cbr = cbr;
            this.WorkArguments = args;
        }
    }
}