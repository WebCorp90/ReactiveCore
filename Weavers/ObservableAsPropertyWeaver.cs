using System;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace PropertyChangedCore.Fody
{
    internal class ObservableAsPropertyWeaver: Module
    {
        public ObservableAsPropertyWeaver(PropertyChangedCoreWeaver module):base(module)
        {

        }

        public override void Execute()
        {
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
                Weaver.LogInfo("Could not find assembly: ReactiveCore.Helpers (" + string.Join(", ", Weaver.ModuleDefinition.AssemblyReferences.Select(x => x.Name)) + ")");
                return;
            }
            Weaver.LogInfo($"{helpers.Name} {helpers.Version}");

            var reactiveObject = Weaver.ModuleDefinition.FindType(PropertyChangedCoreWeaver.REACTIVECORE_ASSEMBLY, PropertyChangedCoreWeaver.REACTIVE_OBJECT, reactiveCore);

            // The types we will scan are subclasses of ReactiveObject
            var targetTypes = Weaver.ModuleDefinition.GetAllTypes().Where(x => x.BaseType != null && reactiveObject.IsAssignableFrom(x.BaseType));

            var observableAsPropertyHelper = Weaver.ModuleDefinition.FindType(PropertyChangedCoreWeaver.REACTIVECORE_ASSEMBLY, "ObservableAsPropertyHelper`1", reactiveCore, "T");
            var observableAsPropertyAttribute = Weaver.ModuleDefinition.FindType(PropertyChangedCoreWeaver.HELPERS_ASSEMBLY, PropertyChangedCoreWeaver.OBSERVABLE_AS_PROPERTY_ATTRIBUTE, helpers);
            var observableAsPropertyHelperGetValue = Weaver.ModuleDefinition.Import(observableAsPropertyHelper.Resolve().Properties.Single(x => x.Name == "Value").GetMethod);
            var exceptionType = Weaver.ModuleDefinition.Import(typeof(Exception));
            var exceptionConstructor = Weaver.ModuleDefinition.Import(exceptionType.Resolve().GetConstructors().Single(x => x.Parameters.Count == 1));

            foreach (var targetType in targetTypes)
            {
                foreach (var property in targetType.Properties.Where(x => x.IsDefined(observableAsPropertyAttribute) || (x.GetMethod?.IsDefined(observableAsPropertyAttribute) ?? false)).ToArray())
                {
                    var genericObservableAsPropertyHelper = observableAsPropertyHelper.MakeGenericInstanceType(property.PropertyType);
                    var genericObservableAsPropertyHelperGetValue = observableAsPropertyHelperGetValue.Bind(genericObservableAsPropertyHelper);
                    Weaver.ModuleDefinition.Import(genericObservableAsPropertyHelperGetValue);

                    // Declare a field to store the property value
                    var field = new FieldDefinition("$" + property.Name, FieldAttributes.Private, genericObservableAsPropertyHelper);
                    targetType.Fields.Add(field);

                    // It's an auto-property, so remove the generated field
                    if (property.SetMethod != null && property.SetMethod.HasBody)
                    {
                        // Remove old field (the generated backing field for the auto property)
                        var oldField = (FieldReference)property.GetMethod.Body.Instructions.Where(x => x.Operand is FieldReference).Single().Operand;
                        var oldFieldDefinition = oldField.Resolve();
                        targetType.Fields.Remove(oldFieldDefinition);

                        // Re-implement setter to throw an exception
                        property.SetMethod.Body = new MethodBody(property.SetMethod);
                        property.SetMethod.Body.Emit(il =>
                        {
                            il.Emit(OpCodes.Ldstr, "Never call the setter of an ObservabeAsPropertyHelper property.");
                            il.Emit(OpCodes.Newobj, exceptionConstructor);
                            il.Emit(OpCodes.Throw);
                            il.Emit(OpCodes.Ret);
                        });
                    }

                    property.GetMethod.Body = new MethodBody(property.GetMethod);
                    property.GetMethod.Body.Emit(il =>
                    {
                        var isValid = il.Create(OpCodes.Nop);
                        il.Emit(OpCodes.Ldarg_0);                                               // this
                        il.Emit(OpCodes.Ldfld, field.BindDefinition(targetType));               // pop -> this.$PropertyName
                        il.Emit(OpCodes.Dup);                                                   // Put an extra copy of this.$PropertyName onto the stack
                        il.Emit(OpCodes.Brtrue, isValid);                                       // If the helper is null, return the default value for the property
                        il.Emit(OpCodes.Pop);                                                   // Drop this.$PropertyName
                        EmitDefaultValue(property.GetMethod.Body, il, property.PropertyType);   // Put the default value onto the stack
                        il.Emit(OpCodes.Ret);                                                   // Return that default value
                        il.Append(isValid);                                                     // Add a marker for if the helper is not null
                        il.Emit(OpCodes.Callvirt, genericObservableAsPropertyHelperGetValue);   // pop -> this.$PropertyName.Value
                        il.Emit(OpCodes.Ret);                                                   // Return the value that is on the stack
                    });
                }
            }
        }

        public void EmitDefaultValue(MethodBody methodBody, ILProcessor il, TypeReference type)
        {
            if (type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Boolean) || type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Byte) ||
                type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Int16) || type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Int32))
            {
                il.Emit(OpCodes.Ldc_I4_0);
            }
            else if (type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Single))
            {
                il.Emit(OpCodes.Ldc_R4, (float)0);
            }
            else if (type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Int64))
            {
                il.Emit(OpCodes.Ldc_I8);
            }
            else if (type.CompareTo(Weaver.ModuleDefinition.TypeSystem.Double))
            {
                il.Emit(OpCodes.Ldc_R8, (double)0);
            }
            else if (type.IsGenericParameter || type.IsValueType)
            {
                methodBody.InitLocals = true;
                var local = new VariableDefinition(type);
                il.Body.Variables.Add(local);
                il.Emit(OpCodes.Ldloca_S, local);
                il.Emit(OpCodes.Initobj, type);
                il.Emit(OpCodes.Ldloc, local);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
        }
    }
}
