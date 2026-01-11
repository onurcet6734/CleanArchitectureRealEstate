using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.CreateFlat
{
    public class CreateFlatCommandValidator : AbstractValidator<CreateFlatCommand>
    {
        public CreateFlatCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
