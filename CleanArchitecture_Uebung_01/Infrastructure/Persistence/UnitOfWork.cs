using Application.Contracts;
using Application.Contracts.Repositories;

namespace Infrastructure.Persistence;

/// <summary>
/// Unit of Work aggregiert Repositories und speichert Änderungen transaktional.
/// </summary>
public class UnitOfWork(AppDbContext dbContext, 
    IVenueRepository venues, IEventRepository events, ITicketRepository tickets) : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _dbContext = dbContext;
    private bool _disposed;

    public IVenueRepository Venues { get; } = venues;
    public IEventRepository Events { get; } = events;
    public ITicketRepository Tickets { get; } = tickets;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _dbContext.Dispose();
        }
        _disposed = true;
    }
}
