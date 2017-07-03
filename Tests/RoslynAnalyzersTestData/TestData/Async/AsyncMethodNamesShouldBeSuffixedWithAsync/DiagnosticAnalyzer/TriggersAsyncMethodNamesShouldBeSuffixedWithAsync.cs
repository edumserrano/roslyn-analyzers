using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class TriggersAsyncMethodNamesShouldBeSuffixedWithAsync
    {
        public async void AsyncVoidMethod()
        {

        }

        public async Task AsyncTaskMethod()
        {

        }

        public async Task<int> AsyncGenericTaskMethod()
        {
            return 1;
        }

        public Task TaskMethod()
        {
            return Task.FromResult(1);
        }

        public Task<int> GenericTaskMethod()
        {
            return Task.FromResult(1);
        }
    }
}