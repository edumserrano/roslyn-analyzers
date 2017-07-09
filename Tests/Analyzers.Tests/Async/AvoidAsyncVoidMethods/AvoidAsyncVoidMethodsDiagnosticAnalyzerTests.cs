using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Async.AvoidAsyncVoidMethods;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Analyzers.Tests._TestEnvironment.Utils;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Async.AvoidAsyncVoidMethods
{
    public class AvoidAsyncVoidMethodsDiagnosticAnalyzerTests : CSharpDiagnosticAnalyzerTest<AvoidAsyncVoidMethodsDiagnosticAnalyzer>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Async;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.AvoidAsyncVoidMethods;

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("AsyncNonVoidMethod.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("AsyncVoidMethod.cs", 5, 22)]
        public void Analyzer_is_triggered(string filename, int diagnosticLine, int diagnosticColumn)
        {
            var source = ReadFile(filename);
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AvoidAsyncVoidMethods,
                Message = AsyncDiagnosticMessageFormats.AvoidAsyncVoidMethods.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", diagnosticLine, diagnosticColumn) }
            };
            VerifyDiagnostic(source, expectedDiagnostic);
        }
    }
}
