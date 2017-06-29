using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
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
        private const string Title = "Change return type from void to Task";
        private const string EquivalenceKey = AvoidAsyncVoidMethodsDiagnosticAnalyzer.DiagnosticId + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AvoidAsyncVoidMethodsDiagnosticAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<MethodDeclarationSyntax>(AvoidAsyncVoidMethodsDiagnosticAnalyzer.DiagnosticId);
            if (!result.success) return;

            var methodDeclaration = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title,
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
