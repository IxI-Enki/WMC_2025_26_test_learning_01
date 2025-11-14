using FluentValidation;

namespace Application.Features.Venues.Commands.CreateVenue;

/// <summary>
/// TODO: Implementiere die FluentValidation-Regeln für CreateVenueCommand.
/// 
/// Regeln:
/// - Name: NotEmpty, MinimumLength(3)
/// - Address: NotEmpty, MinimumLength(5)
/// - Capacity: GreaterThan(0)
/// </summary>
public class CreateVenueCommandValidator : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
        // TODO: Implementiere die Validierungsregeln
        // Beispiel aus dem Template:
        // RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
    }
}
