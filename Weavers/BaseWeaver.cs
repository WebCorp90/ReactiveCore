using Mono.Cecil;
using System;

namespace MyWeaver.Fody
{
    public abstract class BaseWeaver
    {
        public const string REACTIVECORE_ASSEMBLY = "ReactiveCore";
        public const string UNITE_ASSEMBLY = "ReactiveUnits";
        public const string UNITE_OBJECT = "Unit<>";
        public const string SYSTEM_RUNTIME_ASSEMBLY = "System.Runtime.Serialization";
        public const string HELPERS_ASSEMBLY = "ReactiveHelpers";
        public const string IREACTIVE_OBJECT = "IReactiveObject";
        public const string REACTIVE_OBJECT = "ReactiveObject";
        public const string IREACTIVE_OBJECT_EXTENTIONS = "IReactiveObjectExtensions";
        public const string RAISE_AND_SET_IF_CHANGE_METHOD = "RaiseAndSetIfChanged";
        public const string REACTIVE_ATTRIBUTE = "ReactiveAttribute";
        public const string OBSERVABLE_AS_PROPERTY_ATTRIBUTE = " ObservableAsPropertyAttribute";
        public const string DATAMEMBER_ATTRIBUTE = "DataMemberAttribute";

        

        
    }
}
