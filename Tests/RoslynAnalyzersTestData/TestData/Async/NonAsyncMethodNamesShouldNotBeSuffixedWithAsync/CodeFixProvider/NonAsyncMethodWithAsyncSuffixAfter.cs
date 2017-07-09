namespace TestData.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync.CodeFixProvider
{
    public class NonAsyncMethodWithAsyncSuffix
    {
        public int NonAsyncMethod()
        {
            return 0;
        }
    }
}