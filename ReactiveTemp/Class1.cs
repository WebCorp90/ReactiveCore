using ReactiveHelpers.Core;
using System.Runtime.Serialization;
using Webcorp.unite;

namespace ReactiveTemp
{
    [DataContract]
    public class Class1
    {
        [DataMember]
        public int Temp { get; set; }

        [DataMember]
        public Currency Sold { get; set; }

        
        public Currency Price { get; set; }

        [Reactive]
        public string _Price { get { return Price.ToString(); } set { Price = new Currency(value); } }
    }
}
