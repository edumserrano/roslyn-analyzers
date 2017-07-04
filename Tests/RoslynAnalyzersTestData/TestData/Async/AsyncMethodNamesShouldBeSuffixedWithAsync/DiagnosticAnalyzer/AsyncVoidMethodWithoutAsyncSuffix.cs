namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class AsyncVoidMethodWithoutAsyncSuffix
    {
        public async void AsyncVoidMethod()
        {

        }
    }
}