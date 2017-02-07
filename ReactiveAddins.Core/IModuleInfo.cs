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

        bool Candidate { get; set; }
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

        public bool Candidate
        {
            get
            {
                
                return default(bool);
            }
            set { }
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