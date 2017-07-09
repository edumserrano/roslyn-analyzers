namespace TestData.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class NonAsyncMethodWithoutAsyncSuffix
    {
        public int NonAsyncMethod()
        {
            return 0;
        }
    }
}
