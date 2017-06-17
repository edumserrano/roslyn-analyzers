using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Analyzers.Extensions
{
    internal static class SyntaxTriviaExtensions
    {
        public static bool IsWhitespaceOrEndOfLineTrivia(this SyntaxTrivia trivia)
        {
            return trivia.IsKind(SyntaxKind.WhitespaceTrivia) 
                || trivia.IsKind(SyntaxKind.EndOfLineTrivia);
        }
    }
}
