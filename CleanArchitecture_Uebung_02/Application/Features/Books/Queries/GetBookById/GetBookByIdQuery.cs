using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetBookById;

public readonly record struct GetBookByIdQuery( int Id ) : IRequest<Result<GetBookDto>>;

