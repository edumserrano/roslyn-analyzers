using Analyzers.CodeAnalysis.Enums.DefaultLabelShouldBeLastInSwitchStatement;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Utils;
using Xunit;

namespace Analyzers.Tests.Enums.DefaultLabelShouldBeLastInSwitchStatement
{
    public class DefaultLabelShouldBeLastInSwitchStatementCodeFixTests
        : CSharpCodeFixProviderTest<DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer, DefaultLabelShouldBeLastInSwitchStatementCodeFix>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Enums;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.DefaultLabelShouldBeLastInSwitchStatement;

        [Theory]
        [InlineData("EnumerationWithDefaultSwitcAndCasehNotLastBefore.cs", "EnumerationWithDefaultSwitcAndCasehNotLastAfter.cs")]
        [InlineData("EnumerationWithDefaultSwitchNotLastBefore.cs", "EnumerationWithDefaultSwitchNotLastAfter.cs")]
        public void Moves_default_switch_and_case_to_last(string sourceBefore,string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
