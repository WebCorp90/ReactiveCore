
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
    using ReactiveCore;
    [ImplementPropertyChanged]
    public class BlogCore :   ReactiveObject
    {
        string _name;
        public string Name { get { return _name; } set { this.RaiseAndSetIfChanged(ref _name, value); } }

        string _cat;
        public string Category { get { return _cat; } set { this.RaiseAndSetIfChanged(ref _cat, value); } }
    }
}