using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    public class SetClassAsSealedIfPossibleDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = ClassDiagnosticIdentifiers.SetClassAsSealedIfPossible;
        private static readonly LocalizableString Title = "Seal classes that do not have any virtual or abstract methods, properties, events, or indexers";
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

            if (classDeclaration.IsStatic()) return;
            if (classDeclaration.IsSealed()) return;

            var methods = classDeclaration.GetMethods();
            var props = classDeclaration.GetProperties();
            var events = classDeclaration.GetEvents();
            var indexers = classDeclaration.GetIndexers();

            if (HasAbstractOrVirtualMethods(methods)) return;
            if (HasAbstractOrVirtualProperties(props)) return;
            if (HasAbstractOrVirtualEvents(events)) return;
            if (HasAbstractOrVirtualIndexers(indexers)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, classDeclaration.GetLocation()));
        }

        private bool HasAbstractOrVirtualMethods(IEnumerable<MethodDeclarationSyntax> methods)
        {
            foreach (var m in methods)
            {
                var modifiers = m.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        private bool HasAbstractOrVirtualProperties(IEnumerable<PropertyDeclarationSyntax> props)
        {
            foreach (var p in props)
            {
                var modifiers = p.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        private bool HasAbstractOrVirtualEvents(IEnumerable<EventDeclarationSyntax> events)
        {
            foreach (var e in events)
            {
                var modifiers = e.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }

        private bool HasAbstractOrVirtualIndexers(IEnumerable<IndexerDeclarationSyntax> indexers)
        {
            foreach (var i in indexers)
            {
                var modifiers = i.Modifiers.Where(x => x.IsKind(SyntaxKind.AbstractKeyword) || x.IsKind(SyntaxKind.VirtualKeyword));
                if (modifiers.Any()) return true;
            }
            return false;
        }
    }
}
