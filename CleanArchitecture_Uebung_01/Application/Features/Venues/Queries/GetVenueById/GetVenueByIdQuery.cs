using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Venues.Queries.GetVenueById;

public readonly record struct GetVenueByIdQuery(int Id) : IRequest<Result<GetVenueDto>>;
