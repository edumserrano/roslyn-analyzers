using System.Threading.Tasks;

namespace TestData.Async.UseConfigureAwaitFalse.DiagnosticAnalyzer
{
    public class AsyncMethodCallWithConfigureAwaitFalse
    {
        public async Task MethodWithAsyncMethodCall()
        {
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}