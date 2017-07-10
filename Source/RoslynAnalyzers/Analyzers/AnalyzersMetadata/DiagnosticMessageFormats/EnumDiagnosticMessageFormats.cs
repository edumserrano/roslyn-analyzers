using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.DiagnosticMessageFormats
{
    public static class EnumDiagnosticMessageFormats
    {
        public static readonly LocalizableString DefaultLabelShouldBeTheLast = "The default label should be the last in the switch statement";
        public static readonly LocalizableString MergeSwitchSectionsWithEquivalentContent = "Switch statements with equivalent content should be merged";
        public static readonly LocalizableString SwitchOnEnumMustHandleAllCases = "Add missing switch cases. A switch is considered incomplete if it is missing a possible value of the enum or the default case.";
    }
}