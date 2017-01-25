using System.Linq;
using Splat;

namespace ReactiveCore
{
    /// <summary>
    /// Helper class used by ReactiveCore to determine the default property in an
    /// implicit binding.
    /// </summary>
    /*public class DefaultPropertyBinding
    {
        static DefaultPropertyBinding()
        {
            ReactiveCoreApp.EnsureInitialized();
        }

        public static string GetPropertyForControl(object control)
        {
            return Locator.Current.GetServices<IDefaultPropertyBindingProvider>()
                .Select(x => x.GetPropertyForControl(control))
                .Where(x => x != null)
                .OrderByDescending(x => x.Item2)
                .Select(x => x.Item1)
                .FirstOrDefault();
        }
    }*/
}
