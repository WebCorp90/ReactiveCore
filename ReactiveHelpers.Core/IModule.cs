#if CORE
using Microsoft.Extensions.DependencyInjection;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core
{
    public interface IModule
    {
        IServiceProvider ConfigureServices(IServiceProvider services);
    }
}
