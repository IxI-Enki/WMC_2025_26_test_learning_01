using FluentValidation;

namespace Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.VenueId)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.DateTime)
            .GreaterThan(DateTime.Now)
            .WithMessage("DateTime muss in der Zukunft liegen.");

        RuleFor(x => x.MaxAttendees)
            .GreaterThan(0);
    }
}
