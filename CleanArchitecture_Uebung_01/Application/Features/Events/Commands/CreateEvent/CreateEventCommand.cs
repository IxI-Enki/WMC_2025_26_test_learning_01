using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Events.Commands.CreateEvent;

public readonly record struct CreateEventCommand(int VenueId, string Name, DateTime DateTime, int MaxAttendees) 
    : IRequest<Result<GetEventDto>>;
