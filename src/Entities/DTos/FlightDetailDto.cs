using Core.Entities;
using Entities.Concrete;

namespace Entities.DTos;

public class FlightDetailDto : IDto
{
    public Airport OriginAirport { get; set; }
    public Airport DestinationAirport { get; set; }
    public List<FlightOption> FlightOptions { get; set; }
}
