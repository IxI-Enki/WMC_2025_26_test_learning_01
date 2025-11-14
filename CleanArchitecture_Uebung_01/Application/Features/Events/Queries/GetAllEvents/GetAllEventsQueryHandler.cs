using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Events.Queries.GetAllEvents;

public sealed class GetAllEventsQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAllEventsQuery, Result<IEnumerable<GetEventDto>>>
{
    public async Task<Result<IEnumerable<GetEventDto>>> Handle(GetAllEventsQuery request, 
        CancellationToken cancellationToken)
    {
        var entities = await uow.Events.GetAllAsync(
            cancellationToken, 
            orderBy: q => q.OrderBy(e => e.DateTime));
        
        return Result<IEnumerable<GetEventDto>>.Success(entities.Adapt<IEnumerable<GetEventDto>>());
    }
}
