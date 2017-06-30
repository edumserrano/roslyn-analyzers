using Analyzers.Tests._TestEnvironment.Utils;

namespace Analyzers.Tests._TestEnvironment.Base
{
    public abstract class FileReaderTest
    {
        public abstract AnalyzerGroup AnalyzerGroup { get; }

        public abstract AnalyzerName AnalyzerName { get; }

        public abstract AnalysisType AnalysisType { get; }

        public string ReadFile(string filename)
        {
            var fileReader = new TestDataFileReader(AnalyzerGroup, AnalyzerName, AnalysisType);
            return fileReader.ReadFile(filename);
        }
    }
}
