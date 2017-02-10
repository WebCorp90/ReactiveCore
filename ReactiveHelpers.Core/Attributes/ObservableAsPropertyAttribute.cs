using System;

namespace ReactiveHelpers.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ObservableAsPropertyAttribute : Attribute
    {
    }
}
