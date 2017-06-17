using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Analyzers.Extensions
{
    internal static class CodeFixContextExtensions
    {
        public static async Task<(bool success, SyntaxNode root, T syntaxNode, Diagnostic diagnostic)> TryGetSyntaxNode<T>(
            this CodeFixContext context,
            string diagnosticId) where T : SyntaxNode
        {
            var diagnostic = context.Diagnostics
                .FirstOrDefault(x => x.Id == diagnosticId);
            if (diagnostic == null)
            {
                return (success: false, root: null, syntaxNode: null, diagnostic: null);
            }

            var root = await context.Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var node = root
                .FindNode(context.Span, getInnermostNodeForTie: true)
                ?.FirstAncestorOrSelf<T>();

            return node == null
                ? (success: false, root: root, syntaxNode: null as T, diagnostic: diagnostic)
                : (success: true, root: root, syntaxNode: node, diagnostic: diagnostic);
        }

        public static async Task<Document> GetDocumentWithReplacedNode(
            this CodeFixContext context,
            SyntaxNode oldNode,
            SyntaxNode newNode,
            SyntaxNode root = null)
        {
            if (root == null)
            {
                root = await context.Document
                    .GetSyntaxRootAsync(context.CancellationToken)
                    .ConfigureAwait(false);
            }

            var newRoot = root.ReplaceNode(oldNode, newNode);
            return context.Document
                .WithSyntaxRoot(newRoot);
        }
    }
}
