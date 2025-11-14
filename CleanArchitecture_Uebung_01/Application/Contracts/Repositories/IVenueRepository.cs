using Domain.Entities;

namespace Application.Contracts.Repositories;

/// <summary>
/// Venue-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IVenueRepository : IGenericRepository<Venue>
{
    Task<Venue?> GetByNameAsync(string name, CancellationToken ct = default);
}
