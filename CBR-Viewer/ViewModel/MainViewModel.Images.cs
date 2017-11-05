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
using System.Windows.Media.Imaging;
using CBR_Viewer.Model;
using GalaSoft.MvvmLight.Messaging;

namespace CBR_Viewer.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        public const string ImagePropertyName = "Image";
        public const string ImageWidthPropertyName = "ImageWidth";
        public const string ImageHeightPropertyName = "ImageHeight";
        public const string ImageRectPropertyName = "ImageRect";

        public const string ImgFitName = "ImgFit";
        public const string ImgHorName = "ImgHor";
        public const string ImgVertName = "ImgVert";
        public const string ImgStatusName = "ImgStatusBar";

        public BitmapImage ImgOpen { get; private set; }
        public BitmapImage ImgRecent { get; private set; }
        public BitmapImage ImgClose { get; private set; }
        public BitmapImage ImgAbout { get; private set; }
        public BitmapImage ImgSettings { get; private set; }
        protected BitmapImage ImgFitN { get; private set; }
        protected BitmapImage ImgHorN { get; private set; }
        protected BitmapImage ImgVertN { get; private set; }
        protected BitmapImage ImgFitOk { get; private set; }
        protected BitmapImage ImgHorOk { get; private set; }
        protected BitmapImage ImgVertOk { get; private set; }
        public BitmapImage ImgFirst { get; private set; }
        public BitmapImage ImgPrev { get; private set; }
        public BitmapImage ImgNext { get; private set; }
        public BitmapImage ImgLast { get; private set; }

        public BitmapImage ImgStatus { get; private set; }
        public BitmapImage ImgStatusFit { get; private set; }
        public BitmapImage ImgStatusHeight { get; private set; }
        public BitmapImage ImgStatusWidth { get; private set; }

        public void InitImages()
        {
            // images
            this.ImgOpen = MakeBitmap("/CBReader;component/pics/Open_01_48.png");
            this.ImgRecent = MakeBitmap("/CBReader;component/pics/Recent_48_01.png");
            this.ImgClose = MakeBitmap("/CBReader;component/pics/Close_01_48.png");
            this.ImgAbout = MakeBitmap("/CBReader;component/pics/About_01_48.png");
            this.ImgSettings = MakeBitmap("/CBReader;component/pics/Settings_01_48.png");
            this.ImgFitN = MakeBitmap("/CBReader;component/pics/Fit_48_01.png");
            this.ImgHorN = MakeBitmap("/CBReader;component/pics/Width_48_01.png");
            this.ImgVertN = MakeBitmap("/CBReader;component/pics/Height_48_01.png");
            this.ImgFitOk = MakeBitmap("/CBReader;component/pics/FitOk_48_01.png");
            this.ImgHorOk = MakeBitmap("/CBReader;component/pics/WidthOk_48_01.png");
            this.ImgVertOk = MakeBitmap("/CBReader;component/pics/HeightOk_48_01.png");
            this.ImgFirst = MakeBitmap("/CBReader;component/pics/First_48_01.png");
            this.ImgPrev = MakeBitmap("/CBReader;component/pics/Prev_48_01.png");
            this.ImgNext = MakeBitmap("/CBReader;component/pics/Next_48_01.png");
            this.ImgLast = MakeBitmap("/CBReader;component/pics/Last_48_01.png");
            this.ImgStatus = MakeBitmap("/CBReader;component/pics/16/CBR 016.png");
            this.ImgStatusFit = MakeBitmap("/CBReader;component/pics/16/Fit_16_01.png");
            this.ImgStatusHeight = MakeBitmap("/CBReader;component/pics/16/Height_16_01.png");
            this.ImgStatusWidth = MakeBitmap("/CBReader;component/pics/16/Width_16_01.png");
        }

        public BitmapImage Image
        {
            get
            {
                if (this.cbr != null)
                {
                    return this.cbr.Image;
                }
                else
                {
                    return null;
                }
            }
        }

        public BitmapImage ImgFit
        {
            get
            {
                if (((this.cbr != null) && (this.cbr.LastScale != ScaleType.ScaleFit)) ||
                    (this.LastScale != ScaleType.ScaleFit))
                {
                    return this.ImgFitN;
                }
                else
                {
                    return this.ImgFitOk;
                }
            }
        }

        public BitmapImage ImgHor
        {
            get
            {
                if (((this.cbr != null) && (this.cbr.LastScale != ScaleType.ScaleWidth)) ||
                    (this.LastScale != ScaleType.ScaleWidth))
                {
                    return this.ImgHorN;
                }
                else
                {
                    return this.ImgHorOk;
                }
            }
        }

        public BitmapImage ImgVert
        {
            get
            {
                if (((this.cbr != null) && (this.cbr.LastScale != ScaleType.ScaleHeight)) ||
                    (this.LastScale != ScaleType.ScaleHeight))
                {
                    return this.ImgVertN;
                }
                else
                {
                    return this.ImgVertOk;
                }
            }
        }

        public BitmapImage ImgStatusBar
        {
            get
            {
                ScaleType st = this.LastScale;
                if (this.cbr != null)
                {
                    st = this.cbr.LastScale;
                }

                switch (st)
                {
                    case ScaleType.ScaleFit:
                        {
                            return this.ImgStatusFit;
                        }
                    case ScaleType.ScaleHeight:
                        {
                            return this.ImgStatusHeight;
                        }
                    case ScaleType.ScaleWidth:
                        {
                            return this.ImgStatusWidth;
                        }
                    default:
                        {
                            return this.ImgStatus;
                        }
                }
            }
        }

        private BitmapImage MakeBitmap(string resource)
        {
            BitmapImage newImage = new BitmapImage();
            newImage.BeginInit();
            newImage.UriSource = new System.Uri(resource, System.UriKind.RelativeOrAbsolute);
            newImage.EndInit();
            return newImage;
        }

        //private void RaiseImageChanged()
        //{
        //    RaiseImageChanged(false);
        //    RaiseImageChanged(true);
        //}

        //private void RaiseImageChanged(bool t)
        //{
        //    if (t)
        //    {
        //        RaisePropertyChanged(ThumbsPropertyName);
        //        RaisePropertyChanged(SelIndexPropertyName);
        //        RaisePropertyChanged(ImageRectPropertyName);
        //    }
        //    else
        //    {
        //        RaisePropertyChanged(ImagePropertyName);
        //        RaisePropertyChanged(ImageWidthPropertyName);
        //        RaisePropertyChanged(ImageHeightPropertyName);
        //        RaisePropertyChanged(ImgFitName);
        //        RaisePropertyChanged(ImgHorName);
        //        RaisePropertyChanged(ImgVertName);
        //        RaisePropertyChanged(ImgStatusName);
        //        //RaisePropertyChanged(MenuItemsPropertyName);
        //    }
        //}

        private void RaiseSelIndexChanged()
        {
            RaisePropertyChanged(SelIndexPropertyName);
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, SendType.DoScrollHome, "DoScroll"));
        }

        private void RaiseImageChanged()
        {
            RaisePropertyChanged(ImagePropertyName);
            RaisePropertyChanged(ImageWidthPropertyName);
            RaisePropertyChanged(ImageHeightPropertyName);
            RaisePropertyChanged(ImageRectPropertyName);
        }
    }
}
