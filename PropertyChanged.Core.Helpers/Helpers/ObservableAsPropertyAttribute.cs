using System;

namespace PropertyChangedCore.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ObservableAsPropertyAttribute : Attribute
    {
    }
}
