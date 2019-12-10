using System.Collections.Generic;

namespace TestsGenerator.TestMembers
{
    public class NsInfo
    {
        // variables
        public string Name { get; private set; }
        public List<ClassInfo> InnerClasses { get; private set; }



        // methods
        public NsInfo(string name, List<ClassInfo> classes)
        {
            Name = name;
            InnerClasses = classes;
        }
    }
}
