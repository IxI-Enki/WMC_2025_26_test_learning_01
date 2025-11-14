using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Spezifisches Repository für Events mit zusätzlichen Abfragen.
/// </summary>
public class EventRepository(AppDbContext ctx) : GenericRepository<Event>(ctx), IEventRepository
{
    public async Task<IReadOnlyCollection<Event>> GetByVenueIdAsync(int venueId, CancellationToken ct = default)
        => await Set.AsNoTracking()
            .Where(e => e.VenueId == venueId)
            .OrderBy(e => e.DateTime)
            .ToListAsync(ct);
}
