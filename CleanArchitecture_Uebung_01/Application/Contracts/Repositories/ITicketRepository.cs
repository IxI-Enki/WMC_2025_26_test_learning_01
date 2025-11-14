using Domain.Entities;

namespace Application.Contracts.Repositories;

/// <summary>
/// Ticket-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IReadOnlyCollection<Ticket>> GetByEventIdAsync(int eventId, CancellationToken ct = default);
    Task<int> GetTicketCountForEventAsync(int eventId, CancellationToken ct = default);
}
