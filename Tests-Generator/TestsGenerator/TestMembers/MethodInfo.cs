using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace TestsGenerator.TestMembers
{
    public class MethodInfo
    {
        // variables
        public string Name { get; private set; }
        public List<ParameterInfo> Parameters { get; private set; }
        public TypeSyntax ReturnType { get; private set; }



        // methods
        public MethodInfo(string name, List<ParameterInfo> parameters, TypeSyntax returnType)
        {
            Name = name;
            Parameters = parameters;
            ReturnType = returnType;
        }
    }
}
