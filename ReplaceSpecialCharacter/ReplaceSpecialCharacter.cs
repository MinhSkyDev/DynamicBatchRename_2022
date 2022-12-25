using IRenameRules_namepsace;

namespace ReplaceSpecialCharacter
{
    public class ReplaceSpecialCharacter : IRenameRules
    {
        public string CharacterNeedReplace { get; set; }
        public string CharacterReplaceBy { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string getName()
        {
            return "ReplaceSpecialCharacter";
        }

        public void parseData(string data)
        {
            //Implement later
        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            newFilename = newFilename.Replace(this.CharacterNeedReplace, this.CharacterReplaceBy);

            return newFilename;
        }

        public string stringPrototype()
        {
            return "";
        }
    }
}