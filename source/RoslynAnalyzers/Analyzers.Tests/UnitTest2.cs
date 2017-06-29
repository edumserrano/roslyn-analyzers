using Analyzers.CodeAnalysis.Classes.SetClassAsSealedIfPossible;
using Analyzers.Tests._TestEnvironment.Base;
using Xunit;

namespace Analyzers.Tests
{
    public class UnitTest2
        : CSharpCodeFixProviderTest<SetClassAsSealedIfPossibleDiagnosticAnalyzer, SetClassAsSealedIfPossibleCodeFix>
    {
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
            VerifyFix(test, fixtest);
        }
    }
}
