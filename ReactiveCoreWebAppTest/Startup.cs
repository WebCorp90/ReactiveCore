using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveCoreAddins;
using Microsoft.DotNet.PlatformAbstractions;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace ReactiveCoreWebAppTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IServiceProvider service)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            AssemblyProvider ap = new AssemblyProvider(service);
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!\n");
                var assemblies=await ap.GetAssembliesAsync(@"e:\test");
                foreach (var a in assemblies)
                {
                    await context.Response.WriteAsync($"{a.FullName}\n"); 
                }
                await context.Response.WriteAsync($"{BinPath(env)}\n");
            });
        }
        public string BinPath(IHostingEnvironment env)
        {
            string webRootPath = env.WebRootPath;
            string contentRootPath = env.ContentRootPath;

            return webRootPath + "\n" + contentRootPath;
        }
        
    }
}
