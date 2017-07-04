using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Classes.SetClassAsSealed;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Analyzers.Tests._TestEnvironment.Utils;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Classes.SetClassAsSealed
{
    public class SetClassAsSealedDiagnosticAnalyzerTests : CSharpDiagnosticAnalyzerTest<SetClassAsSealedDiagnosticAnalyzer>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Classes;
        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.SetClassAsSealed;

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("ClassWithAbstractEventField.cs")]
        [InlineData("ClassWithAbstractIndex.cs")]
        [InlineData("ClassWithAbstractMethod.cs")]
        [InlineData("ClassWithAbstractProperty.cs")]
        [InlineData("ClassWithStaticModifier.cs")]
        [InlineData("ClassWithVirtualEvent.cs")]
        [InlineData("ClassWithVirtualEventField.cs")]
        [InlineData("ClassWithVirtualIndex.cs")]
        [InlineData("ClassWithVirtualMethod.cs")]
        [InlineData("ClassWithVirtualProperty.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Fact]
        public void Class_without_sealed_modifier_and_without_any_virtual_or_abstract_properties_methods_events_or_indexes_triggers_analyzer()
        {
            var source = ReadFile("ClassWithoutSealedModifier.cs");
            var expectedDiagnostic = new DiagnosticResult
            {
                Id = ClassDiagnosticIdentifiers.SetClassAsSealed,
                Message = ClassDiagnosticMessageFormats.SetClassAsSealed.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 18) }
            };

            VerifyDiagnostic(source, expectedDiagnostic);
        }
    }
}
