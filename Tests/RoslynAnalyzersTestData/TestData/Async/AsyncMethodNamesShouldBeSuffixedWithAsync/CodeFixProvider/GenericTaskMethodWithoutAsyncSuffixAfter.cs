using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.CodeFixProvider
{
    public class GenericTaskMethodWithoutAsyncSuffix
    {
        public Task<int> GenericTaskMethodAsync()
        {
            return Task.FromResult(1);
        }
    }
}