using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using System.Diagnostics.Contracts;

namespace MyWeaver.Fody
{
    public abstract class Module:IExecutable
    {
        public Module(IWeaver weaver)
        {
            Contract.Requires(weaver != null);
            this.Weaver = weaver;
        }

        public IWeaver Weaver { get; private set; }

        public abstract void Execute();
    }
}
