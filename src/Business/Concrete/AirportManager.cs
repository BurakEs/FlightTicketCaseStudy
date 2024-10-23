using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete;

public class AirportManager : IAirportService
{
    IAirportDAL _airportDAL;
    public AirportManager(IAirportDAL airportDAL)
    {
        _airportDAL = airportDAL;
    }

    [ValidationAspect(typeof(AirportSearchValidator), Priority = 1)]
    public IDataResult<List<Airport>> SearchAirports(string searchTerm)
    {
        var result = _airportDAL.Search(searchTerm);
        return new SuccessDataResult<List<Airport>>(result.ToList());
    }
    public Airport GetAirport(int Id)
    {
        var result = _airportDAL.Get(x=> x.Id == Id);
        return result;
    }

}
