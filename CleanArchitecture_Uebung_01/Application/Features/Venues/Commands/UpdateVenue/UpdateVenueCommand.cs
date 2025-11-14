using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Venues.Commands.UpdateVenue;

public readonly record struct UpdateVenueCommand(int Id, string Name, string Address, int Capacity) 
    : IRequest<Result<GetVenueDto>>;
