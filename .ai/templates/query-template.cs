using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.[ENTITY_PLURAL].Queries.[QueryName];

/// <summary>
/// Query zum Abrufen von [Description].
/// </summary>
public readonly record struct [QueryName]Query(
    [OPTIONAL_PARAMS]
) : IRequest<Result<[RETURN_TYPE]>>;

// EXAMPLES:
// GetById: (int Id) : IRequest<Result<Get[ENTITY_NAME]Dto>>
// GetAll:  () : IRequest<Result<IReadOnlyCollection<Get[ENTITY_NAME]Dto>>>
// Custom:  (string Filter) : IRequest<Result<IReadOnlyCollection<Get[ENTITY_NAME]Dto>>>

