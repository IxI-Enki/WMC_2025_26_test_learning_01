using Application.Common.Models;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

public readonly record struct CreateBookCommand(string ISBN, string Title, int AuthorId, 
    int PublicationYear, int AvailableCopies) : IRequest<Result<GetBookDto>>;

