using System.Threading.Tasks;

namespace TestData.Async.UseConfigureAwaitFalse.CodeFixProvider
{
    public class AsyncMethodCall
    {
        public async Task MethodWithAsyncMethodCall()
        {
            await Task.Delay(1000);
        }
    }
}