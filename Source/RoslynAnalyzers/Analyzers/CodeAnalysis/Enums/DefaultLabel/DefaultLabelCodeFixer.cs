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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultLabelCodeFixer))]
    [Shared]
    public class DefaultLabelCodeFixer : CodeFixProvider
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
                               "Move default label to the last position",
                               cancellationToken => GetNewDocument(context),
                               DefaultLabelDiagnosticAnalyzer.DiagnosticId + "CodeFixProvider");

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        public static async Task<Document> GetNewDocument(CodeFixContext context)
        {
            SyntaxGenerator generator = SyntaxGenerator.GetGenerator(context.Document);
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            SyntaxNode node = root.FindNode(context.Span);

            SwitchSectionSyntax switchSection = root
                .FindNode(context.Span, getInnermostNodeForTie: true)?
                .FirstAncestorOrSelf<SwitchSectionSyntax>();



            SyntaxList<SwitchLabelSyntax> labels = switchSection.Labels;
            SwitchLabelSyntax defaultLabel = labels.First(f => f.IsKind(SyntaxKind.DefaultSwitchLabel));
            int index = labels.IndexOf(defaultLabel);
            SwitchLabelSyntax lastLabel = labels.Last();
            labels = labels.Replace(lastLabel, defaultLabel.WithTriviaFrom(lastLabel));
            labels = labels.Replace(labels[index], lastLabel.WithTriviaFrom(defaultLabel));
            SwitchSectionSyntax newSwitchSection = switchSection.WithLabels(labels);
            var newRoot = root.ReplaceNode(switchSection, new List<SyntaxNode> { newSwitchSection });
            var newDocument = context.Document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
    }
}
