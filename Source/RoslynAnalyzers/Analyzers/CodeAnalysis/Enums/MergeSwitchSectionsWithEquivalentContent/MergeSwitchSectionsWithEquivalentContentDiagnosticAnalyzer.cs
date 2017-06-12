using System.Collections.Immutable;
using System.Linq;
using Analyzers.CodeAnalysis.AnalyzersMetadata;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Enums.MergeSwitchSectionsWithEquivalentContent
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent;
        private static readonly LocalizableString Title = "Merge switch sections";
        private static readonly LocalizableString MessageFormat = "Switch statements with equivalent content should be merged";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            DiagnosticCategories.Simplification,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(DefaultLabelOnSwitchStatementIsLast, SyntaxKind.SwitchStatement);
        }

        private void DefaultLabelOnSwitchStatementIsLast(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<SwitchStatementSyntax>();
            if (!result.success) return;

            var switchStatement = result.syntaxNode;
            var switchSections = switchStatement.Sections;

            for (var i = 0; i < switchSections.Count; i++)
            {
                var sectionStatements = GetStatements(switchSections[i]);
                for (var j = i + 1; j < switchSections.Count; j++)
                {
                    var sectionStatements2 = GetStatements(switchSections[j]);

                    if (AreEquivalent(sectionStatements, sectionStatements2))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, switchStatement.GetLocation()));
                        return;
                    }
                }
            }
        }


        private SyntaxList<StatementSyntax> GetStatements(SwitchSectionSyntax section)
        {
            var statements = section.Statements;

            if (statements.Count == 1)
            {
                var statement = statements[0];

                if (statement.IsKind(SyntaxKind.Block))
                    return ((BlockSyntax)statement).Statements;
            }

            return statements;
        }

        private bool AreEquivalent(SyntaxList<StatementSyntax> statements, SyntaxList<StatementSyntax> statements2)
        {
            if (statements.Count == 1)
            {
                if (statements2.Count == 1)
                    return AreEquivalent(statements[0], statements2[0]);
            }
            else if (statements.Count == 2
                && statements2.Count == 2
                && statements[1].IsKind(SyntaxKind.BreakStatement)
                && statements2[1].IsKind(SyntaxKind.BreakStatement))
            {
                return AreEquivalent(statements[0], statements2[0]);
            }

            return false;
        }

        private bool AreEquivalent(StatementSyntax statement, StatementSyntax statement2)
        {
            return statement.Kind() == statement2.Kind()
                && statement.IsEquivalentTo(statement2, topLevel: false)
                && statement.DescendantTrivia().All(IsWhitespaceOrEndOfLineTrivia)
                && statement2.DescendantTrivia().All(IsWhitespaceOrEndOfLineTrivia);
        }

        public bool IsWhitespaceOrEndOfLineTrivia(SyntaxTrivia trivia)
        {
            return trivia.IsKind(SyntaxKind.WhitespaceTrivia) || trivia.IsKind(SyntaxKind.EndOfLineTrivia);
        }
    }
}
