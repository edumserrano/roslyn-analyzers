using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.CodeAnalysis.Enums.DefaultLabelShouldBeLastInSwitchStatement
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultLabelShouldBeLastInSwitchStatementCodeFix)), Shared]
    public sealed class DefaultLabelShouldBeLastInSwitchStatementCodeFix : CodeFixProvider
    {
        private const string Title = "Move default label to the last position";
        private const string EquivalenceKey = DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer.DiagnosticId + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<SwitchSectionSyntax>(DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer.DiagnosticId);
            if (!result.success) return;

            var defaultSwitchSection = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title,
                createChangedDocument: cancellationToken => MoveDefaultLabelToLastInSwitchStatement(context, root, defaultSwitchSection),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        public static Task<Document> MoveDefaultLabelToLastInSwitchStatement(CodeFixContext context, SyntaxNode root, SwitchSectionSyntax defaultSwitchSection)
        {
            var switchStatement = root
                .FindNode(context.Span, getInnermostNodeForTie: true)?
                .FirstAncestorOrSelf<SwitchStatementSyntax>();

            var newDefaultSwitchSection = MoveDefaultlabelToLastInSwitchSection(defaultSwitchSection);
            var newSwitchStatement = MoveDefaultSwitchSectionToLastInSwitchStatement(switchStatement, defaultSwitchSection, newDefaultSwitchSection);
            var newRoot = root.ReplaceNode(switchStatement, newSwitchStatement);
            var newDocument = context.Document.WithSyntaxRoot(newRoot);
            return Task.FromResult(newDocument);
        }

        private static SwitchStatementSyntax MoveDefaultSwitchSectionToLastInSwitchStatement(
            SwitchStatementSyntax switchStatement,
            SwitchSectionSyntax defaultSwitchSection,
            SwitchSectionSyntax newDefaultSwitchSection)
        {
            var switchSections = switchStatement.Sections;
            switchSections = switchSections.Remove(defaultSwitchSection);
            switchSections = switchSections.Add(newDefaultSwitchSection);
            var newSwitchStatement = switchStatement.WithSections(switchSections);
            return newSwitchStatement;
        }

        private static SwitchSectionSyntax MoveDefaultlabelToLastInSwitchSection(SwitchSectionSyntax defaultSwitchSection)
        {
            var defaultSwitchSectionLabels = defaultSwitchSection.Labels;
            var defaultLabel = defaultSwitchSection.Labels
                .First(x => x.IsKind(SyntaxKind.DefaultSwitchLabel));
            defaultSwitchSectionLabels = defaultSwitchSectionLabels.Remove(defaultLabel);
            defaultSwitchSectionLabels = defaultSwitchSectionLabels.Add(defaultLabel);
            var newDefaultSwitchSection = defaultSwitchSection.WithLabels(defaultSwitchSectionLabels);
            return newDefaultSwitchSection;
        }
    }
}
