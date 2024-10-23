using AutoMapper;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using Entities.DTos;
using FlightProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete;

public class FlightManager : IFlightService
{
    AirSearchClient _airSearchClient;
    IMapper _mapper;
    public FlightManager(IMapper mapper)
    {
        _airSearchClient = new AirSearchClient();
        _mapper = mapper;
    }

    [ValidationAspect(typeof(FlightSearchRequestValidator), Priority = 1)]
    public IDataResult<FlightSearchResponseDto> FlightSearch(FlightSearchRequestDto flightSearchRequestDto)
    {
        FlightSearchResponseDto flightSearchResponse = new FlightSearchResponseDto();

        SearchRequest searchDepartureRequest = _mapper.Map<SearchRequest>(flightSearchRequestDto);
        if (flightSearchRequestDto.IsRoundTrip)
        {
            flightSearchRequestDto.SwapAirports();
            SearchRequest searchReturnRequest = _mapper.Map<SearchRequest>(flightSearchRequestDto);
            SearchResult searchReturnResult = _airSearchClient.AvailabilitySearchAsync(searchReturnRequest).Result;

            FlightDetailDto returnFlightSearchResult = _mapper.Map<FlightDetailDto>(searchReturnResult, opt =>
            {
                opt.Items["OriginAirpot"] = flightSearchRequestDto.OriginAirport;
                opt.Items["DestinationAirpot"] = flightSearchRequestDto.DestinationAirport;
            });

            flightSearchResponse.ReturnAirportDetails = returnFlightSearchResult;
        }

        SearchResult searchDepartureResult = _airSearchClient.AvailabilitySearchAsync(searchDepartureRequest).Result;
        FlightDetailDto departureFlightSearchResult = _mapper.Map<FlightDetailDto>(searchDepartureResult, opt =>
        {
            opt.Items["OriginAirpot"] = flightSearchRequestDto.DestinationAirport;
            opt.Items["DestinationAirpot"] = flightSearchRequestDto.OriginAirport;
        });
        flightSearchResponse.DepartureAirportDetails = departureFlightSearchResult;



        return new SuccessDataResult<FlightSearchResponseDto>(flightSearchResponse);
    }
}
