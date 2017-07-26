.. _tips-and-more-resources:

Tips and more resources
=======================

* `Diagnostic Analyzers in Visual Studio 2015: First Look <https://www.pluralsight.com/courses/vs-2015-diagnostic-analyzers-first-look>`_ by Kathleen Dollard.

* `Introduction to the .NET Compiler Platform <https://www.pluralsight.com/courses/dotnet-compiler-platform-introduction>`_ by Bart De Smet.

* If you have a question about how to do something with Roslyn try out the `Gitter channel <https://gitter.im/dotnet/roslyn>`_ for Roslyn.

* Explore the `Roslyn Repo <https://github.com/dotnet/roslyn>`_. Especially look into:

  * `How To Write a C# Analyzer and Code Fix <https://github.com/dotnet/roslyn/wiki/How-To-Write-a-C%23-Analyzer-and-Code-Fix>`_.
  * `The Syntax Visualizer <https://github.com/dotnet/roslyn/wiki/Syntax%20Visualizer>`_. This is an invaluable tool to help understand the syntax tree.
  * `Analyzers and Localization <https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md>`_. I also recommend looking at how localization is done on the project created by the :ref:`default Visual Studio template for analyzers <easy-way>` to get an idea of how it works.
  * `.NET Compiler Platform ("Roslyn") Overview <https://github.com/dotnet/roslyn/wiki/Roslyn%20Overview>`_.

* Since the .NET Compiler Platform is vast I recommend searching `github for roslyn analyzers <https://github.com/search?q=roslyn+analyzer&type=Repositories>`_ to get ideas and learn how to use it.

* Enable **concurrent execution** of your analyzers and prevent them from **running in auto generated code** by using the following in your diagnostic analyzer class:: 

 	public override void Initialize(AnalysisContext context)
	{
		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		...
	}

* There are more advanced Roslyn classes that can aid you in doing very cool things. Look for more Roslyn related info on:

  * **CSharp Syntax Rewriter**
  * **Workspace Services**
  * **Classifier Service**
  * **Formatter Service**
  * **Renamer Service**
  * **Simplifier Service**