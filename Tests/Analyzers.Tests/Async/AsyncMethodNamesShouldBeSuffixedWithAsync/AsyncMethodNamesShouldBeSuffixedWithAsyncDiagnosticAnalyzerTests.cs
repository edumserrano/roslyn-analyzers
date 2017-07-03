using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Async.AsyncMethodNamesShouldBeSuffixedWithAsync;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Analyzers.Tests._TestEnvironment.Utils;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Async.AsyncMethodNamesShouldBeSuffixedWithAsync
{
    public class AsyncMethodNamesShouldBeSuffixedWithAsyncDiagnosticAnalyzerTests
        : CSharpDiagnosticAnalyzerTest<AsyncMethodNamesShouldBeSuffixedWithAsyncDiagnosticAnalyzer>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Async;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.AsyncMethodNamesShouldBeSuffixedWithAsync;

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Fact]
        public void Async_methods_without_async_suffix_triggers_analyzer()
        {
            var source = ReadFile("TriggersAsyncMethodNamesShouldBeSuffixedWithAsync.cs");
            var expectedDiagnostic1 = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync,
                Message = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 7, 27) }
            };
            var expectedDiagnostic2 = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync,
                Message = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 12, 27) }
            };
            var expectedDiagnostic3 = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync,
                Message = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 17, 32) }
            };
            var expectedDiagnostic4 = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync,
                Message = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 22, 21) }
            };
            var expectedDiagnostic5 = new DiagnosticResult
            {
                Id = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync,
                Message = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 27, 26) }
            };
            
            VerifyDiagnostic(source, new[]
            {
                expectedDiagnostic1,
                expectedDiagnostic2,
                expectedDiagnostic3,
                expectedDiagnostic4,
                expectedDiagnostic5
            });
        }
    }
}
