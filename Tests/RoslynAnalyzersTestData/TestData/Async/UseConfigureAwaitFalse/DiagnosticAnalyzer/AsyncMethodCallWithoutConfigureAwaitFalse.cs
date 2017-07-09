using System.Threading.Tasks;

namespace TestData.Async.UseConfigureAwaitFalse.DiagnosticAnalyzer
{
    public class AsyncMethodCallWithoutConfigureAwaitFalse
    {
        public async Task MethodWithoutAsyncMethodCall()
        {
            await Task.Delay(1000);
        }
    }
}
