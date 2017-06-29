using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.Extensions
{
    internal static class StatementSyntaxExtensions
    {
        public static bool AreEquivalent(this StatementSyntax statement, StatementSyntax other)
        {
            return statement.Kind() == other.Kind()
                && statement.IsEquivalentTo(other, topLevel: false)
                && statement.DescendantTrivia().All(x => x.IsWhitespaceOrEndOfLineTrivia())
                && other.DescendantTrivia().All(x => x.IsWhitespaceOrEndOfLineTrivia());
        }

        public static bool AreEquivalent(this SyntaxList<StatementSyntax> statementList, SyntaxList<StatementSyntax> otherList)
        {
            if (statementList.Count == 1)
            {
                if (otherList.Count == 1)
                {
                    return AreEquivalent(statementList[0], otherList[0]);
                }
            }
            else if (statementList.Count == 2
                && otherList.Count == 2
                && statementList[1].IsKind(SyntaxKind.BreakStatement)
                && otherList[1].IsKind(SyntaxKind.BreakStatement))
            {
                return AreEquivalent(statementList[0], otherList[0]);
            }

            return false;
        }
    }
}
