using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.DiagnosticAnalyzers.Enuns
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DefaultLabelDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ENUM002";
        private const string Category = "Enum";
        private static readonly LocalizableString Title = "Default switch label";
        private static readonly LocalizableString MessageFormat = "Make sure the default label is the last label in the switch statement";
        private static readonly LocalizableString Description = "Default label in switch statement should be the last one";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(DefaultLabelOnSwitchStatementIsLast, SyntaxKind.SwitchStatement);
        }

        private void DefaultLabelOnSwitchStatementIsLast(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is SwitchStatementSyntax switchStatement)) return;
            if (switchStatement.ContainsDiagnostics && switchStatement.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error)) return;

            var defaultLabel = switchStatement.Sections
                .SelectMany(x => x.Labels)
                .FirstOrDefault(x => x.IsKind(SyntaxKind.DefaultSwitchLabel));
            if (defaultLabel == null) return;

            var lastSection = switchStatement.Sections.Last();
            var lastLabel = lastSection.Labels.LastOrDefault();
            if (lastLabel == null) return;

            if (lastLabel.IsKind(SyntaxKind.DefaultSwitchLabel)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, defaultLabel.GetLocation()));
        }
    }
}
