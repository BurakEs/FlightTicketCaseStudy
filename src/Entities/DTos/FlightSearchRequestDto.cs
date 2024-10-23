using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTos;

public class FlightSearchRequestDto : IDto
{
    public Airport OriginAirport { get; set; }
    public Airport DestinationAirport { get; set; }
    public bool IsRoundTrip { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public void SwapAirports()
    {
        if (IsRoundTrip)
        {
            var temp = OriginAirport;
            OriginAirport = DestinationAirport;
            DestinationAirport = temp;
            DepartureDate = ReturnDate ?? DateTime.Now;
        }
    }
}
