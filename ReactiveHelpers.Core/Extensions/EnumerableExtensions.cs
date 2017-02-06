using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core
{
    public static class EnumerableExtensions
    {
        public static T FirstOrDefault<T>(this IEnumerable< T> @object,T @default)=>@object.FirstOrDefault().IsNull()?@default:@object.FirstOrDefault();
    }
}
