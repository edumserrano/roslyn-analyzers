using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Analyzers.CodeAnalysis.Async.UseConfigureAwaitFalse
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseConfigureAwaitFalseCodeFix)), Shared]
    public sealed class UseConfigureAwaitFalseCodeFix : CodeFixProvider
    {
        private const string Title = "Add ConfigureAwait(false)";
        private const string EquivalenceKey = UseConfigureAwaitFalseDiagnosticAnalyzer.DiagnosticId + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(UseConfigureAwaitFalseDiagnosticAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<AwaitExpressionSyntax>(UseConfigureAwaitFalseDiagnosticAnalyzer.DiagnosticId);
            if (!result.success) return;

            var awaitExpression = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title,
                createChangedDocument: cancellationToken => AddConfigureAwaitFalse(context, root, awaitExpression),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private Task<Document> AddConfigureAwaitFalse(
            CodeFixContext context,
            SyntaxNode root,
            AwaitExpressionSyntax awaitExpression)
        {
            var invocation = (InvocationExpressionSyntax)awaitExpression.Expression;
            var syntaxGenerator = SyntaxGenerator.GetGenerator(context.Document);

            var memberAccessExpression = syntaxGenerator.MemberAccessExpression(invocation.WithTrailingTrivia(), syntaxGenerator.IdentifierName("ConfigureAwait"));
            var argument = syntaxGenerator.Argument(syntaxGenerator.FalseLiteralExpression());
            var newInvocation = syntaxGenerator.InvocationExpression(memberAccessExpression, argument) as InvocationExpressionSyntax;

            var newAwaitExpression = awaitExpression.WithExpression(newInvocation);
            return context.GetDocumentWithReplacedNode(awaitExpression, newAwaitExpression, root);
        }
    }
}
