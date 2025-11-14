using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// TODO: Implementiere die spezifischen Repository-Methoden für Author.
/// </summary>
public class AuthorRepository(AppDbContext ctx) : GenericRepository<Author>(ctx), IAuthorRepository
{
    /// <summary>
    /// TODO: Implementiere GetAuthorsWithBooksAsync - Alle Autoren mit ihren Büchern.
    /// </summary>
    public async Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException("AuthorRepository.GetAuthorsWithBooksAsync muss noch implementiert werden!");
    }
}

