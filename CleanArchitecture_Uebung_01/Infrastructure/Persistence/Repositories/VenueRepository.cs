using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Spezifisches Repository für Venues mit zusätzlichen Abfragen.
/// </summary>
public class VenueRepository(AppDbContext ctx) : GenericRepository<Venue>(ctx), IVenueRepository
{
    public async Task<Venue?> GetByNameAsync(string name, CancellationToken ct = default)
        => await Set.FirstOrDefaultAsync(v => v.Name == name, ct);
}
