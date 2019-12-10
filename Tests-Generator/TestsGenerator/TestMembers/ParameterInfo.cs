using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestsGenerator.TestMembers
{
    public class ParameterInfo
    {
        // variables
        public string Name { get; private set; }
        public TypeSyntax Type { get; set; }



        // methods
        public ParameterInfo(string name, TypeSyntax type)
        {
            Name = name;
            Type = type;
        }
    }
}
