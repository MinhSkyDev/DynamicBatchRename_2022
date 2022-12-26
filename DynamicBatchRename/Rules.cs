using IRenameRules_namepsace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBatchRename
{
    //Rules class,use for binding 
    internal class Rules : ICloneable
    {
        public string Name { get; set; }
        public IRenameRules rule;

        public Rules(IRenameRules rule)
        {
            this.rule = rule;
            this.Name = rule.getName() + rule.stringPrototype();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void setRule(IRenameRules rule) => this.rule = rule;
        public IRenameRules getRule() => rule;
    }
}
