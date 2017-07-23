using System.IO;

namespace Analyzers.Tests._TestEnvironment.Base
{
    public abstract class FileReaderTest
    {
        public abstract string Filepath { get; }

        public virtual string PathToTestData { get; } = "../../../RoslynAnalyzersTestData/TestData/";

        public string ReadFile(string filename)
        {
            var pathToFile = $"{PathToTestData}{Filepath}/{filename}";
            return File.ReadAllText(pathToFile); 
        }
    }
}
