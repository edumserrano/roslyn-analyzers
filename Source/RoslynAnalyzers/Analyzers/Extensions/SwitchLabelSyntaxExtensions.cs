using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzers.Extensions
{
    internal static class SwitchLabelSyntaxExtensions
    {
        public static SwitchLabelSyntax GetDefaultSwitchLabel(this SyntaxList<SwitchLabelSyntax> switchLabels)
        {
            return switchLabels.FirstOrDefault(label => label.IsKind(SyntaxKind.DefaultSwitchLabel));
        }
    }
}
