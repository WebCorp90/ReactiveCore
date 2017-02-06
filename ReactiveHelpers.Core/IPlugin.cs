using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core
{
    public interface IPlugin
    {
        IPluginDescriptor Descriptor { get; }

        void Install();

        void Uninstall();
    }
}
