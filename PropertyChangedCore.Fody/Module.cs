using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using System.Diagnostics.Contracts;

namespace PropertyChangedCore.Fody
{
    public abstract class Module:IExecutable
    {
        public Module(ModuleWeaver weaver)
        {
            Contract.Requires(weaver != null);
            this.Weaver = weaver;
        }

        public ModuleWeaver Weaver { get; private set; }

        public abstract void Execute();
    }
}
