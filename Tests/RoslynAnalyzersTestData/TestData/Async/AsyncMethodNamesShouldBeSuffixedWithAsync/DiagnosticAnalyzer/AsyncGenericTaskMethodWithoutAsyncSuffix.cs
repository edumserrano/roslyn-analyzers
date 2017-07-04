using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class AsyncGenericTaskMethodWithoutAsyncSuffix
    {
        public async Task<int> AsyncGenericTaskMethod()
        {
            return 1;
        }
    }
}