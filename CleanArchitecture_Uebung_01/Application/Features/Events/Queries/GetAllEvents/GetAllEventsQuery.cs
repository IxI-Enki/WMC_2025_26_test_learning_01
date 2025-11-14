using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Events.Queries.GetAllEvents;

public readonly record struct GetAllEventsQuery() : IRequest<Result<IEnumerable<GetEventDto>>>;
