using Microsoft.CodeAnalysis;

namespace Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats
{
    public static class ClassMessageFormats
    {
        public static readonly LocalizableString SetAllClassesAsSealed = "Seal classes that do not have any virtual or abstract methods, properties, events, or indexers";
    }
}
