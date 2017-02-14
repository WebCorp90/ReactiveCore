using System;

namespace ReactiveHelpers
{
    public static class ObjectExtensions
    {

        public static bool IsNotNull(this object o)
        {
            return !o.IsNull();
        }

        public static bool IsNull(this object o)
        {
            return o == null;
        }

        public static void ThrowIfNull<T>(this object o, string message, params string[] p) where T : Exception
        {
            if (!o.IsNull()) return;
            Exception ex = Activator.CreateInstance(typeof(T), string.Format(message, p)) as Exception;
            throw ex;
        }

        public static void ThrowIfNotNull<T>(this object o, string message, params string[] p) where T : Exception
        {
            if (o.IsNull()) return;
            Exception ex = Activator.CreateInstance(typeof(T), string.Format(message, p)) as Exception;
            throw ex;
        }

        public static TCAST As<T, TCAST>(this T @object) where TCAST : T=> (TCAST)@object;
       
    }
}
