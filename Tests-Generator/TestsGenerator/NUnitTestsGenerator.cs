using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestsGenerator.TestMembers;

namespace TestsGenerator
{
    public class NUnitTestsGenerator
    {
        // variables
        private ExecutionDataflowBlockOptions boMaxFilesToLoadCount;
        private ExecutionDataflowBlockOptions boMaxExecuteTasksCount;
        private ExecutionDataflowBlockOptions boMaxFilesToWriteCount;



        // methods
        public NUnitTestsGenerator(int maxFilesToLoadCount, int maxExecuteTasksCount, int maxFilesToWriteCount)
        {
            boMaxFilesToLoadCount = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFilesToLoadCount };
            boMaxExecuteTasksCount = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxExecuteTasksCount };
            boMaxFilesToWriteCount = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFilesToWriteCount };
        }



        public void Generate(List<string> sourceFiles, string destFolder)
        {
            var loadFiles = new TransformBlock<string, FileInfo>(new Func<string, Task<FileInfo>>(LoadContent), boMaxFilesToLoadCount);
            var getTestClasses = new TransformBlock<FileInfo, FileInfo>(new Func<FileInfo, Task<FileInfo>>(GenerateNUnitTests), boMaxExecuteTasksCount);
        }



        private async Task<FileInfo> LoadContent(string sourceFile)
        {
            string content;
            using (var reader = new StreamReader(new FileStream(sourceFile, FileMode.Open)))
            {
                content = await reader.ReadToEndAsync();
            }
            return new FileInfo(sourceFile, content);
        }



        private async Task<FileInfo> GenerateNUnitTests(FileInfo fi)
        {
            return await GenerateCode(fi);
        }



        private async Task<FileInfo> GenerateCode(FileInfo fi)
        {
            var root = await CSharpSyntaxTree.ParseText(fi.Content).GetRootAsync();
            return new FileInfo(Path.GetFileNameWithoutExtension(fi.Name) + "Test.cs", GenerateCodeFromTree(root));
        }



        private string GenerateCodeFromTree(SyntaxNode root)
        {
            var classes = new List<ClassDeclarationSyntax>(root.DescendantNodes().OfType<ClassDeclarationSyntax>());
            var usings = new List<UsingDirectiveSyntax>(root.DescendantNodes().OfType<UsingDirectiveSyntax>());
            var namespaces = new List<NamespaceDeclarationSyntax>(root.DescendantNodes().OfType<NamespaceDeclarationSyntax>());

            var nsInfo = new List<NsInfo>();
            foreach (var ns in namespaces)
            {
                var innerNsClasses = new List<ClassInfo>();
                foreach (var innerNsClass in classes)
                {
                    innerNsClasses.Add(new ClassInfo(innerNsClass.Identifier.ToString(), GetMethods(innerNsClass)));
                }
                nsInfo.Add(new NsInfo(ns.Name.ToString(), innerNsClasses));
            }

            return CodeGenerator.Generate(nsInfo, usings);
        }



        private List<MethodInfo> GetMethods(ClassDeclarationSyntax innerNsClass)
        {
            var methods = innerNsClass.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Modifiers
                .Any(modifier => modifier.ToString() == "public"));

            var result = new List<MethodInfo>();

            foreach (var method in methods)
            {
                result.Add(new MethodInfo(method.Identifier.ToString(), GetParameters(method), method.ReturnType));
            }

            return result;
        }



        private List<ParameterInfo> GetParameters(MethodDeclarationSyntax method)
        {
            return method.ParameterList.Parameters.Select(param => new ParameterInfo(param.Identifier.Value.ToString(), param.Type)).ToList();
        }
    }
}
