using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Commands.UpdateBook;

public readonly record struct UpdateBookCommand(int Id, string ISBN, string Title, int AuthorId, int PublicationYear, int AvailableCopies)
: IRequest<Result<GetBookDto>>;
