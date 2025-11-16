using Application.Common.Results;
using Application.Contracts;
using Application.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.[ENTITY_PLURAL].Queries.[QueryName];

/// <summary>
/// Handler für [QueryName]Query.
/// </summary>
public sealed class [QueryName]QueryHandler(IUnitOfWork uow)
    : IRequestHandler<[QueryName]Query, Result<[RETURN_TYPE]>>
{
    public async Task<Result<[RETURN_TYPE]>> Handle(
        [QueryName]Query request,
        CancellationToken cancellationToken)
    {
        // FOR GET_BY_ID:
        // var entity = await uow.[ENTITY_PLURAL].GetByIdAsync(request.Id, cancellationToken);
        // if (entity == null)
        //     return Result<Get[ENTITY_NAME]Dto>.NotFound($"[ENTITY_NAME] with ID {request.Id} not found.");
        // return Result<Get[ENTITY_NAME]Dto>.Success(entity.Adapt<Get[ENTITY_NAME]Dto>());
        
        // FOR GET_ALL:
        // var entities = await uow.[ENTITY_PLURAL].GetAllAsync(
        //     orderBy: q => q.OrderBy(e => e.[Property]),
        //     ct: cancellationToken);
        // var dtos = entities.Adapt<IReadOnlyCollection<Get[ENTITY_NAME]Dto>>();
        // return Result<IReadOnlyCollection<Get[ENTITY_NAME]Dto>>.Success(dtos);
        
        // FOR CUSTOM QUERY:
        // var entities = await uow.[ENTITY_PLURAL].GetAllAsync(
        //     filter: e => e.[Property].Contains(request.Filter),
        //     orderBy: q => q.OrderBy(e => e.[Property]),
        //     ct: cancellationToken);
        // var dtos = entities.Adapt<IReadOnlyCollection<Get[ENTITY_NAME]Dto>>();
        // return Result<IReadOnlyCollection<Get[ENTITY_NAME]Dto>>.Success(dtos);
        
        throw new NotImplementedException("Implement query logic");
    }
}

