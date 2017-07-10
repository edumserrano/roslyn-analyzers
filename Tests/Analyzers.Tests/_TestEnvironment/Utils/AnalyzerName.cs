namespace Analyzers.Tests._TestEnvironment.Utils
{
    public enum AnalyzerName
    {
        SetClassAsSealed,
        AsyncMethodNamesShouldBeSuffixedWithAsync,
        AvoidAsyncVoidMethods,
        NonAsyncMethodNamesShouldNotBeSuffixedWithAsync,
        UseConfigureAwaitFalse,
        DefaultLabelShouldBeLastInSwitchStatement,
        MergeSwitchSectionsWithEquivalentContent,
        SwitchOnEnumMustHandleAllCases
    }
}