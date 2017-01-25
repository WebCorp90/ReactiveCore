
using PropertyChanged;


namespace ReactiveCore.Test
{
    using ReactiveUI;
    //[ImplementPropertyChanged]
    public class BlogUI :ReactiveObject
    {
        string _name;
        public string Name { get { return _name; } set { this.RaiseAndSetIfChanged(ref _name, value); } }

        string _cat;
        public string Category { get { return _cat; } set { this.RaiseAndSetIfChanged(ref _cat, value); } }
    }

   
}

namespace ReactiveCore.Test
{
    using PropertyChangedCore.Helpers;
    using ReactiveCore;
    using System.Runtime.Serialization;


    public class BlogCore :   ReactiveObject
    {
        [Reactive]
        public string Name { get; set ;  }
        [Reactive]
        public string Category { get; set; }

       
        public string Temp { get;  }
    }
}

namespace ReactiveCore.Test
{
    [ImplementPropertyChanged]
    public class BlogStd 
    {

        public string Name { get; set; }

        public string Category { get; set; }
    }
}