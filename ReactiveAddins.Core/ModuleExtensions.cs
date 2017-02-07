using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;
using ReactiveHelpers.Core;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.FileProviders;

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
        public static IServiceCollection AddModules( this IServiceCollection services,string path,IMvcCoreBuilder builder)
        {
            Contract.Requires<ArgumentNullException>(services != null);
            Contract.Requires<ArgumentNullException>(path != null);
            services.AddSingleton<IAssemblyProvider, AssemblyProvider>();
            services.AddSingleton<IModuleManager, ModuleManager>();

            

            var moduleManager = services.GetServiceFromCollection<IModuleManager>();
            moduleManager?.LoadFromPath(path);

            //For this view to be recognized by the web application, we need to include the cshtml files as embedded resources
            //in the class library. Currently, this is done by adding the following setting to the project.json file.
            //"buildOptions": {
            //    "embed": "Views/**/*.cshtml"
            //}

            // For the view, a custom ModuleViewLocationExpander is used to help the view engine lookup up the right module folder the views
            services.Configure<RazorViewEngineOptions>(opts =>
            {
                //opts.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
                foreach (var module in moduleManager.All())
                {
                    opts.FileProviders.Add(new EmbeddedFileProvider(module.Assembly));
                }

            });

            return services;
        }
    }
}
