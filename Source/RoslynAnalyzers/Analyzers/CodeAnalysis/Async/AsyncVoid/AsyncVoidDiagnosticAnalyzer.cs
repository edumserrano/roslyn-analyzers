using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Async.AsyncVoid
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AsyncVoidDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Async004";
        private const string Category = "Async";
        private static readonly LocalizableString Title = "Avoid void returning asynchronous method";
        private static readonly LocalizableString MessageFormat = "Change the return type of the asyncronous method";
        private static readonly LocalizableString Description = "Asynchronous method should not be of void return type";
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
            context.RegisterSyntaxNodeAction(ShouldEndWithAsync, SyntaxKind.MethodDeclaration);
        }

        private void ShouldEndWithAsync(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is MethodDeclarationSyntax methodDeclaration)) return;
            if (methodDeclaration.ContainsDiagnostics && methodDeclaration.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error)) return;

            var model = context.SemanticModel;
            var methodSymbol = model.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);
            if (!methodSymbol.IsAsync
                || methodSymbol.ReturnType.SpecialType != SpecialType.System_Void) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation()));
        }
    }
}
