using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetAllBooks;

public readonly record struct GetAllBooksQuery : IRequest<Result<IReadOnlyCollection<GetBookDto>>>;
