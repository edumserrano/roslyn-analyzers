using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class AsyncTaskMethodWithoutAsyncSuffix
    {
        public async Task AsyncTaskMethod()
        {

        }
    }
}