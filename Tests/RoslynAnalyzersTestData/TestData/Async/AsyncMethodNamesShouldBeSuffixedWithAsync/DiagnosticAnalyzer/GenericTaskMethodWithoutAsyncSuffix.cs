using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class GenericTaskMethodWithoutAsyncSuffix
    {
        public Task<int> GenericTaskMethod()
        {
            return Task.FromResult(1);
        }
    }
}