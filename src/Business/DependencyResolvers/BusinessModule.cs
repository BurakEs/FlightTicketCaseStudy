using Business.Utilities.AutoMapper;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolvers
{
    public class BusinessModul : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
