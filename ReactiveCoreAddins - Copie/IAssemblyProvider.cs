using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace ReactiveCoreAddins
{
    /// <summary>
    /// Describes an assembly provider with the mechanism of getting assemblies that should be involved
    /// </summary>
    [ContractClass(typeof(IAssemblyProviderContracts))]
    public interface IAssemblyProvider
    {
        /// <summary>
        /// Discovers and then gets the discovered assemblies.
        /// </summary>
        /// <param name="path">The extensions path of a web application. Might be used or ignored
        /// by an implementation of the <see cref="IAssemblyProvider">IAssemblyProvider</see> interface.</param>
        /// <returns></returns>
        IEnumerable<Assembly> GetAssemblies(string path);
    }

    [ContractClassFor(typeof(IAssemblyProvider))]
    internal sealed class IAssemblyProviderContracts : IAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies(string path)
        {
            Contract.Requires<ArgumentNullException>(path != null);
            Contract.Ensures(Contract.Result<IEnumerable<Assembly>>() != null);
            return new List<Assembly>();
        }
    }
}
