using System;
using Mono.Cecil;
using MyWeaver.Fody;

namespace PropertyChangedCore.Fody
{
    public  class PropertyChangedCoreWeaver : BaseWeaver, IWeaver
    {
        


        public PropertyChangedCoreWeaver()
        {
            LogInfo = s => { };
            LogError = s => { };
            LogWarning = s => { };
            LogDebug = s => { };
        }

        public ModuleDefinition ModuleDefinition { get; set; }

        // Will log an MessageImportance.High message to MSBuild. 
        public Action<string> LogInfo { get; set; }

        // Will log an error message to MSBuild. OPTIONAL
        public Action<string> LogError { get; set; }

        public Action<string> LogWarning { get; set; }

        public Action<string> LogDebug { get; set; }

        public IAssemblyResolver AssemblyResolver { get; set; }

        public string AssemblyFilePath { get; set; }

        public  void Execute()
        {
            new ReactivePropertyModule(this).Execute();
            new ObservableAsPropertyModule(this).Execute();
            new ReactiveDependencyPropertyModule(this).Execute();
        }
    }
}
