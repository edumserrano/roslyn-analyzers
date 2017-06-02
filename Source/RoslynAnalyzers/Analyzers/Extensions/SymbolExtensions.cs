using Microsoft.CodeAnalysis;

namespace Analyzers.Extensions
{
    internal static class SymbolExtensions
    {
        internal static bool ReturnsTask(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null) return false;

            var returnType = methodSymbol.ReturnType;
            return returnType.ContainingNamespace.ToString() == "System.Threading.Tasks"
                && returnType.Name == "Task";
        }
    }
}
