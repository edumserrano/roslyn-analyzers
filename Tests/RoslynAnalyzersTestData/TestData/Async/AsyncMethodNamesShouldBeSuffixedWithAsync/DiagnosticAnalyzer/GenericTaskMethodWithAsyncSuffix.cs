using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class GenericTaskMethodWithAsyncSuffix
    {
        public Task<int> GenericTaskMethodAsync()
        {
            return Task.FromResult(1);
        }
    }
}
