using ReactiveCore;
using ReactiveHelpers.Core.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReactiveDbCore
{

    public class Index
    {
        public IndexAttribute Attribute { get; internal set; }
        public PropertyInfo Property { get; internal set; }
        public string Name => Attribute.Name;
    }
    public class Indexes : SortedList<int,Index> { }

    public static class ObjectMixins
    {
        internal static PropertyInfo GetKey<T>()where T : ReactiveDbObject
        {
            if(!typeof(T).HasDataContractAttribute())
                throw ReactiveDbCoreException.NoDataContractSpecified;
#if CORE
            var properties = typeof(T).GetTypeInfo().GetProperties();
#else
            var properties= typeof(T).GetProperties();
#endif
            var result =properties.Where(property =>
                      property.GetCustomAttributes(false)
                              .OfType<KeyAttribute>()
                              .Any()
                     ).ToList().FirstOrDefault();
            if(result==null)
                throw ReactiveDbCoreException.NoKeySpecified;
            return result;
        }

        internal static Dictionary<string,Indexes> GetIndexes<T>() where T:ReactiveDbObject
        {
            var res = new Dictionary<string, Indexes>();
#if CORE
            var properties = typeof(T).GetTypeInfo().GetProperties();
#else
            var properties= typeof(T).GetProperties();
#endif
            var result = properties.Where(property =>
                       property.GetCustomAttributes(true)
                               .OfType<IndexAttribute>()
                               .Any()
                     ).ToList();

            result.ForEach(prop =>
            {
                prop.GetCustomAttributes<IndexAttribute>().ToList().ForEach(attr =>
                {
                    if (!res.ContainsKey(attr.Name)) res[attr.Name] = new Indexes();
                    res[attr.Name].Add(attr.Range, new Index() { Attribute=attr,Property=prop });
                });
            });
            return res;
        }

        public static bool HasDataContractAttribute(this Type t)
        {
            return t.HasAttribute<DataContractAttribute>() != null;
        }

        public static TAttr HasAttribute<TAttr>(this Type t)where TAttr :Attribute
        {
            try
            {
#if CORE
                return t.GetTypeInfo().GetCustomAttribute<TAttr>(true);
#else
                return Attribute.GetCustomAttribute(t, typeof(TAttr), true) as TAttr;
#endif
            }
            catch
            {
                return null;
            }
           
        }
        
    }
}
