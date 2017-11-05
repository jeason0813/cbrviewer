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

namespace ManageImagesForCBR
{
    public class ImageNameData : IComparable<ImageNameData>
    {
        public string FullName { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public double Ratio { get; private set; }
        public BitmapImage Image { get; private set; }

        public ImageNameData(string name, string path)
        {
            this.FullName = name;
            this.Name = new System.IO.FileInfo(name).Name;
            this.Path = path;
            this.Image = Use7Zip.GetImageFromStream(path, name);
            SetWidthHeight(this.Image.PixelWidth, this.Image.PixelHeight);
            //((BitmapImage)this.Image).PixelWidth;
            System.Diagnostics.Debug.WriteLine("ImageNameData " + this.Name + " : w: " + this.Image.PixelWidth.ToString() + " : Full: " + this.FullName);
        }

        protected void SetWidthHeight(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Ratio = (double)width / (double)height;
        }


        public int CompareTo(ImageNameData obj)
        {
            return this.Name.CompareTo(((ImageNameData)obj).Name);
        }
    }

}
