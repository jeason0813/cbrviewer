#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using CBR_Viewer.Model;

namespace CBR_Viewer.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        public RelayCommand MouseEnterCommand { get; private set; }
        public RelayCommand MouseLeaveCommand { get; private set; }

        public RelayCommand ResetCommand { get; private set; }
        public RelayCommand SimpleCommand { get; private set; }
        public RelayCommand RecentCommand { get; private set; }
        public RelayCommand RecentlyUsedCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }

        public RelayCommand FirstCommand { get; private set; }
        public RelayCommand PrevCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand LastCommand { get; private set; }

        public RelayCommand FitCommand { get; private set; }
        public RelayCommand FitHorCommand { get; private set; }
        public RelayCommand FitVertCommand { get; private set; }


        public RelayCommand<System.Windows.Input.MouseWheelEventArgs> MouseWheelCommand { get; private set; }
        public RelayCommand<System.Windows.Controls.SelectionChangedEventArgs> SelectionChangedCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> SizeChangedCommand { get; private set; }
        public RelayCommand<System.Windows.Input.KeyEventArgs> KeyDownCommand { get; private set; }
        public RelayCommand Window_ClosedCommand { get; private set; }
        public RelayCommand Window_LoadedCommand { get; private set; }


        public void InitCommands()
        {
            // commands
            this.MouseEnterCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("Enter!");
                if (this.Settings.IsUseDynamicCommandBar)
                {
                    this.CommandsVisible = true;
                }
            });

            this.MouseLeaveCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("Leave!");
                if (this.Settings.IsUseDynamicCommandBar)
                {
                    this.CommandsVisible = false;
                }
            });

            this.ResetCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("Reset!");
            });

            this.SimpleCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("Simple!");
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenFileReq, "Open File"));
            });

            this.RecentCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenPopup, "Open Recent"));
            });

            this.RecentlyUsedCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("MRU!");
                //System.Windows.Controls.ContextMenu cm = MRUItem.GetContextMenu(MyBookPages[0], 15);

            });

            this.CloseCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("Close!");
                DoClose();
            });

            this.SettingsCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("Settings!");
                //this.ColumnVisible = !this._columnVisible;
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenSettingsReq, "Open Settings"));
            });

            this.AboutCommand = new RelayCommand(() =>
            {
                //System.Diagnostics.Debug.WriteLine("About!");
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.OpenAboutReq, "Open About"));
            });

            // navigate
            this.FirstCommand = new RelayCommand(() =>
            {
                SetSelectedIndex(0);
                RaiseSelIndexChanged();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ResetScroll, ""));
                //System.Diagnostics.Debug.WriteLine("First!");
            });
            this.PrevCommand = new RelayCommand(() =>
            {
                SetSelectedIndex(this.SelIndex - 1);
                RaiseSelIndexChanged();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ResetScroll, ""));
                //System.Diagnostics.Debug.WriteLine("Prev!");
            });
            this.NextCommand = new RelayCommand(() =>
            {
                SetSelectedIndex(this.SelIndex + 1);
                RaiseSelIndexChanged();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ResetScroll, ""));
                //System.Diagnostics.Debug.WriteLine("Next!");
            });
            this.LastCommand = new RelayCommand(() =>
            {
                // too high page-number will be set to last
                SetSelectedIndex(999999);
                RaiseSelIndexChanged();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.ResetScroll, ""));
                //System.Diagnostics.Debug.WriteLine("Last!");
            });

            //scaling
            this.FitCommand = new RelayCommand(() =>
            {
                if (this.LastScale != ScaleType.ScaleFit)
                {
                    SetScale(ScaleType.ScaleFit);
                }
                else
                {
                    SetScale(ScaleType.ScaleOne);
                }
                //System.Diagnostics.Debug.WriteLine("Last!");
            });
            this.FitHorCommand = new RelayCommand(() =>
            {
                if (this.LastScale != ScaleType.ScaleWidth)
                {
                    SetScale(ScaleType.ScaleWidth);
                }
                else
                {
                    SetScale(ScaleType.ScaleOne);
                }
                //System.Diagnostics.Debug.WriteLine("Last!");
            });
            this.FitVertCommand = new RelayCommand(() =>
            {
                if (this.LastScale != ScaleType.ScaleHeight)
                {
                    SetScale(ScaleType.ScaleHeight);
                }
                else
                {
                    SetScale(ScaleType.ScaleOne);
                }
                //System.Diagnostics.Debug.WriteLine("Last!");
            });
            // events
            this.MouseWheelCommand = new RelayCommand<System.Windows.Input.MouseWheelEventArgs>((args) =>
            {
                ScrollViewer_MouseWheel(args);
            });

            this.SelectionChangedCommand = new RelayCommand<System.Windows.Controls.SelectionChangedEventArgs>((args) =>
            {
                ListBox_SelectionChanged(args);
            });

            this.KeyDownCommand = new RelayCommand<System.Windows.Input.KeyEventArgs>((args) =>
            {
                Window_KeyDown(args);
            });

            this.Window_ClosedCommand = new RelayCommand(() =>
            {
                DoClose();
                this.Settings.Scaling = this.LastScale;
                this.Settings.SaveSettings();
            });

            this.Window_LoadedCommand = new RelayCommand(() =>
            {
                //MRU.Instance.MenuClick = MenuClick;
                // load menu
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.MenuReload, ""));
                // start with loaded book ...
                if (!String.IsNullOrEmpty(this.startupPath))
                {
                    DoOpenNew(this.startupPath, MRU.Instance.GetPageNumberForPath(this.startupPath));
                }
            });

            this.SizeChangedCommand = new RelayCommand<SizeChangedEventArgs>(action =>
            {
                RecalculateObjectPositions(action);
            });
        }

        private void Window_KeyDown(System.Windows.Input.KeyEventArgs e)
        {            
            System.Diagnostics.Debug.WriteLine("Pre-Key {0}", e.Key);

            if (e.Source is System.Windows.Controls.ListBox)
            {
                return;
            }

            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollLineUp, "DoScroll"));
                        break;
                    }
                case System.Windows.Input.Key.PageUp:
                    {
                        this.PrevCommand.Execute(null);
                        break;
                    }
                case System.Windows.Input.Key.PageDown:
                    {
                        this.NextCommand.Execute(null);
                        break;
                    }
                case System.Windows.Input.Key.Down:
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollLineDown, "DoScroll"));
                        break;
                    }
                case System.Windows.Input.Key.Home:
                    {
                        this.FirstCommand.Execute(null);
                        break;
                    }
                case System.Windows.Input.Key.End:
                    {
                        this.LastCommand.Execute(null);
                        break;
                    }
                case System.Windows.Input.Key.Add:
                    {
                        SetScale(ScaleType.ScaleUp);
                        break;
                    }
                case System.Windows.Input.Key.Subtract:
                    {
                        SetScale(ScaleType.ScaleDown);
                        break;
                    }
                case System.Windows.Input.Key.D0:
                case System.Windows.Input.Key.D1:
                case System.Windows.Input.Key.D2:
                case System.Windows.Input.Key.D3:
                case System.Windows.Input.Key.D4:
                case System.Windows.Input.Key.D5:
                case System.Windows.Input.Key.D6:
                case System.Windows.Input.Key.D7:
                case System.Windows.Input.Key.D8:
                case System.Windows.Input.Key.D9:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {

                            if (this.Settings.NumberOfRecent > 0)
                            {
                                string s = MRU.Instance.GetFileNameAndPageForKey(e.Key);
                                if (!String.IsNullOrEmpty(s))
                                {
                                    DoOpenNew(s);
                                }
                            }
                        }
                        break;
                    }
                case System.Windows.Input.Key.O:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.SimpleCommand.Execute(null);
                        }
                        break;
                    }
                case System.Windows.Input.Key.C:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.CloseCommand.Execute(null);
                        }
                        break;
                    }
                case System.Windows.Input.Key.F:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.FitCommand.Execute(null);
                        }
                        break;
                    }
                case System.Windows.Input.Key.H:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.FitHorCommand.Execute(null);
                        }
                        break;
                    }
                case System.Windows.Input.Key.V:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.FitVertCommand.Execute(null);
                        }
                        break;
                    }
                case System.Windows.Input.Key.A:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.AboutCommand.Execute(null);
                        }
                        break;
                    }
                case System.Windows.Input.Key.S:
                    {
                        if (IsCtrl(e.KeyboardDevice))
                        {
                            this.SettingsCommand.Execute(null);
                        }
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            e.Handled = true;
            //System.Diagnostics.Debug.WriteLine("Post-Key {0}", e.Key);
        }

        private bool IsCtrl(System.Windows.Input.KeyboardDevice keyboardDevice)
        {
            if ((keyboardDevice.IsKeyDown(System.Windows.Input.Key.LeftCtrl)) ||
               (keyboardDevice.IsKeyDown(System.Windows.Input.Key.RightCtrl)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetSelectedIndex(int newIndex)
        {
            int i = newIndex;
            if ((this.cbr == null))
            {
                i = -1;
            }
            else if (i >= this.cbr.BookFullNames.Count)
            {
                i = this.cbr.BookFullNames.Count - 1;
            }
            else if (i < 0)
            {
                i = 0;
            }
            //if (i != this.SelIndex)
            //{
            this.SelIndex = i;
            SetPageName();
            //RaisePropertyChanged(ImagePropertyName);
            //}
        }

        protected void SetPageName()
        {
            if (this.cbr == null)
            {
                this.PageName = "";
                return;
            }

            int i = NumberOfDigits(this.cbr.BookFullNames.Count);
            string s = (this.cbr.SelectedIndex + 1).ToString();
            while (s.Length < i)
            {
                s = '0' + s;
            }
            s = s + " - " + this.cbr.BookFullNames.Count;
            this.PageName = s;
        }

    }
}
