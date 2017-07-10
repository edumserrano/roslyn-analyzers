using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.DiagnosticTitles
{
    public static class EnumDiagnosticTitles
    {
        public static readonly LocalizableString DefaultLabelShouldBeTheLast = "Default switch label";
        public static readonly LocalizableString MergeSwitchSectionsWithEquivalentContent = "Merge switch sections";
        public static readonly LocalizableString SwitchOnEnumMustHandleAllCases = "Populate switch";
    }
}