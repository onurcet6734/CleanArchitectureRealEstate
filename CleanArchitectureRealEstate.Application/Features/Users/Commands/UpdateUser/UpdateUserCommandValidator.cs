using FluentValidation;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            When(x => x.FirstName != null, () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(100).WithMessage("FirstName must not exceed 100 characters");
            });

            When(x => x.LastName != null, () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(100).WithMessage("LastName must not exceed 100 characters");
            });
        }
    }
}
