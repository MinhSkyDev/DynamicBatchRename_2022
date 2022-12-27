using IRenameRules_namepsace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public string parsePreset(string stringPrototype, Stack<IRenameRules> current_st) {

            string result = "";
            string process = new string(stringPrototype);

            Stack<IRenameRules> st = new Stack<IRenameRules>(current_st.Reverse());

            while(st.Count != 0)
            {

                IRenameRules currentRule = st.Pop();
                string ruleName = currentRule.getName();
                string rulePrototype = currentRule.stringPrototype();
                result += ruleName;
                if (rulePrototype != "")
                {
                    char rule_prototype_first = rulePrototype[0];
                    char rule_prototype_second = rulePrototype[1];

                    int colon = process.IndexOf(rule_prototype_first) + 1;
                    int hash = process.IndexOf(rule_prototype_second, colon);
                    if (colon == 0 || hash == -1)
                    {
                        //Colon == 0 vi` -1 +1 = 0
                        MessageBox.Show("There are incorrections in the preview box!\n Please try to type it correctly");
                        break;
                    }
                    string parameters = process.Substring(colon, hash - colon);
                    process = process.Substring(hash + 1, process.Length - hash - 1);
                    parameters = parameters.Replace(",", " ");
                    result += " " + parameters;
                }


                result += "\n";
            }

            return result;

        } 

    }
}
