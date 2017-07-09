using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.CodeFixTitles
{
    public static class AsyncCodeFixTitles
    {
        public static readonly LocalizableString AsyncMethodNamesShouldBeSuffixedWithAsync = "Rename {0} to {1}";
        public static readonly LocalizableString AvoidAsyncVoidMethods = "Change return type from void to Task";
    }
}