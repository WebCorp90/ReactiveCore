using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using System.Globalization;
using System.Dynamic;

namespace ReactiveHelpers.Core.Utilities
{
    public static partial class CommonHelper
    {
       
        public static bool TryConvert<T>(object value, out T convertedValue)
        {
            return TryConvert<T>(value, CultureInfo.InvariantCulture, out convertedValue);
        }

        public static bool TryConvert<T>(object value, CultureInfo culture, out T convertedValue)
        {
            return TryAction<T>(delegate
            {
                return value.Convert<T>(culture);
            }, out convertedValue);
        }

        public static bool TryConvert(object value, Type to, out object convertedValue)
        {
            return TryConvert(value, to, CultureInfo.InvariantCulture, out convertedValue);
        }

        public static bool TryConvert(object value, Type to, CultureInfo culture, out object convertedValue)
        {
            return TryAction<object>(delegate { return value.Convert(to, culture); }, out convertedValue);
        }

        private static bool TryAction<T>(Func<T> func, out T output)
        {
            Contract.Requires<ArgumentNullException>(func != null, nameof(func));

            try
            {
                output = func();
                return true;
            }
            catch
            {
                output = default(T);
                return false;
            }
        }
    }

}
