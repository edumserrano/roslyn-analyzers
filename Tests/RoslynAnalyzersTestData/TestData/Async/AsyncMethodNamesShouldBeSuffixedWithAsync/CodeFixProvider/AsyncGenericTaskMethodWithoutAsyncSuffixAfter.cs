using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.CodeFixProvider
{
    public class AsyncGenericTaskMethodWithoutAsyncSuffix
    {
        public async Task<int> AsyncGenericTaskMethodAsync()
        {
            return 1;
        }
    }
}