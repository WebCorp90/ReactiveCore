using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;
using ReactiveHelpers.Core;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using System.Linq;

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
        public static IServiceCollection AddModules( this IServiceCollection services,IMvcCoreBuilder builder, string path, Func<IModuleInfo, bool> predicate=null)
        {
            Contract.Requires<ArgumentNullException>(services != null);
            Contract.Requires<ArgumentNullException>(path != null);
            services.AddSingleton<IAssemblyProvider, AssemblyProvider>();
            services.AddSingleton<IModuleManager, ModuleManager>();

            // For the view, a custom ModuleViewLocationExpander is used to help the view engine lookup up the right module folder the views
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
            });

            var moduleManager = services.GetServiceFromCollection<IModuleManager>();
            moduleManager?.LoadFromPath(path);

            var moduleInitializerInterface = typeof(IModuleInitializer);
            foreach (var module in moduleManager.All(predicate))
            {
                // Register controller from modules
                builder.AddApplicationPart(module.Assembly);

                // Register dependency in modules
                var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModuleInitializer).IsAssignableFrom(x)).FirstOrDefault();
                if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer))
                {
                    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    moduleInitializer.ConfigureServices(services);
                }
            }
            return services;
        }
    }
}
