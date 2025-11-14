using Application.Contracts;
using Domain.Contracts;

namespace Application.Services;

/// <summary>
/// Implementierung des IVenueUniquenessChecker für die Domain-Validierung.
/// </summary>
public class VenueUniquenessChecker(IUnitOfWork uow) : IVenueUniquenessChecker
{
    public async Task<bool> IsUniqueAsync(int id, string name, CancellationToken ct = default)
    {
        var existing = await uow.Venues.GetByNameAsync(name, ct);
        return existing == null || existing.Id == id;
    }
}
