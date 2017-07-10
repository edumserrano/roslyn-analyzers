using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.CodeFixTitles
{
    public static class EnumCodeFixTitles
    {
        public static readonly LocalizableString DefaultLabelShouldBeTheLast = "Move default label to the last position";
        public static readonly LocalizableString MergeSwitchSectionsWithEquivalentContent = "Merge switch statements";
    }
}