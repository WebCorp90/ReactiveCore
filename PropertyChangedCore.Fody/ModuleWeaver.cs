using Mono.Cecil;
using System;

namespace PropertyChangedCore.Fody
{
    public partial class ModuleWeaver : IExecutable
    {
        public const string REACTIVECORE_ASSEMBLY = "ReactiveCore";
        public const string HELPERS= "PropertyChangedCore.Fody.Helpers";
        public const string IREACTIVE_OBJECT = "IReactiveObject";
        public const string REACTIVE_OBJECT = "ReactiveObject";
        public const string IREACTIVE_OBJECT_EXTENTIONS = "IReactiveObjectExtensions";
        public const string RAISE_AND_SET_IF_CHANGE_METHOD = "RaiseAndSetIfChanged";
        public const string REACTIVE_ATTRIBUTE = "ReactiveAttribute";

        public ModuleDefinition ModuleDefinition { get; set; }
         
        // Will log an MessageImportance.High message to MSBuild. 
        public Action<string> LogInfo { get; set; }

        // Will log an error message to MSBuild. OPTIONAL
        public Action<string> LogError { get; set; }

        public Action<string> LogWarning { get;  set; }

        public Action<string> LogDebug { get;  set; }

        public ModuleWeaver()
        {
            LogWarning = s => { };
            LogInfo = s => { };
            LogDebug = s => { };
        }
        public void Execute()
        {
            new ReactiveUIPropertyWeaver(this).Execute();
            new ObservableAsPropertyWeaver(this).Execute();
            new ReactiveDependencyPropertyWeaver(this).Execute();
        }
    }
}
