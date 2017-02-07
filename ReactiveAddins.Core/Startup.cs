using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveHelpers.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.Contracts;
using System.IO;

namespace ReactiveAddins
{
    /// <summary>
    /// Represents default web application <c>Startup</c> class. Must be inherited in the derived web applications
    /// in order ExtCore basic functionality to work.
    /// </summary>
    public abstract class Startup:IModuleStartup
    {
        protected IServiceProvider serviceProvider;
        protected IConfigurationRoot configurationRoot;
        protected ILogger<Startup> logger;
        public const string DEFAULT_MODULES_DIRECTORY = "modules";


        /// <summary>
        /// Initializes a new instance of the <see cref="Startup">Startup</see> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider that is used to get different services from the DI.</param>
        public Startup(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Startup>();
            this.configurationRoot = ConfigurationBuilder().Build();
        }

        /// <summary>
        /// Discovers the assemblies, initializes extensions and services that are used by a web application.
        /// Also this method executes prioritized (defined in a specific order) actions (code fragments)
        /// from all the extensions, so each extension may execute some code within this method using the
        /// <c>ConfigureServicesActionsByPriorities</c> property of the <see cref="IExtension">IExtension</see> interface.
        /// </summary>
        /// <param name="services">The current set of the services configured in the DI.</param>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var mvc=services.AddMvcCore(options =>
            {
                //  options.Filters.Add
            });

            services.AddModules(mvc,ModulePath);

            

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Executes prioritized (defined in a specific order) actions (code fragments)
        /// from all the extensions, so each extension may execute some code within this method using the
        /// <c>ConfigureActionsByPriorities</c> property of the <see cref="IExtension">IExtension</see> interface.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public virtual void Configure(IApplicationBuilder applicationBuilder)
        {

        }

        protected virtual IConfigurationBuilder ConfigurationBuilder()
        {
            Contract.Ensures(Contract.Result<IConfigurationBuilder>() != null);
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(this.serviceProvider.GetService<IHostingEnvironment>().ContentRootPath)
            .AddJsonFile("config.json", optional: true, reloadOnChange: true);
            return configurationBuilder;
        }


        protected virtual string ModulePath
        {
            get
            {
                Contract.Ensures(!Contract.Result<string>().IsNullOrEmpty());
                string modulePath = this.configurationRoot?["Modules:Path"] ?? DEFAULT_MODULES_DIRECTORY;
                return Path.Combine(this.serviceProvider.GetService<IHostingEnvironment>().ContentRootPath, modulePath);
            }
        }

        public IConfigurationRoot Configuration  => configurationRoot;
    }
}
