using System;

namespace TestData.Classes.SetClassAsSealed.DiagnosticAnalyzer
{
    public class ClassWithVirtualEvent
    {
        public virtual event EventHandler Changed
        {
            add
            {
            }
            remove
            {
            }
        }
    }
}