﻿using System.Collections.Immutable;
using Analyzers.AnalyzersMetadata;
using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.AnalyzersMetadata.DiagnosticTitles;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Async.AvoidAsyncVoidMethods
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AvoidAsyncVoidMethodsDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = AsyncDiagnosticIdentifiers.AvoidAsyncVoidMethods;
        private static readonly LocalizableString Title = AsyncDiagnosticTitles.AvoidAsyncVoidMethods;
        private static readonly LocalizableString MessageFormat = AsyncDiagnosticMessageFormats.AvoidAsyncVoidMethods;

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
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<MethodDeclarationSyntax>();
            if (!result.success) return;

            var methodDeclaration = result.syntaxNode;
            var semanticModel = context.SemanticModel;
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);

            if (!methodSymbol.IsAsync
                || methodSymbol.ReturnType.SpecialType != SpecialType.System_Void) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclaration.ReturnType.GetLocation()));
        }
    }
}
