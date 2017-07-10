using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Enums.SwitchOnEnumMustHandleAllCases;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Analyzers.Tests._TestEnvironment.Utils;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Enums.SwitchOnEnumMustHandleAllCases
{
    public class SwitchOnEnumMustHandleAllCasesDiagnosticAnalyzerTests
        : CSharpDiagnosticAnalyzerTest<SwitchOnEnumMustHandleAllCasesDiagnosticAnalyzer>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Enums;
        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.SwitchOnEnumMustHandleAllCases;

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("SwitchWithAllCases.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("SwitchWithMissingCase.cs", 9, 13)]
        [InlineData("SwitchWithoutDefaultCase.cs", 7, 13)]
        public void Analyzer_is_triggered(string filename, int diagnosticLine, int diagnosticColumn)
        {
            var source = ReadFile(filename);
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = EnumDiagnosticIdentifiers.SwitchOnEnumMustHandleAllCases,
                Message = EnumDiagnosticMessageFormats.SwitchOnEnumMustHandleAllCases.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", diagnosticLine, diagnosticColumn) }
            };

            VerifyDiagnostic(source, expectedDiagnostic);
        }
    }
}
