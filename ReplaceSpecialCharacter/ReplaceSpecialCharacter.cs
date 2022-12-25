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
            //Se co 2 truong hop, mot truong hop la tu file preset, mot truong hop la tu preview len
            //Format file preset: ReplaceSpecialCharacter a b
            //Format preview: {a,b}


            //Chia case preview:
            if (data.Contains(','))
            {
                //Input "a,b"
                //preview

                var data_split = data.Split(',');
                CharacterNeedReplace= data_split[0];
                CharacterReplaceBy = data_split[1];

            }
            else
            {
                //Input "a b"
                //preset

                var data_split = data.Split(' ');
                CharacterNeedReplace = data_split[0];
                CharacterReplaceBy = data_split[1];
            }

        }

        public string Rename(string filename)
        {
            string newFilename = filename;

            newFilename = newFilename.Replace(this.CharacterNeedReplace, this.CharacterReplaceBy);

            return newFilename;
        }

        public string stringPrototype()
        {
            return "{}";
        }
    }
}