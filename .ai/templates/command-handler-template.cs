using Application.Common.Results;
using Application.Contracts;
using Application.Dtos;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.[ENTITY_PLURAL].Commands.[CommandName];

/// <summary>
/// Handler für [CommandName][ENTITY_NAME]Command.
/// </summary>
public sealed class [CommandName][ENTITY_NAME]CommandHandler(
    IUnitOfWork uow,
    I[ENTITY_NAME]UniquenessChecker uniquenessChecker)
    : IRequestHandler<[CommandName][ENTITY_NAME]Command, Result<Get[ENTITY_NAME]Dto>>
{
    public async Task<Result<Get[ENTITY_NAME]Dto>> Handle(
        [CommandName][ENTITY_NAME]Command request,
        CancellationToken cancellationToken)
    {
        // FOR CREATE:
        // 1. Load parent entity if needed
        // var parent = await uow.Parents.GetByIdAsync(request.ParentId, cancellationToken);
        // if (parent == null)
        //     return Result<Get[ENTITY_NAME]Dto>.NotFound($"Parent with ID {request.ParentId} not found.");
        
        // 2. Create entity via Factory
        // var entity = await [ENTITY_NAME].CreateAsync(
        //     request.[Param1],
        //     request.[Param2],
        //     parent,
        //     uniquenessChecker,
        //     cancellationToken);
        
        // 3. Persist
        // await uow.[ENTITY_PLURAL].AddAsync(entity, cancellationToken);
        // await uow.SaveChangesAsync(cancellationToken);
        
        // 4. Return DTO
        // return Result<Get[ENTITY_NAME]Dto>.Created(entity.Adapt<Get[ENTITY_NAME]Dto>());
        
        
        // FOR UPDATE:
        // 1. Load existing entity
        // var entity = await uow.[ENTITY_PLURAL].GetByIdAsync(request.Id, cancellationToken);
        // if (entity == null)
        //     return Result<Get[ENTITY_NAME]Dto>.NotFound($"[ENTITY_NAME] with ID {request.Id} not found.");
        
        // 2. Update via method
        // await entity.UpdateAsync(
        //     request.[Param1],
        //     request.[Param2],
        //     uniquenessChecker,
        //     cancellationToken);
        
        // 3. Persist
        // uow.[ENTITY_PLURAL].Update(entity);
        // await uow.SaveChangesAsync(cancellationToken);
        
        // 4. Return DTO
        // return Result<Get[ENTITY_NAME]Dto>.Success(entity.Adapt<Get[ENTITY_NAME]Dto>());
        
        
        // FOR DELETE:
        // 1. Load entity
        // var entity = await uow.[ENTITY_PLURAL].GetByIdAsync(request.Id, cancellationToken);
        // if (entity == null)
        //     return Result<bool>.NotFound($"[ENTITY_NAME] with ID {request.Id} not found.");
        
        // 2. Delete
        // uow.[ENTITY_PLURAL].Remove(entity);
        // await uow.SaveChangesAsync(cancellationToken);
        
        // 3. Return
        // return Result<bool>.NoContent();
        
        throw new NotImplementedException("Implement handler logic");
    }
}

