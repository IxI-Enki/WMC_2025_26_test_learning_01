using Application.Common.Exceptions;
using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Contracts;
using Mapster;
using MediatR;

namespace Application.Features.Venues.Commands.UpdateVenue;

/// <summary>
/// TODO: Implementiere den Command-Handler zum Aktualisieren einer Venue.
/// 
/// HINWEISE:
/// 1. Hole die Venue aus dem Repository (GetByIdAsync)
/// 2. Prüfe, ob die Venue existiert (NotFoundException werfen)
/// 3. Rufe entity.UpdateAsync(...) auf
/// 4. Rufe uow.Venues.Update(entity) auf
/// 5. Rufe uow.SaveChangesAsync auf
/// 6. Gib Result<GetVenueDto>.Success zurück
/// 
/// Signatur: public async Task<Result<GetVenueDto>> Handle(...)
/// </summary>
public sealed class UpdateVenueCommandHandler(IUnitOfWork uow, 
    IVenueUniquenessChecker uniquenessChecker) 
    : IRequestHandler<UpdateVenueCommand, Result<GetVenueDto>>
{
    public async Task<Result<GetVenueDto>> Handle(UpdateVenueCommand request, 
        CancellationToken cancellationToken)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("UpdateVenueCommandHandler.Handle muss noch implementiert werden!");
    }
}
