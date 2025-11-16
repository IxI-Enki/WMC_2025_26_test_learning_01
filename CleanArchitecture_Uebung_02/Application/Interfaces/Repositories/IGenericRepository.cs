using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Generisches Repository-Interface für CRUD-Operationen.
/// </summary>
public interface IGenericRepository<T> where T : IBaseEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync(
        CancellationToken ct = default,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, bool>>? filter = null);

    Task<T?> GetByISBN(int id, CancellationToken ct = default);
    
    Task AddAsync(T entity, CancellationToken ct = default);
    
    void Update(T entity);
    
    void Delete(T entity);
    
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
