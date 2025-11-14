using FluentValidation;

namespace Application.Features.Venues.Commands.UpdateVenue;

public class UpdateVenueCommandValidator : AbstractValidator<UpdateVenueCommand>
{
    public UpdateVenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.Capacity)
            .GreaterThan(0);
    }
}
