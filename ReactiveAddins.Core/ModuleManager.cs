using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReactiveHelpers.Core;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ReactiveAddins
{
    public class ModuleManager : IModuleManager
    {
        private const string MODULE_ALLREADY_REGISTERED = "Module {0} allready registered in path {1}. Check name of your plugin";
        private readonly List<IModuleInfo> _modules;
       
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly IServiceProvider _services;

        public ModuleManager(ILogger logger,IAssemblyProvider assemblyProvider,IServiceProvider services)
        {
            _modules = new List<IModuleInfo>();
            _assemblyProvider = assemblyProvider;
            _services = services;
        }

        public void AddModule(IModuleInfo module)
        {
            GetByName(module.Name).ThrowIfNotNull<ArgumentException>(MODULE_ALLREADY_REGISTERED, module.Name, module.Path);
            _modules.Add(module);
        }

        public IEnumerable<IModuleInfo> All(Func<IModuleInfo,bool> predicate=null)=> predicate.IsNull() ? _modules : _modules.Where(predicate);
 
        public IModuleInfo GetByName(string name)=> _modules.Where(e => e.Name.Equals(name)).FirstOrDefault(ModuleInfo.Empty);

        public void LoadFromPath(string path)
        {
            _modules.AddRange( Candidate( _assemblyProvider.GetAssemblies(path)));
        }

        public void Not(Func<IModuleInfo, bool> predicate, Action<IModuleInfo> action)
        {
            _modules.Except(All(predicate)).ToList().ForEach(action);
        }

        private IEnumerable<IModuleInfo> Candidate(IEnumerable<IModuleInfo> modules)
        {
            foreach (var module in modules)
            {
                var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModule).IsAssignableFrom(x)).FirstOrDefault();
                if (moduleInitializerType != null && moduleInitializerType != typeof(IModule))
                {
                    var moduleInitializer = (IModule)Activator.CreateInstance(moduleInitializerType);
                    moduleInitializer.ConfigureServices(_services);
                    module.Candidate = true;
                    yield return module;
                }
                else
                {
                    module.Candidate = false;
                }
                
            }
            
        }

    }
}
