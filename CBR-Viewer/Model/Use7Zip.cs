#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenZip;
using System.IO;
using System.Windows.Media.Imaging;

namespace ManageImagesForCBR
{
    public static class Use7Zip 
    {
        public static System.Collections.Generic.List<string> ImageExtensions = new System.Collections.Generic.List<string>() { ".png", ".bmp", ".jpg", ".gif", ".tif", ".tiff" };

        public static void SetSevenZipDllPath()
        {
            string p;
            if (Environment.Is64BitOperatingSystem && Environment.Is64BitProcess)
            {
                p = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "7z64.dll");
            }
            else
            {
                p = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "7z.dll");
            }

            SevenZipExtractor.SetLibraryPath(p);
        }

        public static List<ImageNameData> LoadBook(string path)
        {
            ImageExstractor ie = new ImageExstractor();
            return ie.LoadBook(path);
        }

        public static BitmapImage GetImageFromStream(string zipFilePath, string fileName)
        {
            SevenZipExtractor extractor = null;
             
            BitmapImage result = null;
            MemoryStream stream = null;
            try
            {
                extractor = new SevenZipExtractor(zipFilePath);

                stream = new MemoryStream();
                extractor.ExtractFile(fileName, stream);

                result = GetImageFromStream(stream);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
            }
            finally
            {
                extractor.Dispose();
                extractor = null;
                if (stream != null)
                {
                    stream.Close();
                }
                stream = null;
            }
            return result;
        }

        static private BitmapImage GetImageFromStream(MemoryStream stream)
        {
            MemoryStream imStream = new MemoryStream();
            BitmapImage myImage = null;
            try
            {
                stream.WriteTo(imStream);
                stream.Flush();
                stream.Close();
                imStream.Position = 0;

                myImage = new BitmapImage();
                myImage.BeginInit();
                myImage.StreamSource = imStream;
                myImage.EndInit();
                myImage.Freeze();
            }
            catch
            {
                imStream.Dispose();
            }
            finally
            {
                imStream.Flush();
            }
            return myImage;
        }
    }

    public class ImageExstractor
    {
        public event ImageNameDataAddedEventHandler ImageNameDataAdded;
        
        public ImageExstractor()
        { 
        }

        public List<ImageNameData> LoadBook(string path)
        {
            List<ImageNameData> result = new List<ImageNameData>();

            SevenZipExtractor extractor = null;
            try
            {

                Use7Zip.SetSevenZipDllPath();

                extractor = new SevenZipExtractor(path);
                long size = extractor.PackedSize;
                foreach (ArchiveFileInfo fil in extractor.ArchiveFileData)
                {
                    if (!fil.IsDirectory)
                    {
                        if (Use7Zip.ImageExtensions.Contains(Path.GetExtension(fil.FileName).ToLower()))
                        {
                            ImageNameData data = new ImageNameData(fil.FileName, path);
                            result.Add(data);
                            if (this.ImageNameDataAdded != null)
                            {
                                this.ImageNameDataAdded(this, new ImageNameDataAddedEventArgs(data, result.Count));
                            }
                        }
                    }
                }

                result.Sort();
                //foreach (ImageNameData id in result)
                //{
                //    System.Diagnostics.Debug.WriteLine("sorted: " + id.Name);
                //}
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
            }
            finally
            {
                if (extractor != null)
                {
                    extractor.Dispose();
                    extractor = null;
                }
            }
            return result;
        }

    }

    public delegate void ImageNameDataAddedEventHandler(object sender, ImageNameDataAddedEventArgs e);
    [System.ComponentModel.ImmutableObject(true)]
    public class ImageNameDataAddedEventArgs : EventArgs
    {
        public ImageNameData DataDone { get; private set; }
        public int NumberAdded { get; private set; }

        public ImageNameDataAddedEventArgs(ImageNameData current, int count)
        {
            this.DataDone = current;
            this.NumberAdded = count;
        }
    }


 }
