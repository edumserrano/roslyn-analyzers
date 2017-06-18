using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.Extensions
{
    internal static class SwitchStatementSyntaxExtensions
    {
        public static Dictionary<long, ISymbol> GetLabelSymbols(
            this SwitchStatementSyntax switchStatement,
            SemanticModel model,
            CancellationToken cancellationToken)
        {
            var labelSymbols = new Dictionary<long, ISymbol>();

            var caseLabels = switchStatement.GetCaseSwitchLabels();
            foreach (var label in caseLabels)
            {
                if (!(model.GetSymbolInfo(label, cancellationToken).Symbol is IFieldSymbol fieldSymbol))
                {
                    // something is wrong with the label and the SemanticModel was unable to determine its symbol
                    // or the symbol is not a field symbol which should be for case labels of switchs on enum types
                    // abort analyzer
                    return new Dictionary<long, ISymbol>();
                }

                var enumValue = fieldSymbol.ConstantValue.ToInt64();
                labelSymbols.Add(enumValue, fieldSymbol);
            }

            return labelSymbols;
        }

        public static List<ExpressionSyntax> GetCaseSwitchLabels(this SwitchStatementSyntax switchStatement)
        {
            var caseLabels = new List<ExpressionSyntax>();

            foreach (var section in switchStatement.Sections)
            {
                foreach (var label in section.Labels)
                {
                    if (label is CaseSwitchLabelSyntax caseLabel)
                    {
                        caseLabels.Add(caseLabel.Value);
                    }
                }
            }

            return caseLabels;
        }

        public static bool HasDefaultSwitchStatement(this SwitchStatementSyntax switchStatement)
        {
            return switchStatement.Sections.GetSwitchSectionWithDefaultLabel() != null;
        }

        public static SwitchStatementSyntax MoveDefaultSwitchSectionToLastInSwitchStatement(this SwitchStatementSyntax switchStatement)
        {
            var switchSections = switchStatement.Sections;
            var defaultSwitchSection = switchSections.GetSwitchSectionWithDefaultLabel();
            if (defaultSwitchSection == null)
            {
                return switchStatement;
            }

            switchSections = switchSections.Remove(defaultSwitchSection);
            switchSections = switchSections.Add(defaultSwitchSection);
            var newSwitchStatement = switchStatement.WithSections(switchSections);
            return newSwitchStatement;
        }

        public static SwitchStatementSyntax MoveDefaultLabelToLastInSwitchSection(this SwitchStatementSyntax switchStatement)
        {
            var switchSections = switchStatement.Sections;
            var defaultSwitchSection = switchSections.GetSwitchSectionWithDefaultLabel();
            if (defaultSwitchSection == null)
            {
                return switchStatement;
            }
            var newDefaultSwitchSection = defaultSwitchSection.MoveDefaultlabelToLastInSwitchSection();

            var newSwitchSections = switchStatement.Sections;
            newSwitchSections = newSwitchSections.Replace(defaultSwitchSection, newDefaultSwitchSection);
            var newSwitchStatement = switchStatement.WithSections(newSwitchSections);
            return newSwitchStatement;
        }

        public static List<List<int>> GetEquivalentSwitchStatementsIndexes(this SwitchStatementSyntax switchStatement)
        {
            var matches = new List<List<int>>();
            var switchSections = switchStatement.Sections;

            for (var i = 0; i < switchSections.Count; i++)
            {
                if (MatchAlreadyFound(matches, i))
                {
                    continue;
                }

                var indexesMatched = new List<int> { i };
                var sectionStatements = switchSections[i].GetStatements();

                for (var j = i + 1; j < switchSections.Count; j++)
                {
                    if (MatchAlreadyFound(matches, j))
                    {
                        continue;
                    }

                    var sectionStatements2 = switchSections[j].GetStatements();
                    if (sectionStatements.AreEquivalent(sectionStatements2))
                    {
                        indexesMatched.Add(j);
                    }
                }

                if (indexesMatched.Count > 1)
                {
                    matches.Add(indexesMatched);
                }
            }

            return matches;
        }

        private static bool MatchAlreadyFound(List<List<int>> matches, int i)
        {
            return matches
                .SelectMany(x => x)
                .Contains(i);
        }

        public static SwitchStatementSyntax MergeSwitchStatementsWithEquivalentContent(this SwitchStatementSyntax switchStatement)
        {
            var dictOfEquivalentIndexes = switchStatement
                .GetEquivalentSwitchStatementsIndexes()
                .ToDictionary(x => x[0], ints =>
                {
                    ints.RemoveAt(0);
                    return ints;
                });

            if (!dictOfEquivalentIndexes.Any())
            {
                return switchStatement;
            }

            //get a list of the switch sections where labels with equivalent statements have been merged into one switch section
            var newSwitchSections = new List<SwitchSectionSyntax>();
            foreach (var keyValuePair in dictOfEquivalentIndexes)
            {
                var sectionIdx = keyValuePair.Key;
                var section = switchStatement.Sections[sectionIdx];

                var newEquivalentLabels = section.Labels;
                foreach (var equivalentIdx in keyValuePair.Value)
                {
                    var equivalentSection = switchStatement.Sections[equivalentIdx];
                    newEquivalentLabels = equivalentSection.Labels
                        .Aggregate(newEquivalentLabels, (current, switchLabelSyntax) => current.Add(switchLabelSyntax));
                }

                var newSwitchSection = switchStatement.Sections[sectionIdx].WithLabels(newEquivalentLabels);
                newSwitchSections.Add(newSwitchSection);
            }

            //get a list of switch sections that need to be removed because they have been merged and are parte of the new switch sections
            var nodesToRemove = new List<SwitchSectionSyntax>();
            foreach (var keyValuePair in dictOfEquivalentIndexes)
            {
                foreach (var i in keyValuePair.Value)
                {
                    nodesToRemove.Add(switchStatement.Sections[i]);
                }

                nodesToRemove.Add(switchStatement.Sections[keyValuePair.Key]);
            }

            //replace the old switch sections with the new switch sections
            var newSwitchStatement = switchStatement.RemoveNodes(nodesToRemove, default(SyntaxRemoveOptions));
            var newSections = newSwitchSections
                .Aggregate(newSwitchStatement.Sections, (current, switchSectionSyntax) => current.Add(switchSectionSyntax));

            return switchStatement.WithSections(newSections);
        }
    }
}
