using Application.Common.Models;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

public readonly record struct DeleteBookCommand( int Id ) : IRequest<Result<bool>>;

