using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ReactiveAddins
{
    public interface IModuleStartup:IStartup
    {
        IConfigurationRoot Configuration { get; }
    }
}