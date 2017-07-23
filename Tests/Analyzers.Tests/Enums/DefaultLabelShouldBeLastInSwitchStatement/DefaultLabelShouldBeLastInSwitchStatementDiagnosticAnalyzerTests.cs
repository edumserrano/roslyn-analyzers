using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Enums.DefaultLabelShouldBeLastInSwitchStatement;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Enums.DefaultLabelShouldBeLastInSwitchStatement
{
    public class DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzerTests
        : CSharpDiagnosticAnalyzerTest<DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer>
    {
        public override string Filepath { get; } = "Enums/DefaultLabelShouldBeLastInSwitchStatement/DiagnosticAnalyzer";

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("EnumerationWithDefaultSwitchLast.cs")]
        [InlineData("EnumerationWithDefaultSwitchAndCaseLast.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("EnumerationWithDefaultSwitchAndCasehNotLast.cs", 11, 17)]
        [InlineData("EnumerationWithDefaultSwitchNotLast.cs", 11, 17)]
        public void Analyzer_is_triggered(string filename, int diagnosticLine, int diagnosticColumn)
        {
            var source = ReadFile(filename);
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = EnumDiagnosticIdentifiers.DefaultLabelShouldBeTheLast,
                Message = EnumDiagnosticMessageFormats.DefaultLabelShouldBeTheLast.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", diagnosticLine, diagnosticColumn) }
            };

            VerifyDiagnostic(source, expectedDiagnostic);
        }
    }
}
