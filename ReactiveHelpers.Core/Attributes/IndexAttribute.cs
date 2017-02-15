using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =true,Inherited =true)]
    public class IndexAttribute:Attribute
    {

        public readonly string Name;
        public readonly int Range;
        public readonly bool IsUnique;

        public IndexAttribute(bool isUnique=true)
        {
            this.Name = string.Empty;
            this.Range = 0;
            this.IsUnique = isUnique;
        }

        public IndexAttribute(string name,int range,bool isUnique=true)
        {
            Contract.Requires<ArgumentNullException>(name != null, nameof(name));
            this.Name=name;
            this.Range = range;
            this.IsUnique = isUnique;
        }
        
    }
}
