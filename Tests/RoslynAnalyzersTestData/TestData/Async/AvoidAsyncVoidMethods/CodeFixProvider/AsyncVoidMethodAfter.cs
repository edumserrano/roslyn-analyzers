using System.Threading.Tasks;

namespace TestData.Async.AvoidAsyncVoidMethods.CodeFixProvider
{
    public class AsyncVoidMethod
    {
        private async Task AsyncVoid()
        {
            await Task.Delay(1000);
        }
    }
}