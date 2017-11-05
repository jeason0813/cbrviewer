#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBR_Viewer.Model
{
    public static class Test_MRUList
    {
        public static MRUList FilledTestList()
        {
            MRUList list = new MRUList();
            MRUItem item = new MRUItem();
            item.Path = @"C:\Data\";
            item.FileName = @"000";
            item.PageNumber = 100;
            list.AddMRUItem(item);
            item = new MRUItem();
            item.Path = @"C:\Data";
            item.FileName = @"001";
            item.PageNumber = 1;
            list.AddMRUItem(item);
            item = new MRUItem();
            item.Path = @"C:\Data\";
            item.FileName = @"002";
            item.PageNumber = 2;
            list.AddMRUItem(item);
            item = new MRUItem();
            item.Path = @"C:\Data";
            item.FileName = @"003";
            item.PageNumber = 3;
            list.AddMRUItem(item);
            item = new MRUItem();
            item.Path = @"C:\Data";
            item.FileName = @"000";
            item.PageNumber = 0;
            list.AddMRUItem(item); 
            return list;
        }

        public static void PrintList(MRUList list)
        {
            System.Diagnostics.Debug.WriteLine("*** Begin PrintList foreach ***");
            foreach (MRUItem item in list)
            {
                System.Diagnostics.Debug.WriteLine(item.FullFileName);
            }
            System.Diagnostics.Debug.WriteLine("*** End PrintList foreach ***");
            System.Diagnostics.Debug.WriteLine("*** Begin PrintList for ***");
            for (int i=0; i < list.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(list[i].FullFileName);
            }
            System.Diagnostics.Debug.WriteLine("*** End PrintList for ***");
        }

        public static void CheckPageNumber(MRUList list, string numbers)
        {
            System.Diagnostics.Debug.WriteLine("-c-");
            
            string[] nr = numbers.Split(',');
            if (list.Count != nr.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = 0; i < list.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(list[i].FullFileName + " :: " + list[i].PageNumber.ToString());

                if (list[i].PageNumber.ToString() != nr[i])
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static void Test()
        {
            System.Diagnostics.Debug.WriteLine("*** Test ***");
            MRUList list = FilledTestList();
            CheckPageNumber(list, "0,3,2,1");
            InsertTest();
            IndexOfTest();
            MaxNumberTest();
            System.Diagnostics.Debug.WriteLine("*** End ***");
        }

        public static void InsertTest()
        {
            System.Diagnostics.Debug.WriteLine("*** InsertTest ***");
            MRUList list = FilledTestList();
            MRUItem item = new MRUItem();
            item.Path = @"C:\Data";
            item.FileName = "001";
            item.PageNumber = 101;
            list.Insert(3, item);
            CheckPageNumber(list, "0,3,2,101");
            list = FilledTestList();
            list.Insert(0, item);
            CheckPageNumber(list, "101,0,3,2");
            list = FilledTestList();
            list.Insert(1, item);
            CheckPageNumber(list, "0,101,3,2");
            list = FilledTestList();
            item.Path = @"C:\Data";
            item.FileName = "004";
            item.PageNumber = 104;
            list.Insert(1, item);
            CheckPageNumber(list, "0,104,3,2,1");
            list = FilledTestList();
            item.FileName = "002";
            item.PageNumber = 102;
            list.AddMRUItem(item);
            CheckPageNumber(list, "102,0,3,1");

            System.Diagnostics.Debug.WriteLine("*** End InsertTest ***");
        }

        public static void IndexOfTest()
        {
            System.Diagnostics.Debug.WriteLine("*** IndexOfTest ***");
            // "0,3,2,1"
            MRUList list = FilledTestList();
            MRUItem item = new MRUItem();
            item.Path = @"C:\Data";
            item.FileName = "001";
            item.PageNumber = 101;
            if (list.IndexOf(item) != 3)
            {
                throw new ArgumentOutOfRangeException();
            }
            item.FileName = "000";
            if (list.IndexOf(item) != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            item.FileName = "OhGreat";
            if (list.Contains(item))
            {
                throw new ArgumentOutOfRangeException();
            }
            item.FileName = "003";
            if (!list.Contains(item))
            {
                throw new ArgumentOutOfRangeException();
            }


            System.Diagnostics.Debug.WriteLine("*** End IndexOfTest ***");
        }

        public static void MaxNumberTest()
        {
            MRUList list = new MRUList();

            for (int i = 0; i < 11; i++)
            {
                MRUItem item = new MRUItem();
                item.Path = @"D:\Data\";
                item.FileName = i.ToString("D3");
                item.PageNumber = i;
                list.AddMRUItem(item);
            }

            CheckPageNumber(list, "10,9,8,7,6,5,4,3,2,1");
        }

        public static void SerializationTest(System.Windows.Controls.Image image)
        {
            System.Diagnostics.Debug.WriteLine("*** SerializationTest ***");
            // "0,3,2,1"
            MRUItem item = new MRUItem();
            item.Path = @"C:\Data";
            item.FileName = "001";
            item.PageNumber = 101;
            //item.CoverImage = image;
            item.Thumb = (System.Windows.Media.Imaging.RenderTargetBitmap)image.Source;
            MRUItem.Serialize(@"D:\MRUItemXml.xml", item);

            MRUItem item2 = MRUItem.DeSerialize(@"D:\MRUItemXml.xml");
            if ((item.Path != item2.Path) || (item.FileName != item2.FileName))
            {
                throw new ArgumentOutOfRangeException();
            }
            MRUList list = FilledTestList();
            CheckPageNumber(list, "0,3,2,1");
            MRUList.Serialize(@"D:\MRUListXml.xml", list);
            MRUList list2 = MRUList.DeSerialize(@"D:\MRUListXml.xml");
            CheckPageNumber(list2, "0,3,2,1");
            System.Diagnostics.Debug.WriteLine("*** End SerializationTest ***");
        }
    }
}
