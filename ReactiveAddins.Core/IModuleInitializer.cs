using Microsoft.Extensions.DependencyInjection;

namespace ReactiveAddins
{
    internal interface IModuleInitializer
    {
        void ConfigureServices(IServiceCollection services);
    }
}