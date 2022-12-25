using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBatchRename
{
    public class text : INotifyPropertyChanged, ICloneable
    {
        public string Name { get; set; }
        public string NewName { get; set; }
        public long Size { get; set; }
        public DateTime Status { get; set; }
        public string path;
        public bool isChecked { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
