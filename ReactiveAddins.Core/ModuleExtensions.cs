using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace ReactiveAddins
{
    public static  class ModuleExtensions
    {
        /// <summary>
        /// <see cref="https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/DependencyInjection/MvcCoreServiceCollectionExtensions.cs"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path"></param>
        /// <param name="predicate"></param>
        public static void AddModules( this IServiceCollection services, string path, Action<IModuleInfo> predicate=null)
        {
            Contract.Requires<ArgumentNullException>(services != null);
            Contract.Requires<ArgumentNullException>(path != null);
            return nul;
        }
    }
}
