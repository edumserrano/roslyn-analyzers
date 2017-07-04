using Analyzers.CodeAnalysis.Async.AsyncMethodNamesShouldBeSuffixedWithAsync;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Utils;
using Xunit;

namespace Analyzers.Tests.Async.AsyncMethodNamesShouldBeSuffixedWithAsync
{
    public class AsyncMethodNamesShouldBeSuffixedWithAsyncCodeFixTests
        : CSharpCodeFixProviderTest<AsyncMethodNamesShouldBeSuffixedWithAsyncDiagnosticAnalyzer, AsyncMethodNamesShouldBeSuffixedWithAsyncCodeFix>
    {
        public override AnalyzerGroup AnalyzerGroup { get; } = AnalyzerGroup.Async;

        public override AnalyzerName AnalyzerName { get; } = AnalyzerName.AsyncMethodNamesShouldBeSuffixedWithAsync;

        [Theory]
        [InlineData("AsyncGenericTaskMethodWithoutAsyncSuffixBefore.cs", "AsyncGenericTaskMethodWithoutAsyncSuffixAfter.cs")]
        [InlineData("AsyncTaskMethodWithoutAsyncSuffixBefore.cs", "AsyncTaskMethodWithoutAsyncSuffixAfter.cs")]
        [InlineData("AsyncVoidMethodWithoutAsyncSuffixBefore.cs", "AsyncVoidMethodWithoutAsyncSuffixAfter.cs")]
        [InlineData("GenericTaskMethodWithoutAsyncSuffixBefore.cs", "GenericTaskMethodWithoutAsyncSuffixAfter.cs")]
        [InlineData("TaskMethodWithoutAsyncSuffixBefore.cs", "TaskMethodWithoutAsyncSuffixAfter.cs")]
        public void Adds_async_suffix_to_async_methods_when_its_missing(string sourceBefore, string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
