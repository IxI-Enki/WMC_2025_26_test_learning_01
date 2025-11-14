using Application.Common.Results;
using MediatR;

namespace Application.Features.Venues.Commands.DeleteVenue;

public readonly record struct DeleteVenueCommand(int Id) : IRequest<Result>;
