using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using System.Diagnostics.Contracts;

namespace MyWeaver.Fody
{
    public abstract class Module:IExecutable,IWeaver
    {
        public Module(IWeaver weaver)
        {
            Contract.Requires(weaver != null);
            this.Weaver = weaver;
        }

        public string AssemblyFilePath
        {
            get
            {
                return Weaver.AssemblyFilePath;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IAssemblyResolver AssemblyResolver
        {
            get
            {
                return Weaver.AssemblyResolver;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<string> LogDebug
        {
            get
            {
                return Weaver.LogDebug;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<string> LogError
        {
            get
            {
                return Weaver.LogError;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<string> LogInfo
        {
            get
            {
                return Weaver.LogInfo;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<string> LogWarning
        {
            get
            {
                return Weaver.LogWarning;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ModuleDefinition ModuleDefinition
        {
            get
            {
                return Weaver.ModuleDefinition;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IWeaver Weaver { get; private set; }

        public abstract void Execute();
    }
}
