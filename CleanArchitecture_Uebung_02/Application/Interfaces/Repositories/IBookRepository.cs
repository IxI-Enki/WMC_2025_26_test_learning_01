using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Book-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IBookRepository : IGenericRepository<Book>
{
    // TODO: Implementiere die spezifischen Repository-Methoden für Book.
}
