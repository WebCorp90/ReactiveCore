using MyWeaver.Fody;
using System;
using Mono.Cecil;

namespace ReactiveUnit.Fody
{
    public class ReactiveUnitWeaver : BaseWeaver,IWeaver
    {
        public ReactiveUnitWeaver():base()
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
            new ReactiveUnitModule(this).Execute();
        }
    }
}
