using Microsoft.CodeAnalysis;

namespace Analyzers.AnalyzersMetadata.DiagnosticMessageFormats
{
    public static class ReturnTypesDiagnosticMessageFormats
    {
        public static readonly LocalizableString DoNotReturnNull = "Avoid returning null. Consider creating a type to represent what you want.";
    }
}