﻿using Analyzers.Tests._TestEnvironment.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Tests._TestEnvironment.Base
{
    public abstract class CSharpDiagnosticAnalyzerTest<T>
        where T : DiagnosticAnalyzer, new()
    {
        private readonly DiagnosticAnalyzer _diagnosticAnalyzer;

        protected CSharpDiagnosticAnalyzerTest()
        {
            _diagnosticAnalyzer = new T();
        }

        /// <summary>
        /// Called to test a C# DiagnosticAnalyzer when applied on the single inputted string as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected
        /// </summary>
        /// <param name="source">A class in the form of a string to run the analyzer on</param>
        /// <param name="expected"> DiagnosticResults that should appear after the analyzer is run on the source</param>
        public void VerifyDiagnostic(string source, params DiagnosticResult[] expected)
        {
            VerifyDiagnostic(new[] { source }, expected);
        }

        /// <summary>
        /// Called to test a C# DiagnosticAnalyzer when applied on the inputted strings as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected
        /// </summary>
        /// <param name="sources">An array of strings to create source documents from to run the analyzers on</param>
        /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the sources</param>
        public void VerifyDiagnostic(string[] sources, params DiagnosticResult[] expected)
        {
            var actual = _diagnosticAnalyzer.GetSortedDiagnostics(sources, LanguageNames.CSharp);
            _diagnosticAnalyzer.VerifyDiagnosticResults(actual, expected);
        }
    }
}
