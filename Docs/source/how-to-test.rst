.. _how-to-test-an-analyzer:

How to test an analyzer
=======================

Once you have :ref:`created your test project <creating-test-proj>` I believe the best way to test your analyzer is to look at the example unit tests that are created as part of the :ref:`default Visual Studio template for creating a Roslyn Analyzer <easy-way>`.

The test project created by the default template has two folders inside with helper classes for testing Analyzers and Code Fix Providers:

* Helpers 
* Verifiers

I recommend that you copy these and re-use the classes to test your analyzers and code fixes. In the `roslyn-analyzers repository <https://github.com/edumserrano/roslyn-analyzers/tree/master/Tests/Analyzers.Tests/_TestEnvironment>`_ I have adapted the code from these folders to better suit my testing.

Testing strategy
----------------

Although you can create the contents of the C# source file as a string directly in the test I recommend that you create a test solution and add there all the files that are required for your tests. Add the cases that should trigger your analyzers, the cases that should not trigger the analyzers, how the code is before the code fix provider and how it should be after.

This solution should be a separate solution from the solution where you have your analyzer because it is likely that you will have code in there that will not compile. This solution is not meant to build sucessfully. It is only meant to have the source files for your test cases.

I believe this is a better strategy than using string because it will make it easier to mantain your tests, Visual Studio helps you right the test code and you can use the test solution when :ref:`debugging with the experimental version of Visual Studio <how-to-debug>`.




