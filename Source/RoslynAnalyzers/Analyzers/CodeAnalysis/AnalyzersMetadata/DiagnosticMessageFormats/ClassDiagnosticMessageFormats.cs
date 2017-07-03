using Microsoft.CodeAnalysis;

namespace Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats
{
    public static class ClassDiagnosticMessageFormats
    {
        public static readonly LocalizableString SetClassAsSealed = "Seal classes that do not have any virtual or abstract methods, properties, events, or indexers";
    }
}
