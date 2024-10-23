using Core.Entities;

namespace Entities.DTos;

public class FlightSearchResponseDto : IDto
{
   public FlightDetailDto DepartureAirportDetails { get; set; }
   public FlightDetailDto? ReturnAirportDetails { get; set; }
}
