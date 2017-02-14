using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace ReactiveHelpers
{
    public static  class TypeExtensions
    {
#if CORE
        public static bool IsGenericType(this Type t)
        {
            Contract.Requires<ArgumentNullException>(t != null, nameof(t));
            return t.IsConstructedGenericType;
        }

        public static Type[] GetGenericArguments(this Type t)
        {
            Contract.Requires<ArgumentNullException>(t != null, nameof(t));
            return t.GenericTypeArguments;

        }
        public static bool IsAssignableFrom(this Type t,Type c)
        {
            Contract.Requires<ArgumentNullException>(t != null, nameof(t));
            Contract.Requires<ArgumentNullException>(c != null, nameof(c));
            return t.GetTypeInfo().IsAssignableFrom(c);
        }

        public static bool IsEnum(this Type t)
        {
            Contract.Requires<ArgumentNullException>(t != null, nameof(t));
            return t.GetTypeInfo().IsEnum;
        }

#endif

    }
}
