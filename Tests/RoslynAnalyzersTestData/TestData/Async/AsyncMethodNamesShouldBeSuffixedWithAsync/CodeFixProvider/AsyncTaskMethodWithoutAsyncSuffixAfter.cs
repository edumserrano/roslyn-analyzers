using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.CodeFixProvider
{
    public class AsyncTaskMethodWithoutAsyncSuffix
    {
        public async Task AsyncTaskMethodAsync()
        {

        }
    }
}