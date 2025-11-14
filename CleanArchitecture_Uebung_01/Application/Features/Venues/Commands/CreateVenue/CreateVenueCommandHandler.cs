using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Venues.Commands.CreateVenue;

/// <summary>
/// Command-Handler zum Erstellen einer neuen Venue.
/// Domain-Validierungen erfolgen in der Entität (Factory-Methode).
/// </summary>
public sealed class CreateVenueCommandHandler(IUnitOfWork uow, 
    IVenueUniquenessChecker uniquenessChecker) 
    : IRequestHandler<CreateVenueCommand, Result<GetVenueDto>>
{
    public async Task<Result<GetVenueDto>> Handle(CreateVenueCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await Venue.CreateAsync(request.Name, request.Address, request.Capacity,
            uniquenessChecker, cancellationToken);
        await uow.Venues.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return Result<GetVenueDto>.Created(entity.Adapt<GetVenueDto>());
    }
}
