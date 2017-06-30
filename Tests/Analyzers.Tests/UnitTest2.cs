using Analyzers.CodeAnalysis.Classes.SetClassAsSealed;
using Analyzers.Tests._TestEnvironment;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Utils;
using Xunit;

namespace Analyzers.Tests
{
    public class UnitTest2 : CSharpCodeFixProviderTest<SetClassAsSealedDiagnosticAnalyzer, SetClassAsSealedCodeFix>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Classes;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.SetClassAsSealed;

        [Fact]
        public void TestMethod2()
        {
            var test = ReadFile("SetClassAsSealedBeforeFix.cs");
            var fixtest = ReadFile("SetClassAsSealedAfterFix.cs");
            VerifyFix(test, fixtest);
        }
    }
}
