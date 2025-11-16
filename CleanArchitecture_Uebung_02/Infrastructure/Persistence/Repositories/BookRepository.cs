using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Book Repository mit Author-Loading für alle Queries.
/// </summary>
public class BookRepository(AppDbContext ctx) : GenericRepository<Book>(ctx), IBookRepository
{
    /// <summary>
    /// Override GetByIdAsync um Author automatisch mitzuladen.
    /// </summary>
    public override async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    /// <summary>
    /// GetByISBNAsync - Buch per ISBN laden (mit Include Author).
    /// </summary>
    public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)
            .FirstOrDefaultAsync(s => s.ISBN == isbn, ct);
    }

    /// <summary>
    /// Override GetAllAsync um Author automatisch mitzuladen.
    /// </summary>
    public override async Task<IReadOnlyCollection<Book>> GetAllAsync(
        Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null,
        Expression<Func<Book, bool>>? filter = null,
        CancellationToken ct = default)
    {
        IQueryable<Book> query = Set.Include(b => b.Author).AsNoTracking();
        if (filter is not null)
            query = query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        return await query.ToListAsync(ct);
    }

    /// <summary>
    /// GetByBookIdAsync - Buch per Id laden (mit Include Author).
    /// </summary>
    public async Task<Book?> GetByBookIdAsync(int bookId, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)
            .FirstOrDefaultAsync(s => s.Id == bookId, ct);
    }

    /// <summary>
    /// Alle Bücher eines bestimmten Autors mit Author-Navigation Property.
    /// </summary>
    public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync( int authorId, CancellationToken ct = default )
    {
        return await Set
            .AsNoTracking()
            .Include(b => b.Author)
            .Where(b => b.AuthorId == authorId)
            .ToListAsync(ct);
    }

}
