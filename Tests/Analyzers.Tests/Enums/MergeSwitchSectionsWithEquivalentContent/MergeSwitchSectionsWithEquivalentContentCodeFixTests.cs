using Analyzers.CodeAnalysis.Enums.MergeSwitchSectionsWithEquivalentContent;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Utils;
using Xunit;

namespace Analyzers.Tests.Enums.MergeSwitchSectionsWithEquivalentContent
{
    public class MergeSwitchSectionsWithEquivalentContentCodeFixTests
        : CSharpCodeFixProviderTest<MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer, MergeSwitchSectionsWithEquivalentContentCodeFix>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Enums;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.MergeSwitchSectionsWithEquivalentContent;

        [Theory]
        [InlineData("EnumWithEquivalentSwitchSectionContentBefore.cs", "EnumWithEquivalentSwitchSectionContentAfter.cs")]
        public void Merges_switch_sections_with_equivalent_content(string sourceBefore,string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
