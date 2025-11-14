using Application.Common.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

/// <summary>
/// TODO: Implementiere den DeleteBookCommandHandler.
/// </summary>
public sealed class DeleteBookCommandHandler(IUnitOfWork uow) : IRequestHandler<DeleteBookCommand, Result>
{
    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DeleteBookCommandHandler muss noch implementiert werden!");
    }
}

