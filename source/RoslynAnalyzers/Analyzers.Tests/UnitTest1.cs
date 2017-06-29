using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Classes.SetClassAsSealedIfPossible;
using Analyzers.Tests.Roslyn.CodeFixes;
using Analyzers.Tests.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests
{
    public class UnitTest1
    {
        //No diagnostics expected to show up
        [Fact]
        public void TestMethod1()
        {
            var test = @"";

            var diagnosticVerifierAssertions = new DiagnosticVerifierAssertions(new SetClassAsSealedIfPossibleDiagnosticAnalyzer(), null);
            diagnosticVerifierAssertions.VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [Fact]
        public void TestMethod2()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzers.Tests
{
    public class MyClass
    {
    }
}";
            var expected = new DiagnosticResult
            {
                Id = ClassDiagnosticIdentifiers.SetClassAsSealedIfPossible,
                Message = ClassMessageFormats.SetAllClassesAsSealed.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 18) }
            };

            var diagnosticVerifierAssertions = new DiagnosticVerifierAssertions(new SetClassAsSealedIfPossibleDiagnosticAnalyzer(), null);
            diagnosticVerifierAssertions.VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzers.Tests
{
    public sealed class MyClass
    {
    }
}";
            var codeFixVerifierAssertions = new CodeFixVerifierAssertions(new SetClassAsSealedIfPossibleDiagnosticAnalyzer(), null, new SetClassAsSealedIfPossibleCodeFix(), null);
            codeFixVerifierAssertions.VerifyCSharpFix(test, fixtest);
        }
    }
}
