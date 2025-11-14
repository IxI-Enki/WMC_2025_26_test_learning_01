using Application.Common.Exceptions;
using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Events.Commands.CreateEvent;

/// <summary>
/// TODO: Implementiere den Command-Handler zum Erstellen eines Events.
/// 
/// HINWEISE:
/// 1. Hole die Venue aus dem Repository (GetByIdAsync)
/// 2. Prüfe, ob die Venue existiert (NotFoundException werfen falls nicht)
/// 3. Erstelle das Event über Event.Create(venue, ...)
/// 4. Füge das Event zum Repository hinzu (AddAsync)
/// 5. Speichere die Änderungen (SaveChangesAsync)
/// 6. Gib Result<GetEventDto>.Created zurück
/// </summary>
public sealed class CreateEventCommandHandler(IUnitOfWork uow) 
    : IRequestHandler<CreateEventCommand, Result<GetEventDto>>
{
    public async Task<Result<GetEventDto>> Handle(CreateEventCommand request, 
        CancellationToken cancellationToken)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("CreateEventCommandHandler.Handle muss noch implementiert werden!");
    }
}
