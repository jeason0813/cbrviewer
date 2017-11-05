#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls;
using ManageImagesForCBR;

namespace CBR_Viewer.Model
{
    public enum ScaleType
    {
        ScaleUp,
        ScaleDown,
        ScaleFit,
        ScaleWidth,
        ScaleHeight,
        ScaleOne,
        ScaleUse,
    }

    public class CalcCbr 
    {

        const double SmallestScale = 0.05;
        const double LargestScale = 3.0;

        protected string dir;
        protected List<ImageNameData> imageDataList;
        protected int selIndex;
        protected double scale;
        protected ScaleType finalScale;

        protected ImageExstractor imageExtractor;

        public System.ComponentModel.BackgroundWorker Worker { get; set; }

        public CalcCbr()
        {
            this.imageExtractor = new ImageExstractor();
            this.imageExtractor.ImageNameDataAdded += new ImageNameDataAddedEventHandler(ie_ImageNameDataAdded);
            Reset();
        }

        ~CalcCbr()
        {
            if (this.imageDataList != null)
            {
                foreach (ImageNameData ind in this.imageDataList)
                {
                    ind.Image.StreamSource.Close();
                }
            }
        }

        public void LoadBook(string cbrFile)
        {
            if (!System.IO.File.Exists(cbrFile))
            {
                return;
            }
            //this.imageDataList = Use7Zip.LoadBook(cbrFile);

            this.imageDataList = this.imageExtractor.LoadBook(cbrFile);
        }

        void ie_ImageNameDataAdded(object sender, ImageNameDataAddedEventArgs e)
        {
            if (this.Worker != null)
            {
                this.Worker.ReportProgress(e.NumberAdded, e.DataDone);
            }
        }

        public List<ImageNameData> BookFullNames
        {
            get
            {
                return this.imageDataList;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selIndex;
            }
            set
            {
                if ((this.imageDataList == null) || (this.imageDataList.Count == 0))
                {
                    this.selIndex = - 1;
                }

                if ((this.selIndex != value) && (this.selIndex >= 0) && (this.selIndex < this.imageDataList.Count))
                {
                    this.selIndex = value;
                }
            }
        }

        private void Reset()
        {
            this.scale = 1.0;
            this.finalScale = ScaleType.ScaleOne;
        }        

        public void CleanUp()
        {
            System.Diagnostics.Debug.WriteLine("CleanUp");
        }

        public BitmapImage Image
        {
            get
            {
                if (this.selIndex >= 0)
                {
                    return this.BookFullNames[this.selIndex].Image;
                }
                return null;
            }
        }

        public int ImageWidth
        {
            get
            {
                if (this.selIndex >= 0)
                {
                    return (int)(this.BookFullNames[this.selIndex].Width * this.scale + 0.5);
                }
                return 0;
            }
        }

        public int ImageHeight
        {
            get
            {
                if (this.selIndex >= 0)
                {
                    return (int)(this.BookFullNames[this.selIndex].Height * this.scale + 0.5);
                }
                return 0;
            }
        }

        public ScaleType LastScale
        {
            get
            {
                return this.finalScale;
            }
        }

        public void SetScale(ScaleType st, object param)
        {
            if (this.selIndex < 0)
            {
                return;
            }

            switch (st)
            {
                case ScaleType.ScaleDown:
                    {
                        if (this.scale > SmallestScale)
                        {
                            this.scale -= (0.1 * this.scale);
                        }
                        break;
                    }
                case ScaleType.ScaleFit:
                    {
                        if (param is System.Windows.Size)
                        {
                            System.Windows.Size size = (System.Windows.Size)param;
                            double ratio = size.Width / size.Height;
                            if (this.BookFullNames[this.selIndex].Ratio > ratio)
                            {
                                SetScale(ScaleType.ScaleWidth, size.Width);
                            }
                            else
                            {
                                SetScale(ScaleType.ScaleHeight, size.Height);
                            }
                        }
                        break;
                    }
                case ScaleType.ScaleHeight:
                    {
                        if ((param is int) || (param is double))
                        {
                            int h = Convert.ToInt32(param); // new 
                            h -= 32;
                            int hi = this.BookFullNames[this.selIndex].Height; // org
                            this.scale = (double)h / (double)hi;
                        }
                        break;
                    }
                case ScaleType.ScaleUp:
                    {
                        if (this.scale < LargestScale)
                        {
                            this.scale += (0.1 * this.scale);
                        }
                        break;
                    }
                case ScaleType.ScaleWidth:
                    {
                        if ((param is int) || (param is double))
                        {
                            int w = Convert.ToInt32(param);
                            w -= 32;
                            int wi = this.BookFullNames[this.selIndex].Width; // org
                            this.scale = (double)w / (double)wi;
                        }
                        break;
                    }
                case ScaleType.ScaleOne:
                    {
                        this.scale = 1.0;
                        break;
                    }
            }
            // save what was done
            if ((st != ScaleType.ScaleUp) && (st != ScaleType.ScaleDown))
            {
                this.finalScale = st;
            }
            else
            {
                this.finalScale = ScaleType.ScaleUse;
            }
        }
    }
}
