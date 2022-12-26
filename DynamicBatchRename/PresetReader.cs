using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBatchRename
{

    //Class doc Preset từ một đường dẫn
    class PresetReader
    {
        private static PresetReader instance = null;
        private PresetReader()
        {
            // do nothing
        }

        public static PresetReader getInstance()
        {
            if(instance == null)
            {
                instance = new PresetReader();
            }
            return instance;
        }

        public List<String> parsePreset(string rulesData)
        {
             
            List<String> lines = new List<String>(rulesData.Split('\n'));
            return lines;
        }

    }
}
