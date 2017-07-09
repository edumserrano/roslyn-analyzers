using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.AnalyzersMetadata.CodeFixTitles;
using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.CodeAnalysis.Async.AvoidAsyncVoidMethods
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AvoidAsyncVoidMethodsCodeFix)), Shared]
    public sealed class AvoidAsyncVoidMethodsCodeFix : CodeFixProvider
    {
        private static readonly LocalizableString Title = AsyncCodeFixTitles.AvoidAsyncVoidMethods;
        private const string EquivalenceKey = AsyncDiagnosticIdentifiers.AvoidAsyncVoidMethods + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AsyncDiagnosticIdentifiers.AvoidAsyncVoidMethods);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<MethodDeclarationSyntax>(AsyncDiagnosticIdentifiers.AvoidAsyncVoidMethods);
            if (!result.success) return;

            var methodDeclaration = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title.ToString(),
                createChangedDocument: cancellationToken => ChangeReturnTypeToTask(context, root, methodDeclaration),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private Task<Document> ChangeReturnTypeToTask(CodeFixContext context, SyntaxNode root, MethodDeclarationSyntax methodDeclaration)
        {
            var newReturnType = SyntaxFactory.IdentifierName("Task");
            var newMethodDeclaration = methodDeclaration.WithReturnType(newReturnType);
            return context.GetDocumentWithReplacedNode(methodDeclaration, newMethodDeclaration, root);
        }
    }
}
