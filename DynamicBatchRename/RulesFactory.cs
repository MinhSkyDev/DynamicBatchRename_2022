using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRenameRules_namepsace;

namespace DynamicBatchRename
{
    class RulesFactory
    {
        private static RulesFactory instance = null;
        private Dictionary<string, IRenameRules> dict = new Dictionary<string, IRenameRules>();
        private Dictionary<string, IRenameRules> stringPrototype_dict = new Dictionary<string, IRenameRules>();
        private RulesFactory() {
        
        
        }

        private void registerRules(IRenameRules rule)
        {
            string name = rule.getName();
            dict[name] = (IRenameRules) rule.Clone();
            string string_prototype = rule.stringPrototype();
            if(string_prototype.Equals("") == false)
            {
                stringPrototype_dict[string_prototype] = (IRenameRules) rule.Clone();
            }
        }

        public static RulesFactory getInstance()
        {
            if (instance == null)
            {
                instance = new RulesFactory();
            }
            return instance;
        }

        public void RegisterRules(IRenameRules rule)
        {
            string name = rule.getName();
            dict[name] = (IRenameRules) rule.Clone();
        }


        //Input vo se la mot chuoi duoc quy dinh tu file preset .txt
        public IRenameRules createRules(string data)
        {
            IRenameRules result = (IRenameRules) dict[data].Clone();
            
            return result;
        }

    }
}
