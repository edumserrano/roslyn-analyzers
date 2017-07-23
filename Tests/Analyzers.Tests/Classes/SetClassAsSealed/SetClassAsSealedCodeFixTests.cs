using Analyzers.CodeAnalysis.Classes.SetClassAsSealed;
using Analyzers.Tests._TestEnvironment.Base;
using Xunit;

namespace Analyzers.Tests.Classes.SetClassAsSealed
{
    public class SetClassAsSealedCodeFixTests 
        : CSharpCodeFixProviderTest<SetClassAsSealedDiagnosticAnalyzer, SetClassAsSealedCodeFix>
    {
        public override string Filepath { get; } = "Classes/SetClassAsSealed/CodeFixProvider";

        [Theory]
        [InlineData("SetClassAsSealedBeforeFix.cs", "SetClassAsSealedAfterFix.cs")]
        public void Adds_sealed_modifier_to_class_when_its_missing(string sourceBefore, string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
