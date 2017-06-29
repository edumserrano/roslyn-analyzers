using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticIdentifiers;
using Analyzers.CodeAnalysis.AnalyzersMetadata.DiagnosticMessageFormats;
using Analyzers.CodeAnalysis.Classes.SetClassAsSealed;
using Analyzers.Tests._TestEnvironment.Base;
using Analyzers.Tests._TestEnvironment.Roslyn.DiagnosticAnalyzers;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests
{
    public class UnitTest1 : CSharpDiagnosticAnalyzerTest<SetClassAsSealedDiagnosticAnalyzer>
    {
        //No diagnostics expected to show up
        [Fact]
        public void TestMethod1()
        {
            var test = @"";
            VerifyDiagnostic(test);
        }

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
                Id = ClassDiagnosticIdentifiers.SetClassAsSealed,
                Message = ClassMessageFormats.SetClassAsSealed.ToString(),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 18) }
            };

            VerifyDiagnostic(test, expected);
        }
    }
}
