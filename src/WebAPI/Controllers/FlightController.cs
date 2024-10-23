using Business.Abstract;
using Entities.DTos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;
        private IFlightService _flightService { get; set; }
        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost("search")]
        public IActionResult Search(FlightSearchRequestDto flightSearchRequestDto)
        {
            var result = _flightService.FlightSearch(flightSearchRequestDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}

