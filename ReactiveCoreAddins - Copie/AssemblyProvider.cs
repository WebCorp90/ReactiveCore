using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Reflection;

namespace ReactiveCoreAddins
{
    public class AssemblyProvider : IAssemblyProvider
    {
        private Dictionary<string, List<Assembly>> _cachedAssemblies=new Dictionary<string, List<Assembly>>();
        private ISubject<bool> changedSubject;
        private IObservable<bool> changedObservable;

        private ILogger logger;
        

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
                            this.logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
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
                    this.logger.LogWarning("Discovering and loading assemblies from path skipped: path not provided", path);

                else this.logger.LogWarning("Discovering and loading assemblies from path '{0}' skipped: path not found", path);
            }

            return assemblies;
        }
    }
}
