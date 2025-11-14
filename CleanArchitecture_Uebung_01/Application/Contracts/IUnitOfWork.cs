using Application.Contracts.Repositories;

namespace Application.Contracts;

/// <summary>
/// Aggregiert Repositories und speichert Änderungen. Sicherer Umgang mit Transaktionen.
/// </summary>
public interface IUnitOfWork
{
    IVenueRepository Venues { get; }
    IEventRepository Events { get; }
    ITicketRepository Tickets { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
