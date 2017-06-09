namespace Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers
{
    public static class EnumDiagnosticIdentifiers
    {
        public const string EnumPrefix = "ENUM";

        public const string SwitchOnEnumMustHandleAllCases = EnumPrefix + "0001";
        public const string DefaultLabelShouldBeTheLast = EnumPrefix + "0002";
        public const string MergeSwitchSectionsWithEquivalentContent = EnumPrefix + "0003";
        
    }
}