Analyzers in the repository
===========================

Here is the list of the analyzers present in the `roslyn-analyzers repository <https://github.com/edumserrano/roslyn-analyzers/tree/master/Source/RoslynAnalyzers>`_.

.. note:: The analyzers created in the roslyn-analyzers repository were tested and worked on Visual Studio 2017 15.2 on different project types:

   * .NET Framework 4.6.2
   * .NET Core 1.1
   * .NET Standard 1.4

=================================================================================================================  ==========  =======================================================  =================
Name                                                                                                               Identifier  Title                                                    Default action     
=================================================================================================================  ==========  =======================================================  =================
:ref:`AsyncMethodNamesShouldBeSuffixedWithAsync <async-method-names-should-be-suffixed-with-async>`                ASYNC0001   Asynchronous method names should end with Async          Warning            
:ref:`NonAsyncMethodNamesShouldNotBeSuffixedWithAsync <non-async-method-names-should-not-be-suffixed-with-async>`  ASYNC0002   Non asynchronous method names should end with Async      Warning            
:ref:`AvoidAsyncVoidMethods <avoid-async-void-methods>`                                                            ASYNC0003   Avoid void returning asynchronous method                 Warning            
:ref:`UseConfigureAwaitFalse <use-configure-await-false>`                                                          ASYNC0004   Use ConfigureAwait(false) on await expression            Warning            
:ref:`SetClassAsSealed <set-class-as-sealed>`                                                                      CLASS0001   Seal class                                               Warning            
:ref:`DefaultLabelShouldBeTheLast <default-label-should-be-the-last>`                                              ENUM0001    Default switch label                                     Warning            
:ref:`MergeSwitchSectionsWithEquivalentContent <merge-switch-sections-with-equivalent-content>`                    ENUM0002    Merge switch sections                                    Warning            
:ref:`SwitchOnEnumMustHandleAllCases <switch-on-enum-must-handle-all-cases>`                                       ENUM0003    Populate switch                                          Warning            
=================================================================================================================  ==========  =======================================================  =================