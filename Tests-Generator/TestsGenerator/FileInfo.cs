namespace TestsGenerator
{
    public class FileInfo
    {
        // variables
        public string Name { get; private set; }
        public string Content { get; private set; }



        // methods
        public FileInfo(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}
