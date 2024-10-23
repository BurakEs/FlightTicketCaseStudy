using AutoMapper;
using Entities.Concrete;
using Entities.DTos;

namespace Business.Utilities.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FlightSearchRequestDto,FlightProvider.SearchRequest>()
            .ForPath(x => x.Origin, opt => opt.MapFrom(src => src.OriginAirpot.Ident))
            .ForPath(x => x.Destination, opt => opt.MapFrom(src => src.DestinationAirpot.Ident))
            .ForMember(x => x.DepartureDate, opt => opt.MapFrom(src => src.DepartureDate));

        CreateMap<FlightProvider.SearchResult, FlightSearchResponseDto>()
             .ForMember(x => x.FlightOptions, opt => opt.MapFrom(src => src.FlightOptions))
             .AfterMap((src, dest, ctx) =>
             {
                 if (ctx.Items.TryGetValue("OriginAirpot", out var origin))
                 {
                     dest.OriginAirpot = (Airport)origin;
                 }

                 if (ctx.Items.TryGetValue("DestinationAirpot", out var destination))
                 {
                     dest.DestinationAirpot = (Airport)destination;
                 }
             });

        CreateMap<FlightProvider.FlightOption,FlightOption>()
            .ForMember(x => x.FlightNumber, opt => opt.MapFrom(src => src.FlightNumber))
            .ForMember(x => x.DepartureDateTime, opt => opt.MapFrom(src => src.DepartureDateTime))
            .ForMember(x => x.ArrivalDateTime, opt => opt.MapFrom(src => src.ArrivalDateTime))
            .ForMember(x => x.Price, opt => opt.MapFrom(src => src.Price));
    }
}
