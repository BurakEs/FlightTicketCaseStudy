using Business.Abstract;
using Business.Concrete;
using Business.Utilities.AutoMapper;
using Core.Utilities.IoC;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolvers
{
    public class BusinessModul : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(typeof(MappingProfile));


            serviceCollection.AddSingleton<IAirportService, AirportManager>();
            serviceCollection.AddSingleton<IAirportDAL, EfAirportDAL>();
            serviceCollection.AddSingleton<IFlightService, FlightManager>();

        }
    }
}
