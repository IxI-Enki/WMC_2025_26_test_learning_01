namespace Application.Features.Dtos;

public record GetEventDto(int Id, int VenueId, string Name, DateTime DateTime, int MaxAttendees);
