using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Analyzers.AnalyzersMetadata.CodeFixTitles;
using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticTitles;
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
        private readonly LocalizableString Title = EnumCodeFixTitles.MergeSwitchSectionsWithEquivalentContent;
        private const string EquivalenceKey = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent + "CodeFixProvider";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var result = await context.TryGetSyntaxNode<SwitchStatementSyntax>(EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent);
            if (!result.success) return;

            var switchStatement = result.syntaxNode;
            var diagnostic = result.diagnostic;
            var root = result.root;

            var codeAction = CodeAction.Create(
                title: Title.ToString(),
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
