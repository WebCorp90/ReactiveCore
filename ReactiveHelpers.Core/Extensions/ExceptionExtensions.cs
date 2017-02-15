using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveHelpers { 

    public static class ExceptionExtensions
    {
        public static void Dump(this Exception exception)
        {
            try
            {
                exception.StackTrace.Dump();
                exception.Message.Dump();
            }
            catch { }
        }

        public static string ToAllMessages(this Exception exception)
        {
            var sb = new StringBuilder();

            while (exception != null)
            {
                if (!sb.ToString().EmptyNull().Contains(exception.Message))
                {
                    sb.Grow(exception.Message, " * ");
                }
                exception = exception.InnerException;
            }
            return sb.ToString();
        }
    }
}
