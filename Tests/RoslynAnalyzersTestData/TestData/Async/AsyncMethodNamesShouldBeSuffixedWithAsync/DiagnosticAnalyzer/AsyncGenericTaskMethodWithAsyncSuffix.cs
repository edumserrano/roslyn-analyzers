using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class AsyncGenericTaskMethodWithAsyncSuffix
    {
        public async Task<int> AsyncGenericTaskMethodAsync()
        {
            return 1;
        }
    }
}
