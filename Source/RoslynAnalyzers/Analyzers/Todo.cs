namespace Analyzers
{

    
#warning create a wiki with hints on how to create roslyn analyzers/code fixers
    //codefixprovider -> when do I need to use 'var generator = SyntaxGenerator.GetGenerator(context.Document);' ? helps create c# syntax
    //codefixprovider -> var node = root.FindNode(context.Span) gets the node that is highilighted by the analyzer
    //meaning the one that was reported on the anaylizer by context.ReportDiagnostic(Diagnostic.Create(Rule, defaultLabel.GetLocation()));
    // info about 
    // context.EnableConcurrentExecution();
    // context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    // at https://github.com/dotnet/roslyn/issues/6737 from https://www.visualstudio.com/en-us/news/releasenotes/vs2015-update2-vs#CsharpAndVB
    // on paragraph "We made Analyzer API improvements, including enabling Analyzer writers to mark their analyzers for concurrent execution, and providing control over whether analyzers run in generated code."
    
  
#warning what is the difference between (aybe just put it on the wiki):
    //var syntaxGenerator = SyntaxGenerator.GetGenerator(context.Document);
    //and
    //using SyntaxFactory
    // ???

#warning mention roslyn services on the wiki (see pluralsight for reference)
    


    
    //How To - https://github.com/dotnet/roslyn/wiki/How-To-Write-a-C%23-Analyzer-and-Code-Fix
    //gitter for roslyn - https://gitter.im/dotnet/roslyn
    //roslyn repo - https://github.com/dotnet/roslyn
    //

}
