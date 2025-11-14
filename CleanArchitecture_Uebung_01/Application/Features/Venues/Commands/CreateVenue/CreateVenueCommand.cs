using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Venues.Commands.CreateVenue;

public readonly record struct CreateVenueCommand(string Name, string Address, int Capacity) 
    : IRequest<Result<GetVenueDto>>;
