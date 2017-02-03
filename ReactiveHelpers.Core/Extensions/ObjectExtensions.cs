using System;

namespace ReactiveHelpers.Core
{
    public static class ObjectExtensions
    {
        public static bool isNull(this object o)
        {
            return o == null;
        }

        public static void ThrowIfNull<T>(this object o,string message)where T:Exception
        {
            if (!o.isNull()) return;
            Exception ex= Activator.CreateInstance(typeof(T), message) as Exception;
            throw ex;
        }
    }
}
