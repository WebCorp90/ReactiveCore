using Microsoft.Extensions.DependencyModel;
using ReactiveHelpers.Core.FsWatcher;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ReactiveAddins
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

        //// <summary>
        /// Gets or sets the predicate that is used to filter discovered assemblies from a specific folder
        /// before thay have been added to the resulting assemblies set.
        /// </summary>
        Func<Assembly, bool> IsCandidateAssembly { get; set; }

        /// <summary>
        /// Gets or sets the predicate that is used to filter discovered libraries from a web application dependencies
        /// before thay have been added to the resulting assemblies set.
        /// </summary>
        Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

        IObservableFileSystemWatcher  DirectoryWatcher{get;}
    }

    [ContractClassFor(typeof(IAssemblyProvider))]
    internal sealed class IAssemblyProviderContracts : IAssemblyProvider
    {
        public IObservableFileSystemWatcher DirectoryWatcher
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservableFileSystemWatcher>() != null);
                return default(IObservableFileSystemWatcher);
            }
            
        }

        public Func<Assembly, bool> IsCandidateAssembly
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<Assembly, bool>>() != null);
                return default(Func<Assembly, bool>);
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
            }
        }

        public Func<Library, bool> IsCandidateCompilationLibrary
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<Assembly, bool>>() != null);
                return default(Func<Library, bool>);
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
            }
        }

        public IEnumerable<Assembly> GetAssemblies(string path)
        {
            Contract.Requires<ArgumentNullException>(path != null);
            Contract.Ensures(Contract.Result<IEnumerable<Assembly>>() != null);
            return default(IEnumerable<Assembly>);
        }
    }
}
