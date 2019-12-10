using System.Collections.Generic;

namespace TestsGenerator.TestMembers
{
    public class ClassInfo
    {
        // variables
        public string Name { get; private set; }
        public List<MethodInfo> InnerMethods { get; private set; }
        
        
        
        // methods
        public ClassInfo(string name, List<MethodInfo> methods)
        {
            Name = name;
            InnerMethods = methods;
        }
    }
}
