using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Author-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public class AuthorRepository( AppDbContext ctx )
    : GenericRepository<Author>( ctx ),
      IAuthorRepository
{
    /// <summary>
    /// Alle Autoren mit ihren Büchern (Eager Loading).
    /// </summary>
    public async Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync( CancellationToken ct = default )
    {
        return await Set
            .AsNoTracking()
            .Include(a => a.Books)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Findet einen Author anhand des vollständigen Namens.
    /// </summary>
    public async Task<Author?> GetByFullName( string fullName, CancellationToken ct )
    {
        return await Set
            .FirstOrDefaultAsync( a => (a.FirstName + " " + a.LastName) == fullName, ct );
    }

    public async Task<Author?> GetByIdAsync( int id, CancellationToken ct )
    {
        return await Set
            .FirstOrDefaultAsync( a => a.Id == id, ct );
    }
    public async Task<Author?> GetByISBNAsync( string isbn, CancellationToken ct )
    {
        var set
            = await Set
             .Include( a => a.Books )
             .ThenInclude( c => c.Author )
             .Where(b => b.Books.Any(a => a.ISBN == isbn))
                .FirstOrDefaultAsync( ct );

        return set;
        //throw new NotImplementedException( );
    }
}
