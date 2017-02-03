using ReactiveCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ReactiveDbCore
{
    public static class ObjectMixins
    {
        public static PropertyInfo GetKey<T>()where T : ReactiveObject
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
