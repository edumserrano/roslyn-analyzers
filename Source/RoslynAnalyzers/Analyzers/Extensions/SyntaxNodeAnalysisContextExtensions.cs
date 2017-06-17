using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Extensions
{
    internal static class SyntaxNodeAnalysisContextExtensions
    {
        public static (bool success, T syntaxNode) TryGetSyntaxNode<T>(this SyntaxNodeAnalysisContext context) where T : SyntaxNode
        {
            if (!(context.Node is T node))
            {
                return (success: false, syntaxNode: null);
            }

            if (node.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error))
            {
                return (success: false, syntaxNode: null);
            }

            return (success: true, syntaxNode: node);
        }
    }
}
