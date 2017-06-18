using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
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
            var result = await context.TryGetSyntaxNode<SwitchStatementSyntax>(DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer.DiagnosticId);
            if (!result.success) return;

            var switchStatement = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title,
                createChangedDocument: cancellationToken => MoveDefaultLabelToLastInSwitchStatement(context, root, switchStatement),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private Task<Document> MoveDefaultLabelToLastInSwitchStatement(CodeFixContext context, SyntaxNode root, SwitchStatementSyntax switchStatement)
        {
            var newSwitchStatement = switchStatement.MoveDefaultSwitchSectionToLastInSwitchStatement();
            newSwitchStatement = newSwitchStatement.MoveDefaultLabelToLastInSwitchSection();
            return context.GetDocumentWithReplacedNode(switchStatement, newSwitchStatement, root);
        }
    }
}
