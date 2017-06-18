using System.Collections.Immutable;
using Analyzers.CodeAnalysis.AnalyzersMetadata;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Classes.SetClassAsSealedIfPossible
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SetClassAsSealedIfPossibleDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = ClassDiagnosticIdentifiers.SetClassAsSealedIfPossible;
        private static readonly LocalizableString Title = "Seal class";
        private static readonly LocalizableString MessageFormat = "Seal classes that do not have any virtual or abstract methods, properties, events, or indexers";

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
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<ClassDeclarationSyntax>();
            if (!result.success) return;

            var classDeclaration = result.syntaxNode;

            if (classDeclaration.IsSealed()
                || classDeclaration.IsStatic()
                || classDeclaration.HasAbstractOrVirtualMethods()
                || classDeclaration.HasAbstractOrVirtualProperties()
                || classDeclaration.HasAbstractOrVirtualEvents()
                || classDeclaration.HasAbstractOrVirtualIndexers())
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation()));
        }
    }
}
