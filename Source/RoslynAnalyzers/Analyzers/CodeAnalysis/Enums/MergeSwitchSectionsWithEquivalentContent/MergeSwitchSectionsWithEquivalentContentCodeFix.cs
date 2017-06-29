using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.CodeAnalysis.Enums.MergeSwitchSectionsWithEquivalentContent
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MergeSwitchSectionsWithEquivalentContentCodeFix)), Shared]
    public sealed class MergeSwitchSectionsWithEquivalentContentCodeFix : CodeFixProvider
    {
        private const string Title = "Merge switch statements";
        private const string EquivalenceKey = MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer.DiagnosticId + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<SwitchStatementSyntax>(MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer.DiagnosticId);
            if (!result.success) return;

            var switchStatement = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title,
                createChangedDocument: cancellationToken => MergeSwitchStatements(context, root, switchStatement),
                equivalenceKey: EquivalenceKey);

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private Task<Document> MergeSwitchStatements(
            CodeFixContext context, 
            SyntaxNode root, 
            SwitchStatementSyntax switchStatement)
        {
            var newSwitchStatement = switchStatement.MergeSwitchStatementsWithEquivalentContent();
            return context.GetDocumentWithReplacedNode(switchStatement, newSwitchStatement, root);
        }
    }
}
