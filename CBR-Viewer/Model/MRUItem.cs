#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.Serialization;

namespace CBR_Viewer.Model
{
    [XmlRootAttribute("MRUItem")]
    public class MRUItem 
    {
        const int ImageBaseSize = 14;
        public string FileName { get; set; }
        public int PageNumber { get; set; }
        public string Path { get; set; }
        //[XmlIgnore]
        //public Image CoverImage { get; set; }
        [XmlIgnore]
        public BitmapSource Thumb {get; set;}

        public MRUItem()
        {
 
        }

        protected byte[] GetImageByteArray()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(this.Thumb));
                encoder.Save(stream);

                //SerializedImage = stream.ToArray();
                return stream.ToArray();
            }
        }


        protected void SetImageByteArray(byte[] val)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream(val);
            this.Thumb = BitmapFrame.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            stream.Close();
        }

        //[XmlIgnore]
        //public string Index
        //{
        //    get
        //    {
        //        return IndexOf(item).ToString() + " - ";
        //    }
        //}

        [XmlElement("Thumb")]
        public string ThumbAsString
        {
            get
            {
                return Convert.ToBase64String(GetImageByteArray()); 
            }
            set
            {
                SetImageByteArray(Convert.FromBase64String(value));
            }
        }

        string ImageToBase64(BitmapSource bitmap)
        {
            var encoder = new PngBitmapEncoder();
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = new System.IO.MemoryStream())
            {
                encoder.Save(stream);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        BitmapSource Base64ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream);
            }
        }

        [XmlIgnore]
        public Image ThumbImage
        {
            get
            {
                if (this.Thumb == null)
                {
                    return null;
                }
                Image im = new Image();
                im.Width = this.Thumb.Width;
                im.Height = this.Thumb.Height;
                im.Stretch = Stretch.None;
                im.Source = this.Thumb;
                //im.Source = org;
                return im;
            }           
        }

        [XmlIgnore]
        public string FullFileName
        {
            get
            {
                return GetFullFileName(this.Path, this.FileName);
            }
        }

        [XmlIgnore]
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(FileName);
            }
        }

        [XmlIgnore]
        public string PageNumberBrc
        {
            get
            {
                return "[" + this.PageNumber + "]";
            }
        }
        public static MRUItem Create(string file, string path, int page, BitmapImage orgImage)
        {
            MRUItem result = new MRUItem();
            result.FileName = file;
            result.Path = path;
            result.PageNumber = page;
            result.Thumb = CreateImageFromSource(orgImage, MRUItem.ImageBaseSize);
            return result;
        }

        public static string GetFullFileName(string path, string fileName)
        {
            return System.IO.Path.Combine(path, fileName);
        }

        public static RenderTargetBitmap CreateImageFromSource(BitmapImage org, int size)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(size * 3, size * 4, 96, 96, PixelFormats.Pbgra32);
            ImageBrush ib = CreateBrush(org);
            DrawImage(bmp, ib, 0, 0, size * 3, size * 4);
            return bmp;            
        }

        public static ImageBrush CreateBrush(BitmapImage image)
        {
            ImageBrush ib = new ImageBrush();
            ib.Stretch = Stretch.UniformToFill;
            ib.ImageSource = image;
            ib.Viewport = new System.Windows.Rect(0, 0, 1.0, 1.0);
            //grab image portion
            //ib.Viewbox = new System.Windows.Rect(0, 0, image.PixelWidth, image.PixelHeight);
            ib.Viewbox = new System.Windows.Rect(0, 0, image.Width, image.Height);
            ib.ViewboxUnits = BrushMappingMode.Absolute;
            ib.TileMode = TileMode.None;

            return ib;
        }

        public static void DrawImage(RenderTargetBitmap rtb, ImageBrush brush, int x, int y, int hSize, int vSize)
        {
            ImageBrush ib = brush;
            // Create a DrawingVisual objects.
            DrawingVisual drawvis = new DrawingVisual();
            DrawingContext dc = drawvis.RenderOpen();
            dc.DrawRectangle(ib, null, new System.Windows.Rect(x, y, hSize, vSize));
            dc.Close();

            // Render the DrawingVisual on the RenderTargetBitmap.
            rtb.Render(drawvis);
        }

        public static void SaveImage(RenderTargetBitmap rtb, string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            // BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(fs);
            fs.Close();
        }

        static public void Serialize(string path, MRUItem details)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MRUItem));
            using (System.IO.TextWriter writer = new System.IO.StreamWriter(path))
            {
                serializer.Serialize(writer, details);
            }
        }

        static public MRUItem DeSerialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MRUItem));
            using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(path))
            {
                return (MRUItem)serializer.Deserialize(reader);
            }
        }
    }
}
