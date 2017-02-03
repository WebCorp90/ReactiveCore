using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public class ReactiveDbCoreException:Exception
    {
        public static ReactiveDbCoreException NoDataContractSpecified = new ReactiveDbCoreException("ReactiveObject Must have DataContractAttribute to work correctly");
        public static ReactiveDbCoreException NoKeySpecified = new ReactiveDbCoreException("ReactiveObject Must have one KeyAttribute exactly");

        public ReactiveDbCoreException(string message):base(message)
        {

        }
        
    }
}
