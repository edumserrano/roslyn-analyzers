using System.Collections.Immutable;
using System.Linq;
using Analyzers.AnalyzersMetadata;
using Analyzers.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.CodeAnalysis.Enums.SwitchOnEnumMustHandleAllCases
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SwitchOnEnumMustHandleAllCasesDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = EnumDiagnosticIdentifiers.SwitchOnEnumMustHandleAllCases;
        private static readonly LocalizableString Title = "Populate switch";
        private static readonly LocalizableString MessageFormat = "Add missing switch cases. A switch is considered incomplete if it is missing a possible value of the enum or the default case.";

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
            context.RegisterSyntaxNodeAction(AnalyzeSwitchStatement, SyntaxKind.SwitchStatement);
        }

        private void AnalyzeSwitchStatement(SyntaxNodeAnalysisContext context)
        {
            var result = context.TryGetSyntaxNode<SwitchStatementSyntax>();
            if (!result.success) return;

            var switchStatement = result.syntaxNode;
            var semanticModel = context.SemanticModel;
            var enumType = semanticModel.GetTypeInfo(switchStatement.Expression, context.CancellationToken).Type as INamedTypeSymbol;

            if (!IsValidSwitch(enumType)) return;

            if (!switchStatement.HasDefaultSwitchStatement())
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, switchStatement.SwitchKeyword.GetLocation()));
                return;
            }

            var labelSymbols = switchStatement.GetLabelSymbols(semanticModel, context.CancellationToken);
            if (!labelSymbols.Any()) return;

            var possibleEnumSymbols = enumType.GetAllPossibleEnumSymbols();
            if (!possibleEnumSymbols.Any()) return;

            // possibleEnumSymbols and labelSymbols has a dictionary with the enum value and its corresponding symbols
            // I'm not using the symbols but I could compare a one symbol to another
            // right now I'm comparing only the enum values
            var possibleEnumValues = possibleEnumSymbols.Keys
                .OrderBy(x => x)
                .ToList();
            var declaredEnumValues = labelSymbols.Keys
                .OrderBy(x => x)
                .ToList();

            if (declaredEnumValues.SequenceEqual(possibleEnumValues)) return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, switchStatement.SwitchKeyword.GetLocation()));
        }

        private bool IsValidSwitch(INamedTypeSymbol enumType)
        {
            //only allow switch on enum types
            if (enumType == null || enumType.TypeKind != TypeKind.Enum)
            {
                return false;
            }

            // ignore enums marked with Flags
            foreach (var attribute in enumType.GetAttributes())
            {
                var containingClass = attribute.AttributeClass.ToDisplayString();
                if (containingClass == typeof(System.FlagsAttribute).FullName)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
