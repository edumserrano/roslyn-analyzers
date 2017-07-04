using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.CodeFixProvider
{
    public class AsyncGenericTaskMethodWithoutAsyncSuffix
    {
        public async Task<int> AsyncGenericTaskMethod()
        {
            return 1;
        }
    }
}