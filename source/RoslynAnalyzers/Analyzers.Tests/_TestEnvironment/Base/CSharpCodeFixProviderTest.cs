using Analyzers.Tests._TestEnvironment.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Tests._TestEnvironment.Base
{
    public abstract class CSharpCodeFixProviderTest<TDiagnosticAnalyzer, TCodeFixProvider>
        where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFixProvider : CodeFixProvider, new()
    {
        private readonly CodeFixProvider _codeFixProvider;
        private readonly TDiagnosticAnalyzer _diagnosticAnalyzer;

        protected CSharpCodeFixProviderTest()
        {
            _diagnosticAnalyzer = new TDiagnosticAnalyzer();
            _codeFixProvider = new TCodeFixProvider();
        }
        /// <summary>
        /// Called to test a C# codefix when applied on the inputted string as a source
        /// </summary>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        public void VerifyFix(
            string oldSource,
            string newSource,
            int? codeFixIndex = null,
            bool allowNewCompilerDiagnostics = false)
        {
            _codeFixProvider.VerifyFix(
                language: LanguageNames.CSharp,
                analyzer: _diagnosticAnalyzer,
                oldSource: oldSource,
                newSource: newSource,
                codeFixIndex: codeFixIndex,
                allowNewCompilerDiagnostics: allowNewCompilerDiagnostics);
        }
    }
}
