using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Loader;
using System.Reactive.Subjects;

namespace ReactiveCoreAddins
{
    /// <summary>
    /// <see cref="https://github.com/ExtCore/ExtCore"/>
    /// </summary>
    public class AssemblyProvider : ReactiveObject, IAssemblyProvider
    {
        private Dictionary<string, List<Assembly>> _cachedAssemblies = new Dictionary<string, List<Assembly>>();
        private ISubject<bool> directoryChangedSubject;
        private IObservable<bool> directoryChangedObservable;

        private ILogger logger;

        public bool HotPlug { get; set; } = false;

        public IEnumerable<Assembly> GetAssemblies(string path)
        {
            if (_cachedAssemblies.ContainsKey(path)) return _cachedAssemblies[path];


        }

        private IEnumerable<Assembly> GetAssembliesFromPath(string path)
        {
            List<Assembly> assemblies = new List<Assembly>();

            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                this.logger.LogInformation($"Discovering and loading assemblies from path '{path}'");

                foreach (string extensionPath in Directory.EnumerateFiles(path, "*.dll"))
                {
                    Assembly assembly = null;

                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

                        if (this.IsCandidateAssembly(assembly))
                        {
                            assemblies.Add(assembly);
                            this.logger.LogInformation($"Assembly '{assembly.FullName}' is discovered and loaded" );
                        }
                    }

                    catch (Exception e)
                    {
                        this.logger.LogWarning("Error loading assembly '{0}'", extensionPath);
                        this.logger.LogInformation(e.ToString());
                    }
                }
            }

            else
            {
                if (string.IsNullOrEmpty(path))
                    this.logger.LogWarning("Discovering and loading assemblies from path skipped: path not provided");

                else this.logger.LogWarning($"Discovering and loading assemblies from path '{path}' skipped: path not found");
            }

            return assemblies;
        }
    }
}
