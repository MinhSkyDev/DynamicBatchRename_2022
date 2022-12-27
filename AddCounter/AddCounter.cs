using IRenameRules_namepsace;
using System.Diagnostics.Metrics;

namespace AddCounter
{
    public class AddCounter : IRenameRules
    {
        public int Counter { get; set; }

        public AddCounter()
        {
            Counter = 0;
            //do nothing
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public string getName()
        {
            return "AddCounter";
        }

        public void parseData(string data)
        {
            //implement later
            
        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            string counter = String.Format("{0:D2}", Counter);
            int index = filename.LastIndexOf('.'); //get index start extension
            newFilename = newFilename.Insert(index, counter);
            Counter++;
            return newFilename;
        }

        public string stringPrototype()
        {
            return "";
        }
    }
}