using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Classes.SetClassAsSealedIfPossible;
using Analyzers.Tests.Roslyn.CodeFixVerifier;
using Analyzers.Tests.Roslyn.DiagnosticVerifier;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Analyzers.Tests
{
    public class UnitTest1 : CodeFixVerifier
    {
        //No diagnostics expected to show up
        [Fact]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
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

            VerifyCSharpDiagnostic(test, expected);

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
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SetClassAsSealedIfPossibleCodeFix();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SetClassAsSealedIfPossibleDiagnosticAnalyzer();
        }
    }
}
