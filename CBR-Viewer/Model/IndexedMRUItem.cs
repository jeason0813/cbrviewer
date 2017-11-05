using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBR_Viewer.Model
{
    public class IndexedMRUItem : MRUItem
    {
        public string Index { get; private set; }

        public IndexedMRUItem(int index, MRUItem item)
        {
            this.Index = index.ToString() + " -";
            this.Thumb = item.Thumb;
            this.Path = item.Path;
            this.PageNumber = item.PageNumber;
            this.FileName = item.FileName;

        }
    }
}
