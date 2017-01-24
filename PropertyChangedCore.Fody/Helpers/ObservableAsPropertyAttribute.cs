using System;

namespace PropertyChangedCore.Fody.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ObservableAsPropertyAttribute : Attribute
    {
    }
}
