using ReactiveCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public static class ObjectMixins
    {
        public static PropertyInfo GetKey<T>()where T : ReactiveObject
        {
            if(!typeof(T).HasDataContractAttribute())
                throw ReactiveDbCoreException.NoDataContractSpecified;

            var result= typeof(T).GetProperties()
               .Where(property =>
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
                return Attribute.GetCustomAttribute(t, typeof(TAttr), true) as TAttr;
            }
            catch
            {
                return null;
            }
           
        }
        
    }
}
