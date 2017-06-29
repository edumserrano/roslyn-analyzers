using System.IO;

namespace Analyzers.Tests._TestEnvironment
{
    internal static class Utils
    {
        public static string ReadFile(
           AnalyzerGroup analyzerGroup,
           AnalyzerName analyzerName,
           AnalysisType analysisType,
           string filename)
        {
            var filePath = $"../../../RoslynAnalyzersTestData/TestData/{analyzerGroup}/{analyzerName}/{analysisType}/{filename}";
            return File.ReadAllText(filePath);
        }
    }

    public enum AnalyzerGroup
    {
        Async,
        Classes,
        Enums
    }

    public enum AnalyzerName
    {
        SetClassAsSealed
    }

    public enum AnalysisType
    {
        CodeFixProvider,
        DiagnosticAnalyzer
    }
}
