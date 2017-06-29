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
    public sealed class MergeSwitchSectionsWithEquivalentContentDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = EnumDiagnosticIdentifiers.MergeSwitchSectionsWithEquivalentContent;
        private static readonly LocalizableString Title = "Merge switch sections";

        private static readonly LocalizableString MessageFormat =
            "Switch statements with equivalent content should be merged";

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
            context.RegisterSyntaxNodeAction(AnalyzeSwitchStatement, SyntaxKind.SwitchStatement);
        }

        private void AnalyzeSwitchStatement(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<SwitchStatementSyntax>();
            if (!result.success) return;

            var switchStatement = result.syntaxNode;
            var switchSections = switchStatement.Sections;

            var listOfEquivalentIndexes = switchStatement.GetEquivalentSwitchStatementsIndexes();
            if (!listOfEquivalentIndexes.Any()) return;

            var indexes = listOfEquivalentIndexes.SelectMany(x => x);
            foreach (var index in indexes)
            {
                var switchSection = switchSections[index];
                foreach (var switchLabel in switchSection.Labels)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, switchLabel.GetLocation()));
                }
            }
        }
    }
}
