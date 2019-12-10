using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
            boMaxFilesToLoadCount  = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFilesToLoadCount };
            boMaxExecuteTasksCount = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxExecuteTasksCount };
            boMaxFilesToWriteCount = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxFilesToWriteCount };
        }



        public void Generate(List<string> sourceFiles, string destFolder)
        {
            var loadFiles = new TransformBlock<string, FileInfo>(new Func<string, Task<FileInfo>>(LoadContent), boMaxFilesToLoadCount);
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
    }
}
