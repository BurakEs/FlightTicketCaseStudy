using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTos;

public record FlightSearchRequestDto(Airport OriginAirpot, Airport DestinationAirpot, DateTime DepartureDate) : IDto;
public class FlightSearchResponseDto: IDto
{
    public Airport OriginAirpot { get; set; }
    public Airport DestinationAirpot { get; set; }
    public List<FlightOption> FlightOptions { get; set; }
}
public class FlightOption : IDto
{
    public DateTime ArrivalDateTime { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public string FlightNumber { get; set; }
    public decimal Price { get; set; }

}