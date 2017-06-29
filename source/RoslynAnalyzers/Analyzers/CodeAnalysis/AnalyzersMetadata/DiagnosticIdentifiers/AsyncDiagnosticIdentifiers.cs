namespace Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers
{
    public static class AsyncDiagnosticIdentifiers
    {
        private const string AsyncPrefix = "ASYNC";

        public const string AsyncMethodNamesShouldBeSuffixedWithAsync = AsyncPrefix + "0001";
        public const string NonAsyncMethodNamesShouldNotBeSuffixedWithAsync = AsyncPrefix + "0002";
        public const string AvoidAsyncVoidMethods = AsyncPrefix + "0003";
        public const string UseConfigureAwaitFalseWhenPossible = AsyncPrefix + "0004";
    }
}