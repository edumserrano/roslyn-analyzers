using System.Collections.Immutable;
using Analyzers.AnalyzersMetadata;
using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.AnalyzersMetadata.DiagnosticTitles;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Enums.DefaultLabelShouldBeLastInSwitchStatement
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DefaultLabelShouldBeLastInSwitchStatementDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = EnumDiagnosticIdentifiers.DefaultLabelShouldBeTheLast;
        private static readonly LocalizableString Title = EnumDiagnosticTitles.DefaultLabelShouldBeTheLast;
        private static readonly LocalizableString MessageFormat = EnumDiagnosticMessageFormats.DefaultLabelShouldBeTheLast;

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: Title,
            messageFormat: MessageFormat,
            category: DiagnosticCategories.Readibility,
            defaultSeverity: DiagnosticSeverity.Warning,
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
            var defaultSwitchSection = switchStatement.Sections.GetSwitchSectionWithDefaultLabel();
            var defaultLabel = defaultSwitchSection?.Labels.GetDefaultSwitchLabel();
            if (defaultLabel == null) return;

            var lastSection = switchStatement.Sections.Last();
            var lastLabel = lastSection.Labels.LastOrDefault();
            if (lastLabel == null) return;

            if (lastLabel.IsKind(SyntaxKind.DefaultSwitchLabel)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, defaultLabel.GetLocation()));
        }
    }
}
