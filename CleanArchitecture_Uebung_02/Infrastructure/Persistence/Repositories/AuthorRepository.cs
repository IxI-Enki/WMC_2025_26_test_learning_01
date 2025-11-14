using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AuthorRepository(AppDbContext ctx) : GenericRepository<Author>(ctx), IAuthorRepository
{
    public async Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync(CancellationToken ct = default)
        => await Set.AsNoTracking()
            .Include(a => a.Books)
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync(ct);
}

