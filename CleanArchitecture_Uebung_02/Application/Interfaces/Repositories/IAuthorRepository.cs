using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Author-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync( CancellationToken ct );
    Task<Author?> GetByFullName( string fullName, CancellationToken ct = default );
    Task<Author?> GetByIdAsync( int id, CancellationToken ct = default );
    Task<Author?> GetByISBNAsync( string isbn, CancellationToken ct = default );
}

