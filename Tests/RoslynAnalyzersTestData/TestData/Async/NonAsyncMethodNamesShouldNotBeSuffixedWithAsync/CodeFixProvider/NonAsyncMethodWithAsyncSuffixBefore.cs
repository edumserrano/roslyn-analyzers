namespace TestData.Async.NonAsyncMethodNamesShouldNotBeSuffixedWithAsync.CodeFixProvider
{
    public class NonAsyncMethodWithAsyncSuffix
    {
        public int NonAsyncMethodAsync()
        {
            return 0;
        }
    }
}