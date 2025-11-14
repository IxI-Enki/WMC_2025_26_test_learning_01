using FluentValidation;

namespace Application.Features.Tickets.Commands.CreateTicket;

/// <summary>
/// TODO: Implementiere die FluentValidation-Regeln für CreateTicketCommand.
/// 
/// Regeln:
/// - EventId: GreaterThan(0)
/// - BuyerName: NotEmpty, MinimumLength(2)
/// - Price: GreaterThan(0)
/// </summary>
public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        // TODO: Implementiere die Validierungsregeln
    }
}
