using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.ReturnTypes.DoNotReturnNull;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.ReturnTypes.DoNotReturnNull
{
    public class DoNotReturnNullDiagnosticAnalyzerTests : CSharpDiagnosticAnalyzerTest<DoNotReturnNullDiagnosticAnalyzer>
    {
        public override string Filepath { get; } = "ReturnTypes/DoNotReturnNull/DiagnosticAnalyzer";

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("MethodNotReturningNull.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("MethodReturningNull.cs", 7, 13)]
        public void Analyzer_is_triggered(string filename, int diagnosticLine, int diagnosticColumn)
        {
            var source = ReadFile(filename);
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = ReturnTypesDiagnosticIdentifiers.DoNotReturnNull,
                Message = ReturnTypesDiagnosticMessageFormats.DoNotReturnNull.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", diagnosticLine, diagnosticColumn) }
            };

            VerifyDiagnostic(source, expectedDiagnostic);
        }
    }
}
