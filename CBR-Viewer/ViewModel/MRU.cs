#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CBR_Viewer.Model;

namespace CBR_Viewer.ViewModel
{
    public class MRU
    {
        private static readonly MRU _instance = new MRU();

        protected MRUList list;

        protected MRU()
        {
            this.list = new MRUList();
            // load initiates new list if the path is correct
            LoadMRU();
            this.list.ShowMaxEntries = SettingsWrapper.Instance.NumberOfRecent;
        }

        public static MRU Instance
        {
            get
            {
                return _instance;
            }
        }

        public void AddItem(string fileName, string pathName, int page, System.Windows.Media.Imaging.BitmapImage image)
        {
            this.list.AddMRUItem(MRUItem.Create(fileName, pathName, page, image));
        }

        public bool CheckFile(string path)
        {
            bool result = System.IO.File.Exists(path);
            int i=this.list.IndexOf(path);
            if ((!result) && (i >= 0))
            {
                this.list.RemoveAt(i);
            }
            //else if (i >= 0)
            //{
            //    MRUItem item = this.list[i];
            //    this.list.AddMRUItem(item);
            //}
            return result;
        }

        //public List<System.Windows.Controls.MenuItem> GetMenuItems()
        //{
        //    return this.list.GetMenuItems();
        //}

        public List<IndexedMRUItem> GetMRUItems()
        {
            List<IndexedMRUItem> tmp = new List<IndexedMRUItem>();
            for(int i=0;i<this.list.Count;i++)
            {                
                tmp.Add(new IndexedMRUItem(i, this.list[i]));
            }
            return tmp;
        }

        public System.Windows.RoutedEventHandler MenuClick 
        {
            get
            {
                return this.list.MenuClick;
            }
            set
            {
                this.list.MenuClick = value;
            }
        }

        public int GetPageNumberForPath(string path)
        {
            int i=this.list.IndexOf(path);
            MRUItem item = null;
            //if ((i >= 0) && (i < Math.Min(this.list.ShowMaxEntries, this.list.MaxEntries)))
            if ((i >= 0) && (i < this.list.ShowMaxEntries))
            {
                item = this.list[i];
            }
            if (item != null)
            {
                return item.PageNumber;
            }
            else
            {
                return 1;
            }
        }

        public void SetNumberOfRecent(int number)
        {
            this.list.ShowMaxEntries = number;
        }

        public string GetFileNameAndPageForKey(System.Windows.Input.Key key)
        {
            int i = -1;
            string result = "";
            switch (key)
            {
                case System.Windows.Input.Key.D0: { i = 0; break; }
                case System.Windows.Input.Key.D1: { i = 1; break; }
                case System.Windows.Input.Key.D2: { i = 2; break; }
                case System.Windows.Input.Key.D3: { i = 3; break; }
                case System.Windows.Input.Key.D4: { i = 4; break; }
                case System.Windows.Input.Key.D5: { i = 5; break; }
                case System.Windows.Input.Key.D6: { i = 6; break; }
                case System.Windows.Input.Key.D7: { i = 7; break; }
                case System.Windows.Input.Key.D8: { i = 8; break; }
                case System.Windows.Input.Key.D9: { i = 9; break; }
                default: { i = -1; break; }
            }
            if ((i >= 0) && (i < this.list.ShowMaxEntries))
            {
                result = this.list[i].FullFileName + ',' + this.list[i].PageNumber.ToString();
            }
            return result;
        }

        public void SaveMRU()
        {
            string path = SettingsTools.GetConfigPath("CBReader", true) + "mru.xml";
            Serialize(path);
        }

        public void LoadMRU()
        {
            string path = SettingsTools.GetConfigPath("CBReader", true) + "mru.xml";
            DeSerialize(path);
        }

        public void Serialize(string path)
        {
            MRUList.Serialize(path, this.list);
        }

        public void DeSerialize(string path)
        {
            if (System.IO.File.Exists(path))
            {
                this.list = MRUList.DeSerialize(path);
            }
        }
    }
}
