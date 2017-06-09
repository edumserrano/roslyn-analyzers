using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzers
{

#warning change the name of the Vsix proj to DebugAnalyzers.Vsix
#warning create a wiki with hints on how to create roslyn analyzers/code fixers
    //codefixprovider -> when do I need to use 'var generator = SyntaxGenerator.GetGenerator(context.Document);' ? helps create c# syntax
    //codefixprovider -> var node = root.FindNode(context.Span) gets the node that is highilighted by the analyzer
    //meaning the one that was reported on the anaylizer by context.ReportDiagnostic(Diagnostic.Create(Rule, defaultLabel.GetLocation()));
#warning also create a wiki with how to configure analysers for project and solution (there is something on roslynator docs i think)

    class Todo
    {
    }
}
