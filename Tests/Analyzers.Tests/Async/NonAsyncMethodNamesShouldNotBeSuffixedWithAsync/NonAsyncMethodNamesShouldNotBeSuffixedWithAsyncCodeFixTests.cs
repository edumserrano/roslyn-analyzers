using Analyzers.CodeAnalysis.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync;
using Analyzers.Tests._TestEnvironment.Base;
using Xunit;

namespace Analyzers.Tests.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync
{
    public class NonAsyncMethodNamesShouldNotBeSuffixedWithAsyncCodeFixTests
        : CSharpCodeFixProviderTest<NonAsyncMethodNamesShouldNotBeSuffixedWithAsyncDiagnosticAnalyzer, NonAsyncMethodNamesShouldNotBeSuffixedWithAsyncCodeFix>
    {
        public override string Filepath { get; } = "Async/NonAsyncMethodNamesShouldNotBeSuffixedWithAsync/CodeFixProvider";
        
        [Theory]
        [InlineData("NonAsyncMethodWithAsyncSuffixBefore.cs", "NonAsyncMethodWithAsyncSuffixAfter.cs")]
        public void Removes_async_suffix_from_non_async_methods(string sourceBefore, string sourceAfter)
        {
            var sourceBeforeFix = ReadFile(sourceBefore);
            var sourceAfterFix = ReadFile(sourceAfter);
            VerifyFix(sourceBeforeFix, sourceAfterFix);
        }
    }
}
