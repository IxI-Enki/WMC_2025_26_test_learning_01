using Application.Interfaces;
using Domain.Contracts;

namespace Application.Services;

/// <summary>
/// BookUniquenessChecker.
/// </summary>
public class BookUniquenessChecker( IUnitOfWork uow ) : IBookUniquenessChecker
{
    public async Task<bool> IsUniqueAsync( int id, string isbn, CancellationToken ct = default )
    {
        var existing = await uow.Books.GetByISBNAsync(isbn, ct);

        return existing is null || existing.Id == id;
    }
}
