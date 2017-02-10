using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using MyWeaver.Fody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUnit.Fody
{
    internal class ReactiveUnitModule : Module
    {
        public ReactiveUnitModule(IWeaver weaver) : base(weaver)
        {
        }


        private CustomAttribute getCustomAttribute(string fullName)
        {
            Weaver.LogInfo($"Number of modules: {Weaver.ModuleDefinition.Assembly.Modules.Count}");
            foreach (ModuleDefinition ModuleDef in Weaver.ModuleDefinition.Assembly.Modules)
            {
                Weaver.LogInfo($"{ModuleDef.FullyQualifiedName}");
                foreach (TypeDefinition TypeDef in ModuleDef.Types)
                {
                    foreach (CustomAttribute CustomAttrib in TypeDef.CustomAttributes)
                    {
                        if (CustomAttrib.AttributeType.FullName == fullName)
                        {
                            return CustomAttrib;
                        }
                    }
                }
            }
            return null;
        }

        public override void Execute()
        {
            Weaver.LogInfo($"{nameof(ReactiveUnitModule)}");

            var runtime = Weaver.ModuleDefinition.AssemblyReferences.Where(x => x.Name == ReactiveUnitWeaver.SYSTEM_RUNTIME_ASSEMBLY).OrderByDescending(x => x.Version).FirstOrDefault();
            if (runtime == null)
            {
                Weaver.LogInfo($"Could not find assembly: {ReactiveUnitWeaver.SYSTEM_RUNTIME_ASSEMBLY} ({ string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) })");
                return;
            }
            Weaver.LogInfo($"{runtime.Name} {runtime.Version}");
            
            var datamemberAttribute = Weaver.ModuleDefinition.FindType(ReactiveUnitWeaver.SYSTEM_RUNTIME_ASSEMBLY, ReactiveUnitWeaver.DATAMEMBER_ATTRIBUTE, runtime);
            if (datamemberAttribute == null)
                throw new Exception($"{ReactiveUnitWeaver.DATAMEMBER_ATTRIBUTE} is null");
            

            var unite = Weaver.ModuleDefinition.AssemblyReferences.Where(x => x.Name == ReactiveUnitWeaver.UNITE_ASSEMBLY).OrderByDescending(x => x.Version).FirstOrDefault();
            if (unite == null)
            {
                Weaver.LogInfo($"Could not find assembly: {ReactiveUnitWeaver.UNITE_ASSEMBLY} ({ string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) })");
                return;
            }
            Weaver.LogInfo($"{unite.Name} {unite.Version}");
            var uniteBaseRef = Weaver.ModuleDefinition.FindType(ReactiveUnitWeaver.UNITE_ASSEMBLY, ReactiveUnitWeaver.UNITE_OBJECT, unite);
            if (uniteBaseRef == null)
                throw new Exception($"{ReactiveUnitWeaver.UNITE_OBJECT} is null");

            var reactiveCore = Weaver.ModuleDefinition.AssemblyReferences.Where(x => x.Name == ReactiveUnitWeaver.REACTIVECORE_ASSEMBLY).OrderByDescending(x => x.Version).FirstOrDefault();
            if (reactiveCore == null)
            {
                Weaver.LogInfo($"Could not find assembly: {ReactiveUnitWeaver.REACTIVECORE_ASSEMBLY} ({ string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) })");
                return;
            }
            Weaver.LogInfo($"{reactiveCore.Name} {reactiveCore.Version}");

           

            var helpers = Weaver.ModuleDefinition.AssemblyReferences.Where(x => x.Name == ReactiveUnitWeaver.HELPERS_ASSEMBLY).OrderByDescending(x => x.Version).FirstOrDefault();
            if (helpers == null)
            {
                Weaver.LogInfo($"Could not find assembly: {ReactiveUnitWeaver.HELPERS_ASSEMBLY} ({  string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) }");
                return;
            }
            Weaver.LogInfo($"{helpers.Name} {helpers.Version}");
            var reactiveObject = new TypeReference(ReactiveUnitWeaver.REACTIVECORE_ASSEMBLY, ReactiveUnitWeaver.REACTIVE_OBJECT, Weaver.ModuleDefinition, reactiveCore);
            var targetTypes = Weaver.ModuleDefinition.GetAllTypes().Where(x => x.BaseType != null && reactiveObject.IsAssignableFrom(x.BaseType)).ToArray();
            Weaver.LogInfo(string.Join<TypeDefinition>(",", targetTypes));
     
            var reactiveAttribute = Weaver.ModuleDefinition.FindType(ReactiveUnitWeaver.HELPERS_ASSEMBLY, ReactiveUnitWeaver.REACTIVE_ATTRIBUTE, helpers);
            if (reactiveAttribute == null)
                throw new Exception($"{ReactiveUnitWeaver.REACTIVE_ATTRIBUTE} is null");

            var datamemberCustomAttribute = getCustomAttribute("DataMemberAttribute");
            if(datamemberCustomAttribute==null)
                throw new Exception($"datamemberCustomAttribute is null");

            // Weaver.ModuleDefinition.GetAllTypes().Where(x=>x.BaseType)
            foreach (var targetType in targetTypes)
            {
                Weaver.LogInfo($"{targetType.Name}");
                var properties = targetType.Properties.Where(  x=> x.IsDefined(datamemberAttribute)  );
                //properties.ToList().ForEach(p => { Weaver.LogInfo($"Property {p.Name} ");p.CustomAttributes.ToList().ForEach(a => Weaver.LogInfo($"{a.AttributeType.Name}")); });
                foreach (var property in properties)
                {
                    Weaver.LogInfo($"Property {property.DeclaringType.FullName}.{property.Name} is treated for DataMember");
                    if (property.SetMethod == null)
                    {
                        Weaver.LogError($"Property {property.DeclaringType.FullName}.{property.Name} has no setter, therefore it is not possible for the property to change, and thus should not be marked with [Reactive]");
                        continue;
                    }
                    //if(property.PropertyType.IsAssignableFrom)
                    

                    /*// Declare a field to store the property value
                    var field = new FieldDefinition($"${property.Name}", FieldAttributes.Private, property.PropertyType);
                    targetType.Fields.Add(field);

                    // Remove old field (the generated backing field for the auto property)
                    var oldField = (FieldReference)property.GetMethod.Body.Instructions.Where(x => x.Operand is FieldReference).Single().Operand;
                    var oldFieldDefinition = oldField.Resolve();
                    targetType.Fields.Remove(oldFieldDefinition);

                    // See if there exists an initializer for the auto-property
                    var constructors = targetType.Methods.Where(x => x.IsConstructor);
                    foreach (var constructor in constructors)
                    {
                        var fieldAssignment = constructor.Body.Instructions.SingleOrDefault(x => Equals(x.Operand, oldFieldDefinition) || Equals(x.Operand, oldField));
                        if (fieldAssignment != null)
                        {
                            // Replace field assignment with a property set (the stack semantics are the same for both, 
                            // so happily we don't have to manipulate the bytecode any further.)
                            var setterCall = constructor.Body.GetILProcessor().Create(property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, property.SetMethod);
                            constructor.Body.GetILProcessor().Replace(fieldAssignment, setterCall);
                        }
                    }*/
                    
                    property.CustomAttributes.Remove(datamemberCustomAttribute);
                    property.GetMethod.Body. Emit(il=> { });
                    /*
                    // Build out the getter which simply returns the value of the generated field
                    property.GetMethod.Body = new MethodBody(property.GetMethod);
                    property.GetMethod.Body.Emit(il =>
                    {
                        il.Emit(OpCodes.Ldarg_0);                                   // this
                        il.Emit(OpCodes.Ldfld, field.BindDefinition(targetType));   // pop -> this.$PropertyName
                        il.Emit(OpCodes.Ret);                                       // Return the field value that is lying on the stack
                    });

                    TypeReference genericTargetType = targetType;
                    if (targetType.HasGenericParameters)
                    {
                        var genericDeclaration = new GenericInstanceType(targetType);
                        foreach (var parameter in targetType.GenericParameters)
                        {
                            genericDeclaration.GenericArguments.Add(parameter);
                        }
                        genericTargetType = genericDeclaration;
                    }

                    var methodReference = raiseAndSetIfChangedMethod.MakeGenericMethod(genericTargetType, property.PropertyType);

                    // Build out the setter which fires the RaiseAndSetIfChanged method
                    if (property.SetMethod == null)
                    {
                        throw new Exception($"[{PropertyChangedCoreWeaver.REACTIVE_ATTRIBUTE.Replace("Attribute", "")}] is decorating { property.DeclaringType.FullName}.{property.Name} , but the property has no setter so there would be nothing to react to.  Consider removing the attribute.");
                    }
                    property.SetMethod.Body = new MethodBody(property.SetMethod);
                    property.SetMethod.Body.Emit(il =>
                    {
                        il.Emit(OpCodes.Ldarg_0);                                   // this
                        il.Emit(OpCodes.Ldarg_0);                                   // this
                        il.Emit(OpCodes.Ldflda, field.BindDefinition(targetType));  // pop -> this.$PropertyName
                        il.Emit(OpCodes.Ldarg_1);                                   // value
                        il.Emit(OpCodes.Ldstr, property.Name);                      // "PropertyName"
                        il.Emit(OpCodes.Call, methodReference);                     // pop * 4 -> this.RaiseAndSetIfChanged(this.$PropertyName, value, "PropertyName")
                        il.Emit(OpCodes.Pop);                                       // We don't care about the result of RaiseAndSetIfChanged, so pop it off the stack (stack is now empty)
                        il.Emit(OpCodes.Ret);                                       // Return out of the function
                    });*/
                }
            }
        }
    }
}
