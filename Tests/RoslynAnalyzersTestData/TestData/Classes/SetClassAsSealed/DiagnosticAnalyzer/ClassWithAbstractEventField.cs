using System;

namespace TestData.Classes.SetClassAsSealed.DiagnosticAnalyzer
{
    public abstract class ClassWithAbstractEventField
    {
        public abstract event EventHandler Changed;
    }
}