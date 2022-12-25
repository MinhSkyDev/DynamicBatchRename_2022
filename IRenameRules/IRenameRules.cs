namespace IRenameRules_namepsace
{
    public interface IRenameRules : ICloneable
    {
        public string Rename(string filename);
        public string getName();

        public void parseData(string data);

        //Dùng để demo một số thứ để hiển thị lên cho người dùng nhận dạng
        //Ví dụ dll AddPostFix sẽ sử dụng dấu <> để nhận diện, <ABC> là thêm vào cuối chuỗi ký tự ABC
        public string stringPrototype();
    }
}