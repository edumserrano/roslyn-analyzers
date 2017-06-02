using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.DiagnosticAnalyzers.Enuns
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PopulateSwitchDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ENUM001";
        private const string Category = "Enum";
        private static readonly LocalizableString Title = "Populate switch";
        private static readonly LocalizableString MessageFormat = "Add missing switch cases. A switch is considered incomplete if it is missing a possible value of the enum or the default case.";
        private static readonly LocalizableString Description = "Switch cases on enums must contain all possible cases. A switch is considered incomplete if it is missing a possible value of the enum or the default case.";
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
            context.RegisterSyntaxNodeAction(SwitchStatementOnEnumContainsAllCases, SyntaxKind.SwitchStatement);
        }

        private void SwitchStatementOnEnumContainsAllCases(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is SwitchStatementSyntax switchStatement)) return;
            if (switchStatement.ContainsDiagnostics && switchStatement.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error)) return;


            var model = context.SemanticModel;
            var enumType = model.GetTypeInfo(switchStatement.Expression, context.CancellationToken).Type as INamedTypeSymbol;

            if (!IsValidSwitch(enumType)) return;

            var caseLabels = GetCaseLabelsExcludingDefaultCase(switchStatement, out bool hasDefaultCase);
            if (!hasDefaultCase)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, switchStatement.GetLocation()));
                return;
            };

            var labelSymbols = GetLabelSymbols(model, caseLabels, context.CancellationToken);
            if (!labelSymbols.Any()) return;

            var possibleEnumSymbols = GetAllPossibleEnumSymbols(enumType);
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

            context.ReportDiagnostic(Diagnostic.Create(Rule, switchStatement.GetLocation()));
        }

        private Dictionary<long, List<ISymbol>> GetAllPossibleEnumSymbols(INamedTypeSymbol enumType)
        {
            var enumValues = new Dictionary<long, List<ISymbol>>();
            var enumMembers = enumType.GetMembers();

            foreach (var enumMember in enumMembers)
            {
                // skip '.ctor' and '__value'
                var fieldSymbol = enumMember as IFieldSymbol;
                if (fieldSymbol == null || fieldSymbol.Type.SpecialType != SpecialType.None)
                {
                    continue;
                }

                if (fieldSymbol.ConstantValue == null)
                {
                    // We have an enum that has problems with it (i.e. non-const members).  We won't
                    // be able to determine properly if the switch is complete.  Assume it is so we
                    // don't offer to do anything.
                    return new Dictionary<long, List<ISymbol>>();
                }

                //// Multiple enum members may have the same value.
                var enumValue = ToInt64(fieldSymbol.ConstantValue);
                if (!enumValues.ContainsKey(enumValue))
                {
                    enumValues.Add(enumValue, new List<ISymbol> { fieldSymbol });
                }
                else
                {
                    enumValues[enumValue].Add(fieldSymbol);
                }
            }

            return enumValues;
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

        private List<ExpressionSyntax> GetCaseLabelsExcludingDefaultCase(SwitchStatementSyntax switchStatement, out bool hasDefaultCase)
        {
            hasDefaultCase = false;
            var caseLabels = new List<ExpressionSyntax>();

            foreach (var section in switchStatement.Sections)
            {
                foreach (var label in section.Labels)
                {
                    if (label.IsKind(SyntaxKind.DefaultSwitchLabel))
                    {
                        hasDefaultCase = true;
                        continue;
                    }

                    if (label is CaseSwitchLabelSyntax caseLabel)
                    {
                        caseLabels.Add(caseLabel.Value);
                    }
                }
            }

            return caseLabels;
        }

        private Dictionary<long, ISymbol> GetLabelSymbols(SemanticModel model, List<ExpressionSyntax> caseLabels, CancellationToken cancellationToken)
        {
            var labelSymbols = new Dictionary<long, ISymbol>();

            foreach (var label in caseLabels)
            {
                if (!(model.GetSymbolInfo(label, cancellationToken).Symbol is IFieldSymbol fieldSymbol))
                {
                    // something is wrong with the label and the SemanticModel was unable to determine its symbol
                    // or the symbol is not a field symbol which should be for case labels of switchs on enum types
                    // abort analyzer
                    return new Dictionary<long, ISymbol>();
                }

                var enumValue = ToInt64(fieldSymbol.ConstantValue);
                labelSymbols.Add(enumValue, fieldSymbol);
            }

            return labelSymbols;
        }

        public long ToInt64(object o)
        {
            return o is ulong ? unchecked((long)(ulong)o) : System.Convert.ToInt64(o);
        }
    }
}
