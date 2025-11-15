using Domain.Entities;
using System.Diagnostics.Metrics;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Book-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IBookRepository : IGenericRepository<Book>
{
    // TODO: Implementiere die spezifischen Repository-Methoden für Book.
      Task<IReadOnlyCollection<Book>> GetBooksByhAuthorAsync( int authorId, CancellationToken ct = default );

      Task<Book?> GetByISBNAsync( string ísbn, CancellationToken ct = default );

    //Task<int> CountByBookIdAsync( int bookId, CancellationToken ct = default );
    //Task<Book?> GetByTitleAsync( string title, CancellationToken ct = default );
    //Task<IReadOnlyCollection<Book>> GetByBookIdPagedAsync( int bookId, int skip, int take, CancellationToken ct = default );
}
