using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValidationSpecifications;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert einen Veranstaltungsort (z.B. Konzerthalle, Stadion).
/// </summary>
public class Venue : BaseEntity
{
    /// <summary>
    /// Name des Veranstaltungsortes, z.B. "Wiener Stadthalle".
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    
    /// <summary>
    /// Adresse des Veranstaltungsortes, z.B. "Roland-Rainer-Platz 1, 1150 Wien".
    /// </summary>
    public string Address { get; private set; } = string.Empty;
    
    /// <summary>
    /// Maximale Kapazität (Anzahl der Besucher).
    /// </summary>
    public int Capacity { get; private set; }
    
    /// <summary>
    /// Sammlung aller Events, die an diesem Veranstaltungsort stattfinden.
    /// </summary>
    public ICollection<Event> Events { get; private set; } = default!;
    
    private Venue() { } // Für EF Core notwendig

    /// <summary>
    /// Erstellt asynchron eine neue Venue-Instanz mit den angegebenen Eigenschaften.
    /// </summary>
    /// <param name="name">Name des Veranstaltungsortes.</param>
    /// <param name="address">Adresse des Veranstaltungsortes.</param>
    /// <param name="capacity">Maximale Kapazität.</param>
    /// <param name="uniquenessChecker">Checker für die Eindeutigkeit des Namens.</param>
    /// <param name="ct">Cancellation Token.</param>
    /// <returns>Neue Venue-Instanz.</returns>
    public static async Task<Venue> CreateAsync(string name, string address, int capacity,
        IVenueUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        var trimmedName = (name ?? string.Empty).Trim();
        var trimmedAddress = (address ?? string.Empty).Trim();
        
        ValidateVenueProperties(trimmedName, trimmedAddress, capacity);
        await ValidateVenueUniqueness(0, trimmedName, uniquenessChecker, ct);
        
        return new Venue 
        { 
            Name = trimmedName, 
            Address = trimmedAddress, 
            Capacity = capacity 
        };
    }

    /// <summary>
    /// Aktualisiert die Eigenschaften der Venue.
    /// </summary>
    public async Task UpdateAsync(string name, string address, int capacity,
        IVenueUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        var trimmedName = (name ?? string.Empty).Trim();
        var trimmedAddress = (address ?? string.Empty).Trim();
        
        if (Name == trimmedName && Address == trimmedAddress && Capacity == capacity)
            return; // Keine Änderung
        
        ValidateVenueProperties(trimmedName, trimmedAddress, capacity);
        await ValidateVenueUniqueness(Id, trimmedName, uniquenessChecker, ct);
        
        Name = trimmedName;
        Address = trimmedAddress;
        Capacity = capacity;
    }

    public override string ToString() => $"{Name} ({Address})";

    /// <summary>
    /// Validiert die Venue-Eigenschaften auf Domain-Ebene.
    /// </summary>
    public static void ValidateVenueProperties(string name, string address, int capacity)
    {
        var validationResults = new List<DomainValidationResult>
        {
            VenueSpecifications.CheckName(name),
            VenueSpecifications.CheckAddress(address),
            VenueSpecifications.CheckCapacity(capacity)
        };
        
        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }

    /// <summary>
    /// Validiert die Eindeutigkeit der Venue (Name muss eindeutig sein).
    /// </summary>
    public static async Task ValidateVenueUniqueness(int id, string name,
        IVenueUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (!await uniquenessChecker.IsUniqueAsync(id, name, ct))
            throw new DomainValidationException("Uniqueness", "Ein Veranstaltungsort mit diesem Namen existiert bereits.");
    }
}
