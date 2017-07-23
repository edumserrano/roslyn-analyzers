.. _nuget-packages:

Nuget packages
==============

When creating the analyzers and test projects you will have to reference nugets that contain the Roslyn APIs and not only enable the development of the Analyzers and Code Fix Providers as well as integrate them with Visual Studio.

To develop analyzers and code fixes you will need two nuget packages:

* `Microsoft.CodeAnalysis.CSharp.Workspaces <https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp.Workspaces>`_.
* `Microsoft.Composition <https://www.nuget.org/packages/Microsoft.Composition>`_.

Although the latest versions of these packages should work I found that for me at least in the `roslyn-analyzers repository <https://github.com/edumserrano/roslyn-analyzers>`_ that was not true. As of the time of writing this document the latest version of those packages are:

* Version 2.3.1 of the nuget Microsoft.CodeAnalysis.CSharp.Workspaces. 
* Version 1.0.31 of the nuget Microsoft.Composition. 

However the versions that are **working properly** are:

* Version **2.2.0** of the nuget Microsoft.CodeAnalysis.CSharp.Workspaces. 
* Version **1.0.30** of the nuget Microsoft.Composition. 

I could not find out why but in the test project all the tests run with the latest version of the packages however if you try to run the debug project and put break points in the analyzers and code fix providers the following happens:

* With version 2.3.1 of Microsoft.CodeAnalysis.CSharp.Workspaces the breakpoints for analyzers are not triggered even though they are loaded. Downgrading to 2.2.0 makes the breakpoints work again.
* With version 1.0.31 of Microsoft.Composition the breakpoints for code fix providers are not triggered even though they are loaded. Downgrading to 1.0.30 makes the breakpoints work again.

So as a word of caution if you want to upgrade the nuget packages then:

* Run the tests after
* Check that breakpoints are triggering for both analyzers and code fix providers using the :ref:`debug VSIX project <how-to-debug>`.

.. note:: The analyzers created using the working versions of the nugets were tested against the following frameworks:

   * .NET Framework 4.6.2
   * .NET Core 1.1
   * .NET Standard 1.4

