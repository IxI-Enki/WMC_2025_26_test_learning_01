using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BookRepository(AppDbContext ctx) : GenericRepository<Book>(ctx), IBookRepository
{
    public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default)
        => await Set.Include(b => b.Author).FirstOrDefaultAsync(b => b.ISBN == isbn, ct);

    public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken ct = default)
        => await Set.AsNoTracking()
            .Include(b => b.Author)
            .Where(b => b.AuthorId == authorId)
            .OrderBy(b => b.Title)
            .ToListAsync(ct);
}
