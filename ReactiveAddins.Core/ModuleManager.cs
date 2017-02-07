using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReactiveHelpers.Core;
using System.Reflection;

namespace ReactiveAddins
{
    public class ModuleManager : IModuleManager
    {
        private const string MODULE_ALLREADY_REGISTERED = "Module {0} allready registered in path {1}. Check name of your plugin";
        private readonly List<IModuleInfo> _modules;
       
        private readonly IAssemblyProvider _assemblyProvider;

        public ModuleManager(ILogger logger,IAssemblyProvider assemblyProvider)
        {
            _modules = new List<IModuleInfo>();
            _assemblyProvider = assemblyProvider;
        }

        public void AddModule(IModuleInfo module)
        {
            GetByName(module.Name).ThrowIfNotNull<ArgumentException>(MODULE_ALLREADY_REGISTERED, module.Name, module.Path);
            _modules.Add(module);
        }

        public IEnumerable<IModuleInfo> All(Func<IModuleInfo,bool> predicate)=> predicate.IsNull() ? _modules : _modules.Where(predicate);
 
        public IModuleInfo GetByName(string name)=> _modules.Where(e => e.Name.Equals(name)).FirstOrDefault(ModuleInfo.Empty);

        public void LoadFromPath(string path)
        {
            _modules.AddRange( createModuleInfo( _assemblyProvider.GetAssemblies(path)));
        }

        private IEnumerable<IModuleInfo> createModuleInfo(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                yield return new ModuleInfo(assembly.FullName, "", assembly);
            }
            
        }
    }
}
