using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Async.AsyncMethodNamesShouldBeSuffixedWithAsync;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Async.AsyncMethodNamesShouldBeSuffixedWithAsync
{
    public class AsyncMethodNamesShouldBeSuffixedWithAsyncDiagnosticAnalyzerTests
        : CSharpDiagnosticAnalyzerTest<AsyncMethodNamesShouldBeSuffixedWithAsyncDiagnosticAnalyzer>
    {
        public override string Filepath { get; } = "Async/AsyncMethodNamesShouldBeSuffixedWithAsync/DiagnosticAnalyzer";

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("AsyncGenericTaskMethodWithAsyncSuffix.cs")]
        [InlineData("AsyncTaskMethodWithAsyncSuffix.cs")]
        [InlineData("AsyncVoidMethodWithAsyncSuffix.cs")]
        [InlineData("GenericTaskMethodWithAsyncSuffix.cs")]
        [InlineData("TaskMethodWithAsyncSuffix.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("AsyncGenericTaskMethodWithoutAsyncSuffix.cs", 7, 32)]
        [InlineData("AsyncTaskMethodWithoutAsyncSuffix.cs", 7, 27)]
        [InlineData("AsyncVoidMethodWithoutAsyncSuffix.cs", 5, 27)]
        [InlineData("GenericTaskMethodWithoutAsyncSuffix.cs", 7, 26)]
        [InlineData("TaskMethodWithoutAsyncSuffix.cs", 7, 21)]
        public void Analyzer_is_triggered(string filename, int diagnosticLine, int diagnosticColumn)
        {
            var source = ReadFile(filename);
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync,
                Message = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", diagnosticLine, diagnosticColumn) }
            };
            VerifyDiagnostic(source, expectedDiagnostic);
        }
    }
}
