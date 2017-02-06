using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ReactiveAddins
{
    public sealed class ModuleInfo : IModuleInfo
    {
        public static IModuleInfo Empty = new ModuleInfo("", "");
        public ModuleInfo(string name, string path) : this(name, path, Assembly.GetEntryAssembly())
        {

        }
        public ModuleInfo(string name, string path, Assembly assembly)
        {
            this.Name = name;
            this.Path = path;
            this.Assembly = assembly;
        }
        public Assembly Assembly { get; private set; }

        public string Name { get; private set; }

        public string Path { get; private set; }

        public string ShortName => Name.Split('.').Last();


    }
}
