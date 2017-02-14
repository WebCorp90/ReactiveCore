using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using System.Diagnostics.Contracts;

namespace ReactiveHelpers
{

    public static class Error
    {

        [DebuggerStepThrough]
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args));
        }

        [DebuggerStepThrough]
        public static Exception Application(Exception innerException, string message, params object[] args)
            => new ApplicationException(message.FormatCurrent(args), innerException);


        [DebuggerStepThrough]
        public static Exception ArgumentNullOrEmpty(Func<string> arg)
            => new ArgumentException("String parameter '{0}' cannot be null or all whitespace.", arg.Name());


        [DebuggerStepThrough]
        public static Exception ArgumentNull(string argName) 
            => new ArgumentNullException(argName);

        [DebuggerStepThrough]
        public static Exception ArgumentNull<T>(Func<T> arg)
        {
            var message = "Argument of type '{0}' cannot be null".FormatInvariant(typeof(T));
            return new ArgumentNullException(arg.Name(), message);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange<T>(Func<T> arg)
            => new ArgumentOutOfRangeException(arg.Name());

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName)
            => new ArgumentOutOfRangeException(argName);
        

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
            =>new ArgumentOutOfRangeException(argName, String.Format(CultureInfo.CurrentCulture, message, args));
        

        [DebuggerStepThrough]
        public static Exception Argument(string argName, string message, params object[] args)
            => new ArgumentException(String.Format(CultureInfo.CurrentCulture, message, args), argName);
        

        [DebuggerStepThrough]
        public static Exception Argument<T>(Func<T> arg, string message, params object[] args)
            =>new ArgumentException(message.FormatCurrent(args), arg.Name());
        

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, params object[] args)
        {
            return InvalidOperation(message, null, args);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(message.FormatCurrent(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Func<T> member)
        {
            return InvalidOperation<T>(message, null, member);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Exception innerException, Func<T> member)
        {
            Contract.Requires<ArgumentNullException>(message != null, nameof(message));
            Contract.Requires<ArgumentNullException>(member != null, nameof(member));

            return new InvalidOperationException(message.FormatCurrent(member.Name(), innerException));
        }

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType)
            =>InvalidCast(fromType, toType, null);
        

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType, Exception innerException)
            =>new InvalidCastException("Cannot convert from type '{0}' to '{1}'.".FormatCurrent(fromType.FullName, toType.FullName), innerException);
        

        [DebuggerStepThrough]
        public static Exception NotSupported()
            =>new NotSupportedException();

        [DebuggerStepThrough]
        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName)
            =>new ObjectDisposedException(objectName);

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName, string message, params object[] args)
            =>new ObjectDisposedException(objectName, String.Format(CultureInfo.CurrentCulture, message, args));

        [DebuggerStepThrough]
        public static Exception NoElements()
            => new InvalidOperationException("Sequence contains no elements.");

        [DebuggerStepThrough]
        public static Exception MoreThanOneElement()
            =>new InvalidOperationException("Sequence contains more than one element.");

    }

}
