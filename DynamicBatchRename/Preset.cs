using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBatchRename
{
    public class Preset
    {
        public string presetName { get; set; }
        public string uri { get; set; }

        public Preset(string presetName, string uri)
        {
            this.presetName = presetName;
            this.uri = uri;
        }
    }
}
