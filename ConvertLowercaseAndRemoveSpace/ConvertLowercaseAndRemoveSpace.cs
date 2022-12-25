using IRenameRules_namepsace;

namespace ConvertLowercaseAndRemoveSpace
{
    public class ConvertLowercaseAndRemoveSpace : IRenameRules
    {
        public object Clone()
        {
           return MemberwiseClone();
        }

        public string getName()
        {
            return "ConvertLowercaseAndRemoveSpace";
        }

        public void parseData(string data)
        {
            // Implement later
        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            newFilename = newFilename.Trim();

            int i = 0;
            while (i < newFilename.Length - 1)
            {
                while (newFilename[i] == ' ' && newFilename[i + 1] == ' ')
                {
                    newFilename = newFilename.Remove(i, 1);
                }
                i = i + 1;
            }

            newFilename = newFilename.ToLower();

            return newFilename;
        }

        public string stringPrototype()
        {
            return "";
        }
    }
}