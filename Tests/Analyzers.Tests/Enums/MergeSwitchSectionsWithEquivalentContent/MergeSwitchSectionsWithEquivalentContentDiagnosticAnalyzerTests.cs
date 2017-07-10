using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Enums.MergeSwitchSectionsWithEquivalentContent;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Analyzers.Tests._TestEnvironment.Utils;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Enums.MergeSwitchSectionsWithEquivalentContent
{
    public class MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzerTests
        : CSharpDiagnosticAnalyzerTest<MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Enums;
        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.MergeSwitchSectionsWithEquivalentContent;

        [Fact]
        public void Empty_source_code_does_not_trigger_analyzer()
        {
            var source = string.Empty;
            VerifyNoDiagnosticTriggered(source);
        }

        [Theory]
        [InlineData("EnumWithoutEquivalentSwitchSectionContent.cs")]
        public void Analyzer_is_not_triggered(string filename)
        {
            var source = ReadFile(filename);
            VerifyNoDiagnosticTriggered(source);
        }

        [Fact]
        public void Analyzer_is_triggered()
        {
            var source = ReadFile("EnumWithEquivalentSwitchSectionContent.cs");
            var expectedDiagnostic1 = new DiagnosticResult
            {
                Id = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent,
                Message = EnumDiagnosticMessageFormats.MergeSwitchSectionsWithEquivalentContent.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, 17) }
            };
            var expectedDiagnostic2 = new DiagnosticResult
            {
                Id = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent,
                Message = EnumDiagnosticMessageFormats.MergeSwitchSectionsWithEquivalentContent.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 11, 17) }
            };
            var expectedDiagnostic3 = new DiagnosticResult
            {
                Id = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent,
                Message = EnumDiagnosticMessageFormats.MergeSwitchSectionsWithEquivalentContent.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 13, 17) }
            };
            var expectedDiagnostic4 = new DiagnosticResult
            {
                Id = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent,
                Message = EnumDiagnosticMessageFormats.MergeSwitchSectionsWithEquivalentContent.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 15, 17) }
            };

            VerifyDiagnostic(source, new []{ expectedDiagnostic1, expectedDiagnostic2, expectedDiagnostic3, expectedDiagnostic4} );
        }
    }
}
