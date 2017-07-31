using System;
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

namespace Analyzers.CodeAnalysis.Async.AsyncMethodNamesShouldBeSuffixedWithAsync
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AsyncMethodNamesShouldBeSuffixedWithAsyncDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync;
        private static readonly LocalizableString Title = AsyncDiagnosticTitles.AsyncMethodNamesShouldBeSuffixedWithAsync;
        private static readonly LocalizableString MessageFormat = AsyncDiagnosticMessageFormats.AsyncMethodNamesShouldBeSuffixedWithAsync;

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            DiagnosticCategories.Naming,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<MethodDeclarationSyntax>();
            if (!result.success) return;

            var methodDeclaration = result.syntaxNode;
            var semanticModel = context.SemanticModel;
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);

            if (!methodSymbol.ReturnsTask() && !methodSymbol.IsAsync) return;
            if (methodSymbol.Name.EndsWith(AsyncConstants.AsyncSuffix, StringComparison.Ordinal)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation()));
        }
    }
}
