using Business.Constants;
using Entities.DTos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation;

public class FlightSearchRequestValidator : AbstractValidator<FlightSearchRequestDto>
{
    public FlightSearchRequestValidator()
    {
        RuleFor(x => x.OriginAirpot).NotEmpty().WithMessage(Messages.NotEmpty);
        RuleFor(x => x.DestinationAirpot).NotEmpty().WithMessage(Messages.NotEmpty);
        RuleFor(x => x).Must(x => x.OriginAirpot.Id != x.DestinationAirpot.Id).WithMessage(Messages.AirportsCannotBeTheSame); 
        RuleFor(x => x).Must(x => x.OriginAirpot.Name != x.DestinationAirpot.Name).WithMessage(Messages.AirportsCannotBeTheSame); 
        RuleFor(x => x).Must(x => x.OriginAirpot.IataCode != x.DestinationAirpot.IataCode).WithMessage(Messages.AirportsCannotBeTheSame); 

        RuleFor(p => p.DepartureDate)
        .NotEmpty()
        .Must(BeNotInThePast).WithMessage(Messages.CanNotInThePast);
    }
    private bool BeNotInThePast(DateTime date)
    {
        return date >= DateTime.Now;
    }
}
