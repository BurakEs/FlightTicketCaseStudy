using Core.Entities;

namespace Entities.DTos;

public class FlightOption : IDto
{
    public DateTime ArrivalDateTime { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public string FlightNumber { get; set; }
    public decimal Price { get; set; }

}