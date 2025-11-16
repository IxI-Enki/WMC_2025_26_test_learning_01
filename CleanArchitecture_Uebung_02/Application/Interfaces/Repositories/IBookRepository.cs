using Domain.Entities;
using System.Diagnostics.Metrics;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Book-spezifische Repository-Methoden zusätzlich zu den generischen CRUD-Operationen.
/// </summary>
public interface IBookRepository : IGenericRepository<Book>
{
    /// <summary>
    /// Findet ein Buch anhand seiner ISBN.
    /// </summary>
    Task<Book?> GetByISBNAsync( string isbn, CancellationToken ct = default );
    
    /// <summary>
    /// Alle Bücher eines bestimmten Autors.
    /// </summary>
    Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync( int authorId, CancellationToken ct = default );
}
