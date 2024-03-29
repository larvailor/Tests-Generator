﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsGenerator.TestMembers;

namespace TestsGenerator
{
    public class CodeGenerator
    {
        public static List<FileInfo> Generate(List<NsInfo> nsInfo, List<UsingDirectiveSyntax> usings)
        {
            var result = new List<FileInfo>();

            var generatedUsingsDeclaration = GenerateUsingsDeclaration(nsInfo, usings);
            foreach (var ns in nsInfo)
            {
                var generatedNsDeclaration = GenerateNsDeclaration(ns);
                foreach (var innerClass in ns.InnerClasses)
                {
                    var generatedClassDeclaration = GenerateClassDeclaration(innerClass);
                    string fileName = ns.Name + "_" + innerClass.Name + "_autogenerated_" + result.Count.ToString() + ".cs";
                    string fileContent = generatedUsingsDeclaration.NormalizeWhitespace().ToFullString()
                        + "\r\n"
                        + generatedNsDeclaration.WithMembers(generatedClassDeclaration).NormalizeWhitespace().ToFullString();
                    result.Add(new FileInfo(fileName, fileContent));
                }
            }

            return result;
        }



        private static CompilationUnitSyntax GenerateUsingsDeclaration(List<NsInfo> nsInfo, List<UsingDirectiveSyntax> usings)
        {
            return SyntaxFactory.CompilationUnit().WithUsings(GetUsings(nsInfo, usings));
        }



        private static NamespaceDeclarationSyntax GenerateNsDeclaration(NsInfo ns)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(ns.Name));
        }



        private static SyntaxList<MemberDeclarationSyntax> GenerateClassDeclaration(ClassInfo innerClass)
        {
            var result = new SyntaxList<MemberDeclarationSyntax>();
            result = result.Add(SyntaxFactory.ClassDeclaration(innerClass.Name + "_autogenerated")
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                        .WithAttributeLists(
                            SyntaxFactory.SingletonList(
                                SyntaxFactory.AttributeList(
                                    SyntaxFactory.SingletonSeparatedList(
                                        SyntaxFactory.Attribute(
                                            SyntaxFactory.IdentifierName(
                                                "TestFixture"
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        .WithMembers(GetMethodsAndProperties(innerClass)));
            return result;
        }



        private static SyntaxList<UsingDirectiveSyntax> GetUsings(List<NsInfo> namespaces, List<UsingDirectiveSyntax> classUsings)
        {
            classUsings.Add(
                SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("NUnit"),
                        SyntaxFactory.IdentifierName("Framework")
                        )
                    )
                );

            foreach (var namespaceInfo in namespaces)
            {
                classUsings.Add(
                    SyntaxFactory.UsingDirective(
                        SyntaxFactory.IdentifierName(namespaceInfo.Name)
                        )
                    );
            }

            return new SyntaxList<UsingDirectiveSyntax>().AddRange(classUsings);
        }



        private static SyntaxList<MemberDeclarationSyntax> GetMethodsAndProperties(ClassInfo innerClass)
        {
            var methodsAndProperties = new List<MemberDeclarationSyntax>();

            foreach (var method in innerClass.InnerMethods)
            {
                methodsAndProperties.Add(SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.VoidKeyword)
                        ),
                    SyntaxFactory.Identifier(
                        method.Name + "_autogenerated"
                        )
                    )
                .WithAttributeLists(
                    SyntaxFactory.SingletonList(
                        SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Attribute(
                                    SyntaxFactory.IdentifierName(
                                        "Test"
                                        )
                                    )
                                )
                            )
                        )
                    )
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement("Assert.Fail(\"autogenerated\");"))));
            }

            return new SyntaxList<MemberDeclarationSyntax>().AddRange(methodsAndProperties);
        }
    }
}
