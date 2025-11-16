using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class, IBaseEntity
{
    protected AppDbContext DbContext { get; } = dbContext;
    protected DbSet<T> Set { get; } = dbContext.Set<T>();

    public virtual async Task<T?> GetByISBN(int id, CancellationToken ct = default)
        => await Set.FindAsync([id], ct);

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(
        CancellationToken ct = default,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = Set.AsNoTracking();
        if (filter is not null)
            query = query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        return await query.ToListAsync(ct);
    }

    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await Set.AddAsync(entity, ct);

    public virtual void Update(T entity) => Set.Update(entity);

    public virtual void Delete(T entity) => Set.Remove(entity);

    public virtual async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await Set.AnyAsync(e => e.Id == id, ct);
}

