using System.IO;

namespace Analyzers.Tests._TestEnvironment.Utils
{
    internal class TestDataFileReader
    {
        private readonly AnalyzerGroup _analyzerGroup;
        private readonly AnalyzerName _analyzerName;
        private readonly AnalysisType _analysisType;

        public TestDataFileReader(
            AnalyzerGroup analyzerGroup,
            AnalyzerName analyzerName,
            AnalysisType analysisType)
        {
            _analyzerGroup = analyzerGroup;
            _analyzerName = analyzerName;
            _analysisType = analysisType;
        }

        public string ReadFile(string filename)
        {
            var filePath = $"../../../RoslynAnalyzersTestData/TestData/{_analyzerGroup}/{_analyzerName}/{_analysisType}/{filename}";
            return File.ReadAllText(filePath);
        }
    }
}
