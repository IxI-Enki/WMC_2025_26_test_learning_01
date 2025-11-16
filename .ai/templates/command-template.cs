using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.[ENTITY_PLURAL].Commands.[CommandName];

/// <summary>
/// Command zum [Action Description] eines [ENTITY_NAME].
/// </summary>
/// <param name="[Param1]">[Description]</param>
/// <param name="[Param2]">[Description]</param>
public readonly record struct [CommandName][ENTITY_NAME]Command(
    [TYPE_1] [Param1],
    [TYPE_2] [Param2],
    [TYPE_3] [Param3]
) : IRequest<Result<Get[ENTITY_NAME]Dto>>;

