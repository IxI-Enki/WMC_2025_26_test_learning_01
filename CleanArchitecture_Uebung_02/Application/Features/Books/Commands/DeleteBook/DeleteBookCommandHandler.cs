using Application.Common.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandHandler( IUnitOfWork uow ) : IRequestHandler<DeleteBookCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle( DeleteBookCommand request, CancellationToken cancellationToken )
    {
        // Sensor laden
        var entity = await uow.Books.GetByISBN(request.Id, cancellationToken);
        if(entity is null)
        {
            return Result<bool>.NotFound( $"Book with Id {request.Id} not found." );
        }
        // Sensor entfernen und Änderungen speichern
        uow.Books.Delete( entity );

        await uow.SaveChangesAsync( cancellationToken );

        return Result<bool>.NoContent( );
    }
}
