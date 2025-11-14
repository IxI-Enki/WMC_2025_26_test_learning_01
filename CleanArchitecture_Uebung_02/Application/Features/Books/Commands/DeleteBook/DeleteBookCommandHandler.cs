using Application.Common.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandHandler(IUnitOfWork uow) : IRequestHandler<DeleteBookCommand, Result>
{
    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await uow.Books.GetByIdAsync(request.Id, cancellationToken);
        
        if (entity == null)
            return Result.NotFound($"Book with ID {request.Id} not found.");
        
        uow.Books.Delete(entity);
        await uow.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}

