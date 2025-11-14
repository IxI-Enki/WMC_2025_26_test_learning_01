using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Author-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync(CancellationToken ct = default);
}

