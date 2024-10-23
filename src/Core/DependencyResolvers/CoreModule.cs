using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<ICacheManager,MemoryCacheManager>();
            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
