using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetAllBooks;

public record GetAllBooksQuery : IRequest<Result<IReadOnlyCollection<GetBookDto>>>;
