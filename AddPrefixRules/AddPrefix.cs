using IRenameRules_namepsace;

namespace AddPrefixRules
{
    public class AddPrefix : IRenameRules
    {
        public string Prefix { get; set; }
        public AddPrefix()
        {
            // do nothing
        }

        public string Rename(string filename)
        {
            string newFilename = "";

            newFilename = Prefix + filename;

            return newFilename;
        }

        public string getName()
        {
            return "AddPrefix";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }


        //Contains prefix
        public void parseData(string data)
        {
            this.Prefix = data;
        }

        public string stringPrototype()
        {
            return "[]";
        }
    }
}