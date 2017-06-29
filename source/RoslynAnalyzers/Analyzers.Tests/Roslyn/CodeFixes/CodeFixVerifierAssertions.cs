using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Analyzers.Tests.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;
using Xunit;

namespace Analyzers.Tests.Roslyn.CodeFixes
{
    /// <summary>
    /// Superclass of all Unit tests made for diagnostics with codefixes.
    /// Contains methods used to verify correctness of codefixes
    /// </summary>
    public class CodeFixVerifierAssertions
    {
        private readonly DiagnosticAnalyzer _cSharpAnalyzer;
        private readonly DiagnosticAnalyzer _visualBasicAnalyzer;
        private readonly CodeFixProvider _cSharpCodeFixProvider;
        private readonly CodeFixProvider _visualBasicFixProvider;
        private readonly CodeFixVerifier _codeFixVerifier;
        private readonly DiagnosticVerifier _diagnosticVerifier;

        public CodeFixVerifierAssertions(
            DiagnosticAnalyzer cSharpAnalyzer,
            DiagnosticAnalyzer visualBasicAnalyzer,
            CodeFixProvider cSharpCodeFixProvider,
            CodeFixProvider visualBasicFixProvider)
        {
            _cSharpAnalyzer = cSharpAnalyzer;
            _visualBasicAnalyzer = visualBasicAnalyzer;
            _cSharpCodeFixProvider = cSharpCodeFixProvider;
            _visualBasicFixProvider = visualBasicFixProvider;

            _diagnosticVerifier = new DiagnosticVerifier();
            _codeFixVerifier = new CodeFixVerifier();
        }

        /// <summary>
        /// Called to test a C# codefix when applied on the inputted string as a source
        /// </summary>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        public void VerifyCSharpFix(string oldSource, string newSource, int? codeFixIndex = null, bool allowNewCompilerDiagnostics = false)
        {
            VerifyFix(LanguageNames.CSharp, _cSharpAnalyzer, _cSharpCodeFixProvider, oldSource, newSource, codeFixIndex, allowNewCompilerDiagnostics);
        }

        /// <summary>
        /// Called to test a VB codefix when applied on the inputted string as a source
        /// </summary>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        public void VerifyBasicFix(string oldSource, string newSource, int? codeFixIndex = null, bool allowNewCompilerDiagnostics = false)
        {
            VerifyFix(LanguageNames.VisualBasic, _visualBasicAnalyzer, _visualBasicFixProvider, oldSource, newSource, codeFixIndex, allowNewCompilerDiagnostics);
        }

        /// <summary>
        /// General verifier for codefixes.
        /// Creates a Document from the source string, then gets diagnostics on it and applies the relevant codefixes.
        /// Then gets the string after the codefix is applied and compares it with the expected result.
        /// Note: If any codefix causes new diagnostics to show up, the test fails unless allowNewCompilerDiagnostics is set to true.
        /// </summary>
        /// <param name="language">The language the source code is in</param>
        /// <param name="analyzer">The analyzer to be applied to the source code</param>
        /// <param name="codeFixProvider">The codefix to be applied to the code wherever the relevant Diagnostic is found</param>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied</param>
        private void VerifyFix(string language, DiagnosticAnalyzer analyzer, CodeFixProvider codeFixProvider, string oldSource, string newSource, int? codeFixIndex, bool allowNewCompilerDiagnostics)
        {
            var document = _diagnosticVerifier.CreateDocument(oldSource, language);
            var analyzerDiagnostics = _diagnosticVerifier.GetSortedDiagnosticsFromDocuments(analyzer, new[] { document });
            var compilerDiagnostics = _codeFixVerifier.GetCompilerDiagnostics(document);
            var attempts = analyzerDiagnostics.Length;

            for (var i = 0; i < attempts; ++i)
            {
                var actions = new List<CodeAction>();
                var context = new CodeFixContext(document, analyzerDiagnostics[0], (a, d) => actions.Add(a), CancellationToken.None);
                codeFixProvider.RegisterCodeFixesAsync(context).Wait();

                if (!actions.Any())
                {
                    break;
                }

                if (codeFixIndex != null)
                {
                    document = _codeFixVerifier.ApplyFix(document, actions.ElementAt((int)codeFixIndex));
                    break;
                }

                document = _codeFixVerifier.ApplyFix(document, actions.ElementAt(0));
                analyzerDiagnostics = _diagnosticVerifier.GetSortedDiagnosticsFromDocuments(analyzer, new[] { document });

                var newCompilerDiagnostics = _codeFixVerifier.GetNewDiagnostics(compilerDiagnostics, _codeFixVerifier.GetCompilerDiagnostics(document));

                //check if applying the code fix introduced any new compiler diagnostics
                if (!allowNewCompilerDiagnostics && newCompilerDiagnostics.Any())
                {
                    // Format and get the compiler diagnostics again so that the locations make sense in the output
                    document = document.WithSyntaxRoot(Formatter.Format(document.GetSyntaxRootAsync().Result, Formatter.Annotation, document.Project.Solution.Workspace));
                    newCompilerDiagnostics = _codeFixVerifier.GetNewDiagnostics(compilerDiagnostics, _codeFixVerifier.GetCompilerDiagnostics(document));

                    Assert.True(false,
                        string.Format("Fix introduced new compiler diagnostics:\r\n{0}\r\n\r\nNew document:\r\n{1}\r\n",
                            string.Join("\r\n", newCompilerDiagnostics.Select(d => d.ToString())),
                            document.GetSyntaxRootAsync().Result.ToFullString()));
                }

                //check if there are analyzer diagnostics left after the code fix
                if (!analyzerDiagnostics.Any())
                {
                    break;
                }
            }

            //after applying all of the code fixes, compare the resulting string to the inputted one
            var actual = _codeFixVerifier.GetStringFromDocument(document);
            Assert.Equal(newSource, actual);
        }
    }
}