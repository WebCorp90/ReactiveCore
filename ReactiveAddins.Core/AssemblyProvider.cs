using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Loader;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using ReactiveCore;
using Microsoft.Extensions.DependencyModel;
using PropertyChangedCore.Helpers;
using ReactiveHelpers.Core;
using ReactiveHelpers.Core.FsWatcher;
using System.Threading.Tasks;

namespace ReactiveAddins
{
    /// <summary>
    /// <see cref="https://github.com/ExtCore/ExtCore"/>
    /// </summary>
    public class AssemblyProvider : ReactiveObject, IAssemblyProvider, IDisposable
    {
        private string _path;
        private List<Assembly> _cachedAssemblies = new List<Assembly>();
        private ISubject<List<Assembly>> directoryChangedSubject;
        private IObservable<List<Assembly>> directoryChangedObservable;

        private ISubject<bool> hotplugSubject;
        private IObservable<bool> hotpluggedObservable;

        private IObservableFileSystemWatcher _fsWatcher;

        protected readonly ILogger<AssemblyProvider> logger;

        [Reactive]
        public bool HotPlug { get; set; } = false;

        public const string PATH_ALLREADY_SET = "PATH_ALLREADY_SET";

        public AssemblyProvider(IServiceProvider serviceProvider)
        {
            this.logger = (serviceProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory).CreateLogger<AssemblyProvider>();
            this.IsCandidateAssembly = assembly => !assembly.FullName.StartsWith("Microsoft.") && !assembly.FullName.StartsWith("System.");
            this.IsCandidateCompilationLibrary = library => library.Name != "NETStandard.Library" && !library.Name.StartsWith("Microsoft.") && !library.Name.StartsWith("System.");

            this.directoryChangedSubject = new Subject<List<Assembly>>();
            this.hotplugSubject = new Subject<bool>();

            this.directoryChangedObservable = directoryChangedSubject.AsObservable();
            this.hotpluggedObservable = hotplugSubject.AsObservable();


            this.Changed.Where(e => e.PropertyName.Equals(HotPlug)).Subscribe(e =>
              {
                  hotplugSubject.OnNext(HotPlug);
                  var _ = HotPlug ? startScan() : stopScan();
              });
        }

        public IEnumerable<Assembly> GetAssemblies(string path)
        {
            _path.ThrowIfNotNull<ArgumentException>(PATH_ALLREADY_SET);
            _path = path;

            if (_cachedAssemblies.Count == 0) GetAssemblies();
            return _cachedAssemblies;

        }



        

        private IEnumerable<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.AddRange(this.GetAssembliesFromPath(_path));
            assemblies.AddRange(this.GetAssembliesFromDependencyContext());
            _cachedAssemblies = assemblies;

            return assemblies;
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
                            this.logger.LogInformation($"Assembly '{assembly.FullName}' is discovered and loaded");
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


        private IEnumerable<Assembly> GetAssembliesFromDependencyContext()
        {
            List<Assembly> assemblies = new List<Assembly>();

            this.logger.LogInformation("Discovering and loading assemblies from DependencyContext");

            foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
            {
                if (this.IsCandidateCompilationLibrary(compilationLibrary))
                {
                    Assembly assembly = null;

                    try
                    {
                        assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
                        assemblies.Add(assembly);
                        this.logger.LogInformation($"Assembly '{assembly.FullName}' is discovered and loaded");
                    }

                    catch (Exception e)
                    {
                        this.logger.LogWarning($"Error loading assembly '{compilationLibrary.Name}'");
                        this.logger.LogInformation(e.ToString());
                    }
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Gets or sets the predicate that is used to filter discovered assemblies from a specific folder
        /// before thay have been added to the resulting assemblies set.
        /// </summary>
        public Func<Assembly, bool> IsCandidateAssembly { get; set; }

        /// <summary>
        /// Gets or sets the predicate that is used to filter discovered libraries from a web application dependencies
        /// before thay have been added to the resulting assemblies set.
        /// </summary>
        public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

        private bool startScan()
        {
            _fsWatcher.Start();
            return true;
        }

        private bool stopScan()
        {
            _fsWatcher.Stop();
            return true;
        }

        public void Dispose()
        {
            _fsWatcher.Dispose();
        }


        private void Reload()
        {
            Task.Run(() =>
            {
                GetAssemblies();
            }).ContinueWith((t) =>
            {
                directoryChangedSubject.OnNext(_cachedAssemblies);
            });
            
        }
        #region events
        /// <summary>
        /// Represents an Observable that fires *after* file system changed.
        /// </summary>
        public IObservable<bool> HotPlugged => hotpluggedObservable;

        public IObservableFileSystemWatcher DirectoryWatcher
        {
            get
            {
                if (_fsWatcher.IsNull())
                {
                    Action<FileSystemWatcher> configure = c => { c.Path = _path; c.Filter = "*.dll"; c.IncludeSubdirectories = false; };
                    _fsWatcher = new ObservableFileSystemWatcher(configure);
                    _fsWatcher.Created.Subscribe(e => { Reload(); });
                    _fsWatcher.Changed.Subscribe(e => { Reload(); });
                    _fsWatcher.Deleted.Subscribe(e => { Reload(); });

                }
                return _fsWatcher;
            }

        }
        #endregion
    }
}
