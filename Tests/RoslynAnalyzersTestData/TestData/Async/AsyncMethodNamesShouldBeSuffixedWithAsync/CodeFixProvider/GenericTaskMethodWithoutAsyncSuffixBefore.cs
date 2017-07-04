using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.CodeFixProvider
{
    public class GenericTaskMethodWithoutAsyncSuffix
    {
        public Task<int> GenericTaskMethod()
        {
            return Task.FromResult(1);
        }
    }
}