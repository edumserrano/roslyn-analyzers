using System.Threading.Tasks;

namespace TestData.Async.AsyncMethodNamesShouldBeSuffixedWithAsync.DiagnosticAnalyzer
{
    public class TaskMethodWithoutAsyncSuffix
    {
        public Task TaskMethod()
        {
            return Task.FromResult(1);
        }
    }
}