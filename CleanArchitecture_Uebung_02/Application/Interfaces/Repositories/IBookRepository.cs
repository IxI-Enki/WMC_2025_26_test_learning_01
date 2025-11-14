using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Book-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default);
    Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken ct = default);
}

