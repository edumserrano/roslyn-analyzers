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

namespace Analyzers.CodeAnalysis.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NonAsyncMethodNamesShouldNotBeSuffixedWithAsyncCodeFix)), Shared]
    public sealed class NonAsyncMethodNamesShouldNotBeSuffixedWithAsyncCodeFix : CodeFixProvider
    {
        private readonly LocalizableString Title = AsyncCodeFixTitles.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync;
        private const string EquivalenceKey = AsyncDiagnosticIdentifiers.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AsyncDiagnosticIdentifiers.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<MethodDeclarationSyntax>(AsyncDiagnosticIdentifiers.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync);
            if (!result.success) return;

            var methodDeclaration = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var solution = context.Document.Project.Solution;
            var semanticModel = await context.Document.GetSemanticModelAsync().ConfigureAwait(false);

            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken) as IMethodSymbol;
            var oldName = methodDeclaration.Identifier.ValueText;
            var newName = oldName.Remove(oldName.Length - AsyncConstants.AsyncSuffix.Length);

            var codeAction = CodeAction.Create(
                title: string.Format(Title.ToString(), oldName, newName),
                createChangedSolution: cancellationToken => Renamer.RenameSymbolAsync(solution, methodSymbol, newName, default(OptionSet), cancellationToken),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }
}
