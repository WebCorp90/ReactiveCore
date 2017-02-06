using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core
{
    public static class VersionExtensions
    {
        public static bool IsCompliant(this VersionCompliance thisVersion,Version version)
        {
            Contract.Requires<ArgumentNullException>(thisVersion != null);
            Contract.Requires<ArgumentNullException>(version != null);
            return thisVersion.Min >= version && thisVersion.Max <= version;
        }
    }
}
