using Analyzers.CodeAnalysis.Async.UseConfigureAwaitFalse;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Utils;
using Xunit;

namespace Analyzers.Tests.Async.UseConfigureAwaitFalse
{
    public class UseConfigureAwaitFalseCodeFixTests
        : CSharpCodeFixProviderTest<UseConfigureAwaitFalseDiagnosticAnalyzer, UseConfigureAwaitFalseCodeFix>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Async;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.UseConfigureAwaitFalse;

        [Theory]
        [InlineData("AsyncMethodCallBefore.cs", "AsyncMethodCallAfter.cs")]
        public void Adds_configure_await_false_to_async_method_call(string sourceBefore, string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
