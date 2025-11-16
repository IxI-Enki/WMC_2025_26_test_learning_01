using Application.Common.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

/// <summary>
/// Command-Handler zum Löschen eines Buchs.
/// Gibt bei Erfolg NoContent zurück, ansonsten NotFound.
/// </summary>
public sealed class DeleteBookCommandHandler(IUnitOfWork uow) : IRequestHandler<DeleteBookCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        // Book laden
        var entity = await uow.Books.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)      
        {
            return Result<bool>.NotFound($"Book with Id {request.Id} not found.");
        }
        // Book entfernen und Änderungen speichern
        uow.Books.Remove(entity);

        await uow.SaveChangesAsync(cancellationToken);

        return Result<bool>.NoContent();
    }
}
