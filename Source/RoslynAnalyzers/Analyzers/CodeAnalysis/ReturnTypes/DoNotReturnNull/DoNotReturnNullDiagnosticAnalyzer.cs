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

namespace Analyzers.CodeAnalysis.ReturnTypes.DoNotReturnNull
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DoNotReturnNullDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = ReturnTypesDiagnosticIdentifiers.DoNotReturnNull;
        private static readonly LocalizableString Title = ReturnTypesDiagnosticTitles.DoNotReturnNull;
        private static readonly LocalizableString MessageFormat = ReturnTypesDiagnosticMessageFormats.DoNotReturnNull;

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            DiagnosticCategories.Maintainability,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ReturnStatement);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<ReturnStatementSyntax>();
            if (!result.success) return;

            var returnStatement = result.syntaxNode;
            if (!returnStatement.Expression.IsKind(SyntaxKind.NullLiteralExpression)) return;
            context.ReportDiagnostic(Diagnostic.Create(Rule, returnStatement.GetLocation()));
        }
    }
}
