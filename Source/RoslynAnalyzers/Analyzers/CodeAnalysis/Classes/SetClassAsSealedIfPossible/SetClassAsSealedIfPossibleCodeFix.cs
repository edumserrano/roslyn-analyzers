using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Analyzers.CodeAnalysis.Classes.SetClassAsSealedIfPossible
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SetClassAsSealedIfPossibleCodeFix)), Shared]
    public sealed class SetClassAsSealedIfPossibleCodeFix : CodeFixProvider
    {
        private const string DiagnosticId = ClassDiagnosticIdentifiers.SetClassAsSealedIfPossible;
        private const string Title = "Add sealed modifier to class";
        private const string EquivalenceKey = DiagnosticId + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<ClassDeclarationSyntax>(DiagnosticId);
            if (!result.success) return;

            var classDeclaration = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title,
                createChangedDocument: cancellationToken => AddSealedModifierToClass(context, root, classDeclaration),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private Task<Document> AddSealedModifierToClass(
            CodeFixContext context,
            SyntaxNode root,
            ClassDeclarationSyntax classDeclaration)
        {
            var syntaxGenerator = SyntaxGenerator.GetGenerator(context.Document);
            var newClassDeclaration = syntaxGenerator.WithModifiers(classDeclaration, DeclarationModifiers.Sealed);
            return context.GetDocumentWithReplacedNode(classDeclaration, newClassDeclaration, root);
        }
    }
}
