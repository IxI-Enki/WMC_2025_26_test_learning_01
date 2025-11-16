using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Author-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public class AuthorRepository(AppDbContext ctx)
    : GenericRepository<Author>(ctx), 
      IAuthorRepository
{
    /// <summary>
    /// Alle Autoren mit ihren Büchern.
    /// </summary>
    public async Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync(CancellationToken ct = default)
    {
    
    
    
        throw new NotImplementedException("AuthorRepository.GetAuthorsWithBooksAsync muss noch implementiert werden!");
    }

    /// <summary>
    /// Findet einen Author anhand des vollständigen Namens.
    /// </summary>
    public async Task<Author?> GetByFullName(string fullName, CancellationToken ct)
    {
        return await Set
            .FirstOrDefaultAsync(a => (a.FirstName + " " + a.LastName) == fullName, ct);
    }
}
