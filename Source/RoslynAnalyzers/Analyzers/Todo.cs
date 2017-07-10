namespace Analyzers
{
#warning nos docs referir os 2 cursos no pluralsight!
    //section for how to debug
    //section for how to create a project like this: the debug vsxis, the analyzer and the tests

#warning change the name of the Vsix proj to DebugAnalyzers.Vsix
#warning create a wiki with hints on how to create roslyn analyzers/code fixers
    //codefixprovider -> when do I need to use 'var generator = SyntaxGenerator.GetGenerator(context.Document);' ? helps create c# syntax
    //codefixprovider -> var node = root.FindNode(context.Span) gets the node that is highilighted by the analyzer
    //meaning the one that was reported on the anaylizer by context.ReportDiagnostic(Diagnostic.Create(Rule, defaultLabel.GetLocation()));
    // info about 
    // context.EnableConcurrentExecution();
    // context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    // at https://github.com/dotnet/roslyn/issues/6737 from https://www.visualstudio.com/en-us/news/releasenotes/vs2015-update2-vs#CsharpAndVB
    // on paragraph "We made Analyzer API improvements, including enabling Analyzer writers to mark their analyzers for concurrent execution, and providing control over whether analyzers run in generated code."

#warning also create a wiki with how to configure analysers for project and solution (there is something on roslynator docs i think)

    //Google   "visual studio 2017" reset visual studio experimental instance
    // https://www.codeproject.com/Tips/832362/Resetting-the-Visual-Studio-Experimental-Instance
    // https://msdn.microsoft.com/en-us/library/bb166507.aspx
    //
    // C:\Users\eduar\AppData\Local\Microsoft\VisualStudio
    // C:\Program Files(x86)\Microsoft Visual Studio\2017\Enterprise\VSSDK\VisualStudioIntegration\Tools\Bin>
    // CreateExpInstance.exe /Reset /VSInstance=15.0 /RootSuffix=_9e84fa4fRoslyn
    
#warning what is the difference between :
    //var syntaxGenerator = SyntaxGenerator.GetGenerator(context.Document);
    //and
    //using SyntaxFactory
    // ???

#warning localization
    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization

#warning links

    //search for roslyn analyzers on github - https://github.com/search?p=2&q=roslyn+analyzer&type=Repositories&utf8=%E2%9C%93
    //assign all analyzer - https://github.com/anjdreas/roslyn-analyzers#enabledisable-by-comments
    //roslynator - https://github.com/JosefPihrt/Roslynator
    //exception analyzer - https://github.com/SergeyTeplyakov/ExceptionAnalyzer
    //var rewriter - http://www.hexacta.com/2016/01/12/refactoring-roslyn-platform/
    //syntax csharp rewriter - https://johnkoerner.com/csharp/using-a-csharp-syntax-rewriter/
    //How To - https://github.com/dotnet/roslyn/wiki/How-To-Write-a-C%23-Analyzer-and-Code-Fix
    //gitter for roslyn - https://gitter.im/dotnet/roslyn
    //roslyn repo - https://github.com/dotnet/roslyn
    //

}
