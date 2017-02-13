using ReactiveCore;
using ReactiveHelpers.Core;
using System.Runtime.Serialization;
using Webcorp.unite;

namespace ReactiveTemp
{
    [DataContract]
    public class Class1:ReactiveObject
    {
        public Class1()
        {
                
        }

        
        [DataMember]
        public int Temp { get; set; }

        [Reactive]
        [DataMember]
        public Currency Sold { get; set; }

        [Reactive]
        [DataMember]
        public Currency Price { get; set; }

       // [DataMember]
       // public string _Price { get { return Price.ToString(); } set { Price = new Currency(value); } }
    }
}
