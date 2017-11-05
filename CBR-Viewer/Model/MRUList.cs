#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Windows.Controls;
using System.Windows;

namespace CBR_Viewer.Model
{
    [XmlRootAttribute("MRUList")]
    public class MRUList : IEnumerable<MRUItem>, ICollection<MRUItem>, IList<MRUItem>
    {
        private List<MRUItem> list;
        //public int MaxEntries { get; set; }
        public int ShowMaxEntries { get; set; }
        public RoutedEventHandler MenuClick { get; set; }

        public MRUList()
        {
            this.list = new List<MRUItem>();
            this.ShowMaxEntries = 16;
        }

        public IEnumerator<MRUItem> GetEnumerator()
        {
            //get last one first
            for (int i = this.list.Count - 1; i >= 0; i--)
            {
                yield return this.list[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddMRUItem(MRUItem item)
        {
            Remove(item);
            this.list.Add(item);
            CheckLength();
        }

        private void CheckLength()
        {
            if (this.Count > this.ShowMaxEntries)
            {
                this.list.RemoveAt(0);
            }
        }

        // Needed for serialization, but usualy adds items in the wrong order
        [Obsolete("Use AddMRUItem to add items")]
        public void Add(MRUItem item)
        {
            Remove(item);
            this.list.Insert(0, item);
            CheckLength();
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(MRUItem item)
        {            
            return Contains(item.FullFileName);
        }

        public bool Contains(string path)
        {            
            if (IndexOf(path) >= 0)
            {
                return true;
            }
            return false;
        }

        public void CopyTo(MRUItem[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        [XmlIgnore]
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(MRUItem item)
        {
            int i = IndexOf(item);
            if (i >= 0)
            {
                RemoveAt(i);
                return true;
            }
            return false;
        }

        protected int GetListIndex(int index)
        {
            int i = this.list.Count - 1 - index;
            if ((i < 0) || (i >= Count))
            {
                throw new ArgumentOutOfRangeException();
            }
            return i;
        }

        public void RemoveAt(int index)
        {
            int i = GetListIndex(index);
            this.list.RemoveAt(i);
        }

        public int IndexOf(MRUItem item)
        {
            return IndexOf(item.FullFileName);
        }

        public int IndexOf(string path)
        {
            for (int i = this.list.Count - 1; i >= 0; i--)
            {
                if (this.list[i].FullFileName == path)
                {
                    return GetListIndex(i);
                }
            }
            return -1;
        }

        public void Insert(int index, MRUItem item)
        {
            int i = Count - 1 - index;
            if (!Remove(item))
            { 
                i++;
            }
            if (i != this.list.Count)
            {
                this.list.Insert(i, item);
            }
            else
            {
                this.list.Add(item);
            }
            CheckLength();
        }

        public MRUItem this[int index]
        {
            get
            {
                int i = GetListIndex(index);
                return this.list[i];
            }
            set
            {
                int i = GetListIndex(index);
                this.list[i] = value;
            }
        }
        
        public List<MRUItem> GetMRUItems()
        {
            return this.list;
        }

        //public List<MenuItem> GetMenuItems()
        //{
        //    List<MenuItem> result = new List<MenuItem>();
        //    foreach (MRUItem item in this)
        //    {
        //        //if (result.Count >= Math.Min(this.MaxEntries, this.ShowMaxEntries))
        //        if (result.Count >= this.ShowMaxEntries)                
        //        {
        //            break;
        //        }

        //        MenuItem mu = new MenuItem();
        //        mu.FontFamily = new System.Windows.Media.FontFamily("Segoe UI"); ;
        //        mu.FontSize = 11;
        //        mu.Foreground=new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF,0xFF,0xFF,0xFF));
        //        mu.Background=new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF,0xF7,0x94,0x1E));   
        //        Grid g = new Grid();
        //        ColumnDefinition cd = new ColumnDefinition();
        //        cd.Width = new GridLength(20);
        //        g.ColumnDefinitions.Add(cd);
        //        cd = new ColumnDefinition();
        //        cd.Width = new GridLength(300);
        //        g.ColumnDefinitions.Add(cd);
        //        cd = new ColumnDefinition();
        //        cd.Width = new GridLength(10);
        //        g.ColumnDefinitions.Add(cd);
        //        //cd = new ColumnDefinition();
        //        //cd.Width = new GridLength(25);
        //        //g.ColumnDefinitions.Add(cd);
        //        RowDefinition rd = new RowDefinition();
        //        rd.Height = new GridLength(35);
        //        g.RowDefinitions.Add(rd);
        //        rd = new RowDefinition();
        //        rd.Height = new GridLength(22);
        //        g.RowDefinitions.Add(rd);


        //        TextBlock tb = new TextBlock();
        //        //tb.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
        //        tb.VerticalAlignment = VerticalAlignment.Center;
        //        tb.HorizontalAlignment = HorizontalAlignment.Center;
        //        //tb.FontSize = 11;
        //        tb.Text = IndexOf(item).ToString()+" - ";
        //        //tb.Hint = path;

        //        g.Children.Add(tb);
        //        Grid.SetRow(tb, 0);
        //        Grid.SetColumn(tb, 0);
        //        Grid.SetRowSpan(tb, 2);
                
        //        tb = new TextBlock();
        //        //tb.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
        //        tb.FontSize = 24;
        //        tb.HorizontalAlignment = HorizontalAlignment.Center;
        //        tb.VerticalAlignment = VerticalAlignment.Center;
        //        tb.Text = System.IO.Path.GetFileNameWithoutExtension(item.FileName);

        //        g.Children.Add(tb);
        //        Grid.SetRow(tb, 0);
        //        Grid.SetColumn(tb, 1);
        //        Grid.SetColumnSpan(tb, 3);

        //        tb = new TextBlock();
        //        //tb.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
        //        tb.VerticalAlignment = VerticalAlignment.Center;
        //        //tb.FontSize = 11;
        //        tb.Text = item.FullFileName;
        //        tb.ToolTip = item.FullFileName;

        //        g.Children.Add(tb);
        //        Grid.SetRow(tb, 1);
        //        Grid.SetColumn(tb, 1);

        //        tb = new TextBlock();
        //        //tb.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
        //        //tb.FontSize = 11;
        //        tb.VerticalAlignment = VerticalAlignment.Center;
        //        tb.HorizontalAlignment = HorizontalAlignment.Right;
        //        tb.Text = '[' + item.PageNumber.ToString() + ']';

        //        g.Children.Add(tb);
        //        Grid.SetRow(tb, 1);
        //        Grid.SetColumn(tb, 4);

        //        mu.Icon = item.ThumbImage;

        //        mu.Header = g;
        //        if (this.MenuClick != null)
        //        {
        //            mu.CommandParameter = item.FullFileName + "," + item.PageNumber.ToString();

        //            mu.Click += new RoutedEventHandler(MenuClick);

        //        }

        //        // experimenting with keybinding for the menu. 
        //        // experiment froozen (implement a more primitive keybinding first ;-) );
                
        //        //System.Windows.Input.InputGestureCollection igc = new System.Windows.Input.InputGestureCollection();
        //        //igc.Add(new System.Windows.Input.KeyGesture(System.Windows.Input.Key.D0, System.Windows.Input.ModifierKeys.Control, "[Ctrl+0]"));
        //        //System.Windows.Input.RoutedUICommand rc = new System.Windows.Input.RoutedUICommand("myText", "rucName",mu.GetType(), igc);
                
        //        //System.Windows.Input.CommandBinding cb = new System.Windows.Input.CommandBinding();
        //        //cb.Command = rc;               
        //        //cb.Executed += new System.Windows.Input.ExecutedRoutedEventHandler(MenuClick);
        //        //cb.CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(RequeryCanExecute);
        //        //if (result.Count == 0)
        //        //{
        //        //    mu.CommandBindings.Add(cb);
        //        //    mu.Command = cb.Command;
        //        //}

        //        result.Add(mu);
        //    }
        //    return result;
        //}

        //private void RequeryCanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}

        static public void Serialize(string path, MRUList list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MRUList));
            using (System.IO.TextWriter writer = new System.IO.StreamWriter(path))
            {
                serializer.Serialize(writer, list);
            }
        }

        static public MRUList DeSerialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MRUList));
            using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(path))
            {
                return (MRUList)serializer.Deserialize(reader);
            }
        }
    }
}
