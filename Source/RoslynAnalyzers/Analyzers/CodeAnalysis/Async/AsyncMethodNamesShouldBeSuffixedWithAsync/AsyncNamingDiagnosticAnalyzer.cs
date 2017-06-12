using System;
using System.Collections.Immutable;
using Analyzers.CodeAnalysis.AnalyzersMetadata;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Async.AsyncMethodNamesShouldBeSuffixedWithAsync
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AsyncNamingDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string AsyncSuffix = "Async";

        public const string DiagnosticId = AsyncDiagnosticIdentifiers.AsyncMethodNamesShouldBeSuffixedWithAsync;
        private static readonly LocalizableString Title = "Asynchronous method names should end with Async";
        private static readonly LocalizableString MessageFormat = "Append asynchronous method name with Async";

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
            var model = context.SemanticModel;
            var methodSymbol = model.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);

            if (!methodSymbol.ReturnsTask() && !methodSymbol.IsAsync) return;
            if (methodSymbol.Name.EndsWith(AsyncSuffix, StringComparison.Ordinal)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation()));
        }
    }
}
