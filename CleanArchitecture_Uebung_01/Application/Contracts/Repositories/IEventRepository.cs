using Domain.Entities;

namespace Application.Contracts.Repositories;

/// <summary>
/// Event-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface IEventRepository : IGenericRepository<Event>
{
    Task<IReadOnlyCollection<Event>> GetByVenueIdAsync(int venueId, CancellationToken ct = default);
}
