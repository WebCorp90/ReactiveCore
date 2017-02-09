#if CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace ReactiveHelpers.Core
{

    public static class ServiceExtensions
    {
        public  static T GetServiceFromCollection<T>(this IServiceCollection services)
        {
            Contract.Requires<ArgumentNullException>(services != null);
            return (T)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

       // public static T GetService<T>(this IServiceProvider services) => (T)services.GetService(typeof(T));
    }
}
#endif