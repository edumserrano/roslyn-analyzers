using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.Extensions
{
    internal static class SwitchSectionSyntaxExtensions
    {
        public static SyntaxList<StatementSyntax> GetStatements(this SwitchSectionSyntax section)
        {
            var statements = section.Statements;

            if (statements.Count == 1)
            {
                var statement = statements[0];
                if (statement.IsKind(SyntaxKind.Block))
                {
                    return ((BlockSyntax)statement).Statements;
                }
            }

            return statements;
        }

        public static SwitchSectionSyntax MoveDefaultlabelToLastInSwitchSection(this SwitchSectionSyntax switchSection)
        {
            var labels = switchSection.Labels;
            var defaultLabel = labels.GetDefaultSwitchLabel();
            if (defaultLabel == null)
            {
                return switchSection;
            }

            labels = labels.Remove(defaultLabel);
            labels = labels.Add(defaultLabel);
            var newDefaultSwitchSection = switchSection.WithLabels(labels);
            return newDefaultSwitchSection;
        }

        public static SwitchSectionSyntax GetSwitchSectionWithDefaultLabel(this SyntaxList<SwitchSectionSyntax> switchSections)
        {
            return switchSections
               .FirstOrDefault(switchSection => switchSection.Labels.Any(label => label.IsKind(SyntaxKind.DefaultSwitchLabel)));
        }
    }
}
