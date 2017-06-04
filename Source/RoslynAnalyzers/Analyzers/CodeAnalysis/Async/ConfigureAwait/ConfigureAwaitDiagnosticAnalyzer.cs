using System.Collections.Immutable;
using System.Linq;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Async.ConfigureAwait
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConfigureAwaitDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Async001";
        private const string Category = "Async";
        private static readonly LocalizableString Title = "Consider using ConfigureAwait(false)";
        private static readonly LocalizableString MessageFormat = "If possible use ConfigureAwait(false)";
        private static readonly LocalizableString Description = "Methods calls that return Task should be appendend with ConfigureAwait(false)";
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(ShouldUseConfigureAwait, SyntaxKind.AwaitExpression);
        }

        private void ShouldUseConfigureAwait(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is AwaitExpressionSyntax awaitExpression)) return;
            if (awaitExpression.ContainsDiagnostics && awaitExpression.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error)) return;

            var model = context.SemanticModel;
            var expression = awaitExpression.Expression;
            var methodSymbol = model.GetSymbolInfo(expression, context.CancellationToken).Symbol as IMethodSymbol;
            if (!methodSymbol.ReturnsTask()) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, awaitExpression.GetLocation()));
        }
    }
}
