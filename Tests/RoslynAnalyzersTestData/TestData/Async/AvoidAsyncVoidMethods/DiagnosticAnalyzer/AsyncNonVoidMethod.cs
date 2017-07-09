using System.Threading.Tasks;

namespace TestData.Async.AvoidAsyncVoidMethods.DiagnosticAnalyzer
{
    public class AsyncNonVoidMethod
    {
        private async Task AsyncNonVoid()
        {
            
        }
    }
}