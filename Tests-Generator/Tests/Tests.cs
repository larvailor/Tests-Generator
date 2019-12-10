using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestsGenerator;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        // variables
        private string generatedClassesFolder;



        [TestInitialize]
        public async Task TestsInitialize()
        {
            // get all test classes
            string testClassesFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\TestClasses");
            var allTestClasses = new List<string>();
            foreach (string path in Directory.GetFiles(testClassesFolder, "*.cs"))
            {
                allTestClasses.Add(path);
            }

            // create folder for generated classes
            generatedClassesFolder = System.IO.Path.Combine(testClassesFolder, "NUnitGeneretadClasses");
            Directory.CreateDirectory(generatedClassesFolder);

            // generate classes
            NUnitTestsGenerator generator = new NUnitTestsGenerator(allTestClasses, generatedClassesFolder, 3, 3, 3);
            await generator.GenerateAsync();
        }



        [TestMethod]
        public void When_OneClass_Should_Generate_One_File()
        {
            
        }
    }
}
