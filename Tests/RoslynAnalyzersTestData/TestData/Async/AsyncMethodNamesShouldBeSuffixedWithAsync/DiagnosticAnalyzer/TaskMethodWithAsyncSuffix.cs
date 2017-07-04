using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class TaskMethodWithAsyncSuffix
    {
        public Task TaskMethodAsync()
        {
            return Task.FromResult(1);
        }
    }
}
