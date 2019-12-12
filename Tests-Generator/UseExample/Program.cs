using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsGenerator;

namespace UseExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new Program().PerformAsync();
        }


        public async Task PerformAsync()
        {
            Console.WriteLine("Enter maxFilesToLoadCount: ");
            var maxFilesToLoadCount = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter maxExecuteTasksCount: ");
            var maxExecuteTasksCount = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter maxFilesToWriteCount: ");
            var maxFilesToWriteCount = int.Parse(Console.ReadLine());

            string sourceFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Tests\\TestClasses");
            string destFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Tests\\TestClasses\\GeneratedInRuntime");
            Directory.CreateDirectory(destFolder);

            await new NUnitTestsGenerator(GetSourceFiles(sourceFolder), destFolder, maxFilesToLoadCount, maxExecuteTasksCount, maxFilesToWriteCount).Generate();

            Console.WriteLine("Success");
            Console.ReadLine();
        }



        private List<string> GetSourceFiles(string sourceFolder)
        {
            var sourceFiles = new List<string>();
            foreach (string path in Directory.GetFiles(sourceFolder, "*.cs"))
            {
                sourceFiles.Add(path);
            }

            return sourceFiles;
        }
    }
}
