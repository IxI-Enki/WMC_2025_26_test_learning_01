using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Venues.Queries.GetVenueById;

/// <summary>
/// TODO: Implementiere den Query-Handler zum Abrufen einer Venue nach ID.
/// 
/// HINWEISE:
/// 1. Hole die Venue aus dem Repository (GetByIdAsync)
/// 2. Wenn null, gib Result<GetVenueDto>.NotFound zurück
/// 3. Sonst gib Result<GetVenueDto>.Success zurück
/// 
/// Signatur: public async Task<Result<GetVenueDto>> Handle(...)
/// </summary>
public sealed class GetVenueByIdQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetVenueByIdQuery, Result<GetVenueDto>>
{
    public async Task<Result<GetVenueDto>> Handle(GetVenueByIdQuery request, 
        CancellationToken cancellationToken)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("GetVenueByIdQueryHandler.Handle muss noch implementiert werden!");
    }
}
