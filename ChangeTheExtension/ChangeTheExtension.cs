using IRenameRules_namepsace;

namespace ChangeTheExtension
{
    public class ChangeTheExtension : IRenameRules
    {
        public string Extension { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public string getName()
        {
            return "ChangeTheExtension";
        }

        public void parseData(string data)
        {
            this.Extension = data;
        }

        public string Rename(string filename)
        {
            string newFilename = filename;
            int index = newFilename.LastIndexOf('.') + 1; //get index start extension
            if (index != -1)
            {
                string preExtension = newFilename.Substring(index, newFilename.Length - index);//get extension
                newFilename = newFilename.Remove(index, preExtension.Length);
                newFilename = newFilename.Insert(index, Extension);
            }
            return newFilename;
        }

        public string stringPrototype()
        {
            return "()";
        }
    }
}