using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.DiagnosticTitles
{
    public static class AsyncDiagnosticTitles
    {
        public static readonly LocalizableString AsyncMethodNamesShouldBeSuffixedWithAsync = "Asynchronous method names should end with Async";
        public static readonly LocalizableString AvoidAsyncVoidMethods = "Avoid void returning asynchronous method";
        public static readonly LocalizableString NonAsyncMethodNamesShouldNotBeSuffixedWithAsync = "Non asynchronous method names should end with Async";
        public static readonly LocalizableString UseConfigureAwaitFalse = "Use ConfigureAwait(false) on await expression";
    }
}