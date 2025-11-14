using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// TODO: Implementiere das Ticket-Repository.
/// 
/// HINWEISE:
/// - Erbt von GenericRepository<Ticket>
/// - Implementiert ITicketRepository
/// - GetByEventIdAsync: Holt alle Tickets für ein Event (AsNoTracking, Where, OrderBy PurchaseDate, ToListAsync)
/// - GetTicketCountForEventAsync: Zählt die Tickets für ein Event (CountAsync)
/// </summary>
public class TicketRepository(AppDbContext ctx) : GenericRepository<Ticket>(ctx), ITicketRepository
{
    public async Task<IReadOnlyCollection<Ticket>> GetByEventIdAsync(int eventId, CancellationToken ct = default)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("GetByEventIdAsync muss noch implementiert werden!");
    }

    public async Task<int> GetTicketCountForEventAsync(int eventId, CancellationToken ct = default)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("GetTicketCountForEventAsync muss noch implementiert werden!");
    }
}
