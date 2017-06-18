namespace Analyzers.Extensions
{
    internal static class ObjectExtensions
    {
        public static long ToInt64(this object o)
        {
            return o is ulong ? unchecked((long)(ulong)o) : System.Convert.ToInt64(o);
        }
    }
}
