using Analyzers.CodeAnalysis.Enums.MergeSwitchSectionsWithEquivalentContent;
using Analyzers.Tests._TestEnvironment.Base;
using Xunit;

namespace Analyzers.Tests.Enums.MergeSwitchSectionsWithEquivalentContent
{
    public class MergeSwitchSectionsWithEquivalentContentCodeFixTests
        : CSharpCodeFixProviderTest<MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer, MergeSwitchSectionsWithEquivalentContentCodeFix>
    {
        public override string Filepath { get; } = "Enums/MergeSwitchSectionsWithEquivalentContent/CodeFixProvider";
        
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
