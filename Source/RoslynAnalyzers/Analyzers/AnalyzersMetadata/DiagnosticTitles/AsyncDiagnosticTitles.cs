using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.DiagnosticTitles
{
    public static class AsyncDiagnosticTitles
    {
        public static readonly LocalizableString AsyncMethodNamesShouldBeSuffixedWithAsync = "Asynchronous method names should end with Async";
        public static readonly LocalizableString AvoidAsyncVoidMethods = "Avoid void returning asynchronous method";
    }
}