using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Venues.Queries.GetAllVenues;

public readonly record struct GetAllVenuesQuery() : IRequest<Result<IEnumerable<GetVenueDto>>>;
