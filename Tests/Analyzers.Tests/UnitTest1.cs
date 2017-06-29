using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Classes.SetClassAsSealed;
using Analyzers.Tests._TestEnvironment;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests
{
    public class UnitTest1 : CSharpDiagnosticAnalyzerTest<SetClassAsSealedDiagnosticAnalyzer>
    {
        protected override string ReadFile(string filename)
        {
            return Utils.ReadFile(AnalyzerGroup.Classes, AnalyzerName.SetClassAsSealed, AnalysisType.DiagnosticAnalyzer, filename);
        }

        //No diagnostics expected to show up
        [Fact]
        public void TestMethod1()
        {
            var test = @"";
            VerifyDiagnostic(test);
        }

        [Fact]
        public void TestMethod2()
        {
            var source = ReadFile("TriggersSetClassAsSealed.cs");
            var expected = new DiagnosticResult
            {
                Id = ClassDiagnosticIdentifiers.SetClassAsSealed,
                Message = ClassMessageFormats.SetClassAsSealed.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 18) }
            };

            VerifyDiagnostic(source, expected);
        }
    }
}
