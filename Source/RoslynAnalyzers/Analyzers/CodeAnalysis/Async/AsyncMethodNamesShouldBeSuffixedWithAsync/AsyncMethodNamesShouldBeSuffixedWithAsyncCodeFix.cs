using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.AnalyzersMetadata.CodeFixTitles;
using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Rename;

namespace Analyzers.CodeAnalysis.Async.AsyncMethodNamesShouldBeSuffixedWithAsync
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AsyncMethodNamesShouldBeSuffixedWithAsyncCodeFix)), Shared]
    public sealed class AsyncMethodNamesShouldBeSuffixedWithAsyncCodeFix : CodeFixProvider
    {
        private static readonly LocalizableString Title = AsyncCodeFixTitles.AsyncMethodNamesShouldBeSuffixedWithAsync;
        private const string EquivalenceKey = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<MethodDeclarationSyntax>(AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync);
            if (!result.success) return;

            var methodDeclaration = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var solution = context.Document.Project.Solution;
            var semanticModel = await context.Document.GetSemanticModelAsync().ConfigureAwait(false);

            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken) as IMethodSymbol;
            var oldName = methodDeclaration.Identifier.ValueText;
            var newName = $"{oldName}{AsyncConstants.AsyncSuffix}";

            var codeAction = CodeAction.Create(
                title: string.Format(Title.ToString(), oldName, newName),
                createChangedSolution: cancellationToken => Renamer.RenameSymbolAsync(solution, methodSymbol, newName, default(OptionSet), cancellationToken),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }
}
