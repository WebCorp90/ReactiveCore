using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System.Diagnostics;
using MyWeaver.Fody;
using ReactiveHelpers.Core;

namespace PropertyChangedCore.Fody
{
    /// <summary>
    /// Weaver that replaces properties marked with `[DataMember]` on subclasses of `ReactiveObject` with an 
    /// implementation that invokes `RaisePropertyChanged` as is required for reaciveui.
    /// </summary>
    internal class ReactivePropertyModule : Module
    {
        

        public ReactivePropertyModule(PropertyChangedCoreWeaver module):base(module)
        {

        }

        public override void Execute()
        {
            Weaver.LogInfo($"{nameof( ReactivePropertyModule)}");
#if DEBUG
            if (Debugger.IsAttached) Debugger.Break();
#endif
            var reactiveCore = Weaver.ModuleDefinition.AssemblyReferences.Where(x => x.Name == PropertyChangedCoreWeaver.REACTIVECORE_ASSEMBLY).OrderByDescending(x => x.Version).FirstOrDefault();
            if (reactiveCore == null)
            {
                Weaver.LogInfo($"Could not find assembly: {PropertyChangedCoreWeaver.REACTIVECORE_ASSEMBLY} ({ string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) })");
                return;
            }
            Weaver.LogInfo($"{reactiveCore.Name} {reactiveCore.Version}");
            var helpers = Weaver.ModuleDefinition.AssemblyReferences.Where(x => x.Name == PropertyChangedCoreWeaver.HELPERS_ASSEMBLY).OrderByDescending(x => x.Version).FirstOrDefault();
            if (helpers == null)
            {
                Weaver.LogInfo($"Could not find assembly: {PropertyChangedCoreWeaver.HELPERS_ASSEMBLY} ({  string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) }");
                return;
            }
            Weaver.LogInfo($"{helpers.Name} {helpers.Version}");
            var reactiveObject = new TypeReference(PropertyChangedCoreWeaver.REACTIVECORE_ASSEMBLY, PropertyChangedCoreWeaver.REACTIVE_OBJECT, Weaver.ModuleDefinition, reactiveCore);
            var targetTypes = Weaver.ModuleDefinition.GetAllTypes().Where(x => x.BaseType != null && reactiveObject.IsAssignableFrom(x.BaseType)).ToArray();
            //Weaver.LogInfo(string.Join<TypeDefinition>(",", targetTypes));
            var reactiveObjectExtensions = new TypeReference(PropertyChangedCoreWeaver.REACTIVECORE_ASSEMBLY, PropertyChangedCoreWeaver.IREACTIVE_OBJECT_EXTENTIONS, Weaver.ModuleDefinition, reactiveCore).Resolve();
            if (reactiveObjectExtensions == null)
                throw new Exception($"{PropertyChangedCoreWeaver.IREACTIVE_OBJECT_EXTENTIONS} is null");

            var raiseAndSetIfChangedMethod = Weaver.ModuleDefinition.Import(reactiveObjectExtensions.Methods.Single(x => x.Name == PropertyChangedCoreWeaver.RAISE_AND_SET_IF_CHANGE_METHOD));
            if (raiseAndSetIfChangedMethod == null)
                throw new Exception($"{PropertyChangedCoreWeaver.RAISE_AND_SET_IF_CHANGE_METHOD} is null");

            var reactiveAttribute =ModuleDefinition.Import(typeof(ReactiveAttribute));
           // dataMemberDef = dataMemberRef.Resolve();

            //var reactiveAttribute = Weaver.ModuleDefinition.FindType(PropertyChangedCoreWeaver.HELPERS_ASSEMBLY,  PropertyChangedCoreWeaver.REACTIVE_ATTRIBUTE , helpers);
            if (reactiveAttribute == null)
                throw new Exception($"{PropertyChangedCoreWeaver.REACTIVE_ATTRIBUTE} is null");
            LogInfo($" REactive Attribute fullname {reactiveAttribute.FullName}");
            foreach (var targetType in targetTypes)
            {
                targetTypes.ToList().ForEach(e => targetType.Properties.ToList().ForEach(p=> LogInfo(p.FullName)));
                var properties = targetType.Properties.Where(x => x.IsDefined(reactiveAttribute)).ToArray();
                foreach (var property in properties)
                {
                    LogInfo(property.Name);
                   /* if(property.CustomAttributes.Where(x => { LogInfo(x.Constructor.DeclaringType.FullName);  return x.Constructor.DeclaringType.FullName == reactiveAttribute.FullName; }).FirstOrDefault() == null)
                    {
                        LogInfo($"{property.Name} has no Custom Attribute {reactiveAttribute.Name}");
                        continue;
                    }*/
                    if (property.SetMethod == null)
                    {
                        Weaver.LogError($"Property {property.DeclaringType.FullName}.{property.Name} has no setter, therefore it is not possible for the property to change, and thus should not be marked with [Reactive]");
                        continue;
                    }
                   
                    // Declare a field to store the property value
                    var field = new FieldDefinition($"${property.Name}"  , FieldAttributes.Private, property.PropertyType);
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
                    }

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
                        throw new Exception($"[{PropertyChangedCoreWeaver.REACTIVE_ATTRIBUTE.Replace("Attribute","")}] is decorating { property.DeclaringType.FullName}.{property.Name} , but the property has no setter so there would be nothing to react to.  Consider removing the attribute.");
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
                    });
                }
            }
        }
    }
}