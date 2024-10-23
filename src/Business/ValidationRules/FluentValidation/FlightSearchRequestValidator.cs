using Business.Constants;
using Entities.DTos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation;

public class FlightSearchRequestValidator : AbstractValidator<FlightSearchRequestDto>
{
    public FlightSearchRequestValidator()
    {
        RuleFor(x => x.OriginAirport)
    .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(x => x.DestinationAirport)
            .NotEmpty().WithMessage(Messages.NotEmpty);

        RuleFor(x => x)
            .Must(x => x.OriginAirport.Name != x.DestinationAirport.Name)
            .WithMessage(Messages.AirportsCannotBeTheSame);
        RuleFor(p => p.DepartureDate)
            .NotEmpty().WithMessage(Messages.DepartureDateCannotBeEmpty)
            .Must(BeNotInThePast).WithMessage(Messages.CanNotInThePast);

        RuleFor(p => p.ReturnDate)
        .NotEmpty()
        .When(p => p.IsRoundTrip) // ReturnDate boş olmamalı ise
        .WithMessage(Messages.ReturnDateCannotBeEmpty);

        RuleFor(p => p.ReturnDate)
            .GreaterThan(p => p.DepartureDate)
            .WithMessage(Messages.ReturnDateMustBeGreaterThanDepartureDate)
            .When(p => p.ReturnDate.HasValue);

    }
    private bool BeNotInThePast(DateTime date)
    {
        return date >= DateTime.Now;
    }
}
