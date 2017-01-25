using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyChangedCore.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReactiveAttribute : Attribute
    {
    }
}
