using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.CodeFixProvider
{
    public class TaskMethodWithoutAsyncSuffix
    {
        public Task TaskMethodAsync()
        {
            return Task.FromResult(1);
        }
    }
}