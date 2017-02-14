#if CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{

    public class ApplicationException : Exception
    {
        const int Result = unchecked((int)0x80131600);

        // Constructors
        public ApplicationException()
            : base("An application exception has occurred.")
        {
            HResult = Result;
        }

        public ApplicationException(string message)
            : base(message)
        {
            HResult = Result;
        }

        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
            HResult = Result;
        }

   
    }
}
#endif