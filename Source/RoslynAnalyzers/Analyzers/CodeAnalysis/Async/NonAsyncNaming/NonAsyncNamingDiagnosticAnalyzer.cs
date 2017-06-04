using System;
using System.Collections.Immutable;
using System.Linq;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Async.NonAsyncNaming
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NonAsyncNamingDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string AsyncSuffix = "Async";

        public const string DiagnosticId = "Async003";
        private const string Category = "Async";
        private static readonly LocalizableString Title = "Non asynchronous method names should end with Async";
        private static readonly LocalizableString MessageFormat = "Remove Async suffix from method name";
        private static readonly LocalizableString Description = "Non asynchronous method name should end with Async";
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
            context.RegisterSyntaxNodeAction(ShouldEndWithAsync, SyntaxKind.MethodDeclaration);
        }

        private void ShouldEndWithAsync(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is MethodDeclarationSyntax methodDeclaration)) return;
            if (methodDeclaration.ContainsDiagnostics && methodDeclaration.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error)) return;

            var model = context.SemanticModel;
            var methodSymbol = model.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);
            if (methodSymbol.ReturnsTask() || methodSymbol.IsAsync) return;
            if (!methodSymbol.Name.EndsWith(AsyncSuffix, StringComparison.Ordinal)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation()));
        }
    }
}
