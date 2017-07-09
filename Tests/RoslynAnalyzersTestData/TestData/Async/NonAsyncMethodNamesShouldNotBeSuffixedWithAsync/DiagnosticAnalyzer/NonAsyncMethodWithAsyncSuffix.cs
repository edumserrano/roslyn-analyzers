namespace TestData.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class NonAsyncMethodWithAsyncSuffix
    {
        public int NonAsyncMethodAsync()
        {
            return 0;
        }
    }
}