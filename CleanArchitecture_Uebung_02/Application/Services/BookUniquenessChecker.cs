using Application.Interfaces;
using Domain.Contracts;

namespace Application.Services;

/// <summary>
/// Implementierung des IBookUniquenessChecker für die Domain-Validierung.
/// </summary>
public class BookUniquenessChecker(IUnitOfWork uow) : IBookUniquenessChecker
{
    public async Task<bool> IsUniqueAsync(int id, string isbn, CancellationToken ct = default)
    {
        var existing = await uow.Books.GetByISBNAsync(isbn, ct);
        return existing == null || existing.Id == id;
    }
}

