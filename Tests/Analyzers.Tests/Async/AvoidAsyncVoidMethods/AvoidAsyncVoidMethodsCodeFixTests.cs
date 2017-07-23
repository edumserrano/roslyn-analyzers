using Analyzers.CodeAnalysis.Async.AvoidAsyncVoidMethods;
using Analyzers.Tests._TestEnvironment.Base;
using Xunit;

namespace Analyzers.Tests.Async.AvoidAsyncVoidMethods
{
    public class AvoidAsyncVoidMethodsCodeFixTests : CSharpCodeFixProviderTest<AvoidAsyncVoidMethodsDiagnosticAnalyzer, AvoidAsyncVoidMethodsCodeFix>
    {
        public override string Filepath { get; } = "Async/AvoidAsyncVoidMethods/CodeFixProvider";
        
        [Theory]
        [InlineData("AsyncVoidMethodBefore.cs", "AsyncVoidMethodAfter.cs")]
        public void Changes_return_type_to_task_for_async_void_method(string sourceBefore, string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
