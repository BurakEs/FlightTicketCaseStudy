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
        SearchRequest searchRequest = _mapper.Map<SearchRequest>(flightSearchRequestDto);

        SearchResult searchResult = _airSearchClient.AvailabilitySearchAsync(searchRequest).Result;

        if (!searchResult.HasError)
        {
            FlightSearchResponseDto flightSearchResponseDto = _mapper.Map<FlightSearchResponseDto>(searchResult, opt =>
            {
                opt.Items["OriginAirpot"] = flightSearchRequestDto.OriginAirpot;
                opt.Items["DestinationAirpot"] = flightSearchRequestDto.DestinationAirpot;
            });

            return new SuccessDataResult<FlightSearchResponseDto>(flightSearchResponseDto);
        }

        return new ErrorDataResult<FlightSearchResponseDto>();
    }
}
