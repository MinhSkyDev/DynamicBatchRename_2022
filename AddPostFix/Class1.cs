using IRenameRules_namepsace;

namespace AddPostFix
{
    public class AddPostFix : IRenameRules
    {

        public string Postfix { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string getName()
        {
            return "AddPostFix<>";
        }

        public void parseData(string data)
        {
            this.Postfix = data;
        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            int index = filename.LastIndexOf('.'); //get index start extension
            newFilename = newFilename.Insert(index, Postfix);

            return newFilename;
        }

        public string stringPrototype()
        {
            return "<>";
        }
    }
}