using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;


namespace Analyzers.CodeAnalysis.Enums.DefaultLabel
{
    //https://msdn.microsoft.com/en-us/magazine/dn904670.aspx
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultLabelCodeFix)), Shared]
    public class DefaultLabelCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DefaultLabelDiagnosticAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.FirstOrDefault(x => x.Id == DefaultLabelDiagnosticAnalyzer.DiagnosticId);
            if (diagnostic == null) return;

            CodeAction codeAction = CodeAction.Create(
                               "Move default label to the last position 2",
                               cancellationToken => GetNewDocument(context),
                               DefaultLabelDiagnosticAnalyzer.DiagnosticId + "CodeFixProvider");

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        public static async Task<Document> GetNewDocument(CodeFixContext context)
        {
            SyntaxGenerator generator = SyntaxGenerator.GetGenerator(context.Document);
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            SyntaxNode node = root.FindNode(context.Span);

            SwitchSectionSyntax defaultSwitchSection = root
                .FindNode(context.Span, getInnermostNodeForTie: true)?
                .FirstAncestorOrSelf<SwitchSectionSyntax>();

            SwitchStatementSyntax switchStatement = root
                .FindNode(context.Span, getInnermostNodeForTie: true)?
                .FirstAncestorOrSelf<SwitchStatementSyntax>();

            var defaultSwitchSectionLabels = defaultSwitchSection.Labels;
            var defaultLabel = defaultSwitchSection.Labels.First(x => x.IsKind(SyntaxKind.DefaultSwitchLabel));
            defaultSwitchSectionLabels = defaultSwitchSectionLabels.Remove(defaultLabel);
            defaultSwitchSectionLabels = defaultSwitchSectionLabels.Add(defaultLabel);

            var newDefaultSwitchSection = defaultSwitchSection.WithLabels(defaultSwitchSectionLabels);

            var switchSections = switchStatement.Sections;
            switchSections = switchSections.Remove(defaultSwitchSection);
            switchSections = switchSections.Add(newDefaultSwitchSection);

            SwitchStatementSyntax newSwitchStatement = switchStatement.WithSections(switchSections);

            var newRoot = root.ReplaceNode(switchStatement, newSwitchStatement);
            var newDocument = context.Document.WithSyntaxRoot(newRoot);
            return newDocument;
        }
    }
}
