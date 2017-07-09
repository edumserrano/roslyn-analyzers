using System.Threading.Tasks;

namespace TestData.Async.AvoidAsyncVoidMethods.CodeFixProvider
{
    public class AsyncVoidMethod
    {
        private async void AsyncVoid()
        {
            await Task.Delay(1000);
        }
    }
}