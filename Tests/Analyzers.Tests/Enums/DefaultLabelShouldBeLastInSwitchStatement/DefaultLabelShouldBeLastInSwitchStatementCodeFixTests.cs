using Analyzers.CodeAnalysis.Enums.DefaultLabelShouldBeLastInSwitchStatement;
using Analyzers.Tests._TestEnvironment.Base;
using Xunit;

namespace Analyzers.Tests.Enums.DefaultLabelShouldBeLastInSwitchStatement
{
    public class DefaultLabelShouldBeLastInSwitchStatementCodeFixTests
        : CSharpCodeFixProviderTest<DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer, DefaultLabelShouldBeLastInSwitchStatementCodeFix>
    {
        public override string Filepath { get; } = "Enums/DefaultLabelShouldBeLastInSwitchStatement/CodeFixProvider";

        [Theory]
        [InlineData("EnumerationWithDefaultSwitchAndCaseNotLastBefore.cs", "EnumerationWithDefaultSwitchAndCaseNotLastAfter.cs")]
        [InlineData("EnumerationWithDefaultSwitchNotLastBefore.cs", "EnumerationWithDefaultSwitchNotLastAfter.cs")]
        public void Moves_default_switch_and_case_to_last(string sourceBefore,string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
