using IRenameRules_namepsace;

namespace RemoveSpaceBeginningEnding
{
    public class RemoveSpaceBeginningEnding : IRenameRules
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public string getName()
        {
            return "RemoveSpaceBeginningEnding";
        }

        public void parseData(string data)
        {
            //Implement later
        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            while (newFilename[0] == ' ')
            {
                newFilename = newFilename.Remove(0, 1);
            }

            while (newFilename[newFilename.Length - 1] == ' ')
            {
                newFilename = newFilename.Remove(newFilename.Length - 1);
            }

            return newFilename;
        }

        public string stringPrototype()
        {
            return "";
        }
    }
}