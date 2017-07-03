namespace TestData.Classes.SetClassAsSealed.DiagnosticAnalyzer
{
    public abstract class ClassWithAbstractIndex
    {
        public abstract object this[int index] { get; set; }
    }
}