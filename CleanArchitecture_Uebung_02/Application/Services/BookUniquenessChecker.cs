using Application.Interfaces;
using Domain.Contracts;

namespace Application.Services;

/// <summary>
/// TODO: Implementiere den BookUniquenessChecker.
/// </summary>
public class BookUniquenessChecker(IUnitOfWork uow) : IBookUniquenessChecker
{
    public async Task<bool> IsUniqueAsync(int id, string isbn, CancellationToken ct = default)
    {
        throw new NotImplementedException("BookUniquenessChecker muss noch implementiert werden!");
    }
}

