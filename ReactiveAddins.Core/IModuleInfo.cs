using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace ReactiveAddins
{
    [ContractClass(typeof(IModuleInfoContract))]
    public interface IModuleInfo
    {
        string Name { get; }

        string ShortName { get; }

        Assembly Assembly { get; }

        string Path { get; }
    }

    [ContractClassFor(typeof(IModuleInfo))]
    internal sealed class IModuleInfoContract : IModuleInfo
    {
        public Assembly Assembly
        {
            get
            {
                Contract.Ensures(Contract.Result<Assembly>() != null);
                return default(Assembly);
            }
        }

        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }

        public string Path
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }

        public string ShortName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }
    }
}