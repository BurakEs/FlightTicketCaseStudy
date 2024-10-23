using Business.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class AirportSearchValidator : AbstractValidator<string>
    {
        public AirportSearchValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage(Messages.NotEmpty).MinimumLength(4).WithMessage(Messages.SearchCharacterLimit);
        }
    }
}
