using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext. Verwaltet die Verbindung zur Datenbank und das Mapping der Entitäten.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    /// <summary>
    /// Fluent-API Konfigurationen für EF Core.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venue>(venue =>
        {
            venue.Property(v => v.Name).HasMaxLength(200).IsRequired();
            venue.Property(v => v.Address).HasMaxLength(500).IsRequired();
            venue.Property(v => v.Capacity).IsRequired();

            // RowVersion für Optimistic Concurrency
            venue.Property(v => v.RowVersion).IsRowVersion();
            
            // Uniqueness constraint (Name muss eindeutig sein)
            venue.HasIndex(v => v.Name)
                  .IsUnique()
                  .HasDatabaseName("UX_Venues_Name");
        });

        modelBuilder.Entity<Event>(evt =>
        {
            evt.Property(e => e.Name).HasMaxLength(200).IsRequired();
            evt.Property(e => e.DateTime).IsRequired();
            evt.Property(e => e.MaxAttendees).IsRequired();

            // Index für häufige Abfragen nach Venue und DateTime
            evt.HasIndex(e => new { e.VenueId, e.DateTime });
            
            // Beziehung: Jedes Event gehört zu genau einer Venue (Restrict Delete)
            evt.HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // RowVersion für Optimistic Concurrency
            evt.Property(e => e.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<Ticket>(ticket =>
        {
            ticket.Property(t => t.BuyerName).HasMaxLength(200).IsRequired();
            ticket.Property(t => t.PurchaseDate).IsRequired();
            ticket.Property(t => t.Price).HasColumnType("decimal(18,2)").IsRequired();

            // Index für häufige Abfragen nach Event
            ticket.HasIndex(t => t.EventId);
            
            // Beziehung: Jedes Ticket gehört zu genau einem Event (Cascade Delete)
            ticket.HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // RowVersion für Optimistic Concurrency
            ticket.Property(t => t.RowVersion).IsRowVersion();
        });
    }
}
