﻿using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly ILogger<AirportController> _logger;
        private IAirportService _airportService { get; set; }
        public AirportController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet("search")]
        public IActionResult Search(string searchTerm)
        {
            var result = _airportService.SearchAirports(searchTerm);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}