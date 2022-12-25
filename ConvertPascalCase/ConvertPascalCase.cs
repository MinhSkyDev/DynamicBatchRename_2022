using IRenameRules_namepsace;

namespace ConvertPascalCase
{
    public class ConvertPascalCase : IRenameRules
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public string getName()
        {
            return "ConvertPascalCase";
        }

        public void parseData(string data)
        {
            //Implement later
        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            newFilename = newFilename.ToLower();

            string temp = newFilename.Substring(0, 1);
            temp = temp.ToUpper();
            newFilename = newFilename.Remove(0, 1);
            newFilename = newFilename.Insert(0, temp);

            for (int i = 0; i < newFilename.Length - 1; i++)
            {
                if (newFilename[i] == ' ')
                {
                    temp = newFilename.Substring(i + 1, 1);
                    temp = temp.ToUpper();
                    newFilename = newFilename.Remove(i + 1, 1);
                    newFilename = newFilename.Insert(i + 1, temp);
                }
            }
            newFilename = newFilename.Replace(" ", "");

            return newFilename;
        }

        public string stringPrototype()
        {
            return "";
        }
    }
}