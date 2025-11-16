using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Author-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// GetByIdAsync ist bereits in IGenericRepository definiert.
/// </summary>
public interface IAuthorRepository : IGenericRepository<Author>
{
    /// <summary>
    /// Alle Autoren mit ihren Büchern (Eager Loading).
    /// </summary>
    Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync( CancellationToken ct );
    
    /// <summary>
    /// Findet einen Author anhand des vollständigen Namens (FirstName + LastName).
    /// </summary>
    Task<Author?> GetByFullName( string fullName, CancellationToken ct = default );
    
    /// <summary>
    /// Findet einen Author über die ISBN eines seiner Bücher (mit Books Navigation Property).
    /// </summary>
    Task<Author?> GetByISBNAsync( string isbn, CancellationToken ct = default );
}

