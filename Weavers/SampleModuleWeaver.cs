using Mono.Cecil;
using Mono.Cecil.Cil;
using MyWeaver.Fody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Webcorp.unite;

namespace PropertyChangedCore.Fody
{

    /// <summary>
    ///  Sample module to copy to make Weaver
    /// </summary>
    public class SampleModuleWeaver:IWeaver
    {
        public SampleModuleWeaver()
        {
            LogDebug = m => { };
            LogInfo = m => { };
            LogWarning = m => { };
            LogWarningPoint = (m, p) => { };
            LogError = m => { };
            LogErrorPoint = (m, p) => { };
        }

        public Action<string> LogDebug { get; set; }
        public Action<string> LogInfo { get; set; }
        public Action<string> LogWarning { get; set; }
        public Action<string, SequencePoint> LogWarningPoint { get; set; }
        public Action<string> LogError { get; set; }
        public Action<string, SequencePoint> LogErrorPoint { get; set; }

        // An instance of Mono.Cecil.IAssemblyResolver for resolving assembly references. OPTIONAL
        public IAssemblyResolver AssemblyResolver { get; set; }

        // An instance of Mono.Cecil.ModuleDefinition for processing. REQUIRED
        public ModuleDefinition ModuleDefinition { get; set; }

        // Will contain the full path of the target assembly. OPTIONAL
        public string AssemblyFilePath { get; set; }

        public  void Execute()
        {
            LogInfo("Module running");
            new DataMemberModule(this).Execute();

        }

        public void AfterWeaving()
        {
            LogInfo("Module Weaved");
        }


    }

    public class DataMemberModule : Module
    {
        readonly TypeReference dataMemberRef;
        readonly TypeDefinition dataMemberDef;

        readonly TypeReference unitRef;
        public DataMemberModule(IWeaver weaver) : base(weaver)
        {
            dataMemberRef = ModuleDefinition.Import(typeof(DataMemberAttribute));
            dataMemberDef = dataMemberRef.Resolve();

            unitRef = ModuleDefinition.Import(typeof(IUnit));
        }

        public override void Execute()
        {
            LogInfo($"{nameof(DataMemberModule)} start execution");
            //Weaver.LogInfo(dataMemberDef.FullName);

            //Weaver.ModuleDefinition.Types.ToList().ForEach(x => Weaver.LogInfo(x.FullName));

            GetDataMemberProperties().ToList().ForEach(m => {
                LogInfo($"{m.FullName}");
               // createDataMemberPath(m);
            });

            LogInfo($"{nameof(DataMemberModule)} ended");
        }

        IEnumerable<PropertyDefinition> GetDataMemberProperties()
        {
            return Weaver.ModuleDefinition.Types.SelectMany(x => x.Properties.Where(ContainsDataMemberAttribute));
        }

        bool ContainsDataMemberAttribute(PropertyDefinition property)
        {
            
            //LogInfo(property.PropertyType.FullName);
            var isUnit=unitRef.IsAssignableFrom(property.PropertyType);
           // LogInfo($"{property.FullName }" + (isUnit ? " est une unite" : "n est pas une unite"));
             var dataMemberAttr = property.CustomAttributes.Where(x => x.Constructor.DeclaringType.FullName == dataMemberDef.FullName).FirstOrDefault();
            if (isUnit && dataMemberAttr != null)
            {
                property.CustomAttributes.Remove(dataMemberAttr);
                
            }
            return dataMemberAttr != null;
        }

        
        bool createDataMemberPath(PropertyDefinition property)
        {
            LogInfo($"createDataMemberPath on class { property.DeclaringType}");
            string propName = $"_{property.Name}";
            PropertyDefinition newProperty = new PropertyDefinition(propName, PropertyAttributes.None, property.PropertyType);
            //newProperty.CustomAttributes.Add(new CustomAttribute())
            MethodDefinition get = new MethodDefinition($"get_{propName}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, property.PropertyType);
           // get.Body.Instructions.Add(Instruction.Create()

            newProperty.GetMethod = get;
            property.DeclaringType.Properties.Add(newProperty);
            return true;
        }
    }
}
