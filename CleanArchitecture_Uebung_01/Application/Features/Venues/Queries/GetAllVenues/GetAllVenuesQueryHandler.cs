using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Venues.Queries.GetAllVenues;

/// <summary>
/// Query-Handler zum Abrufen aller Venues.
/// </summary>
public sealed class GetAllVenuesQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAllVenuesQuery, Result<IEnumerable<GetVenueDto>>>
{
    public async Task<Result<IEnumerable<GetVenueDto>>> Handle(GetAllVenuesQuery request, 
        CancellationToken cancellationToken)
    {
        var entities = await uow.Venues.GetAllAsync(
            cancellationToken, 
            orderBy: q => q.OrderBy(v => v.Name));
        
        return Result<IEnumerable<GetVenueDto>>.Success(entities.Adapt<IEnumerable<GetVenueDto>>());
    }
}
