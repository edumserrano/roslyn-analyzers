using Analyzers.CodeAnalysis.Classes.SetClassAsSealed;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Utils;
using Xunit;

namespace Analyzers.Tests.Classes.SetClassAsSealed
{
    public class SetClassAsSealedCodeFixTests : CSharpCodeFixProviderTest<SetClassAsSealedDiagnosticAnalyzer, SetClassAsSealedCodeFix>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Classes;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.SetClassAsSealed;

        [Fact]
        public void Adds_sealed_modifier_to_class_when_its_missing()
        {
            var sourceBeforeFix = ReadFile("SetClassAsSealedBeforeFix.cs");
            var sourceAfterFix = ReadFile("SetClassAsSealedAfterFix.cs");
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
