using Domain.Common;
using Domain.Exceptions;
using Domain.ValidationSpecifications;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert eine Veranstaltung an einem bestimmten Ort.
/// </summary>
public class Event : BaseEntity
{
    /// <summary>
    /// Fremdschlüssel auf die Venue.
    /// </summary>
    public int VenueId { get; private set; }
    
    /// <summary>
    /// Navigation zur Venue. Wird von EF geladen, wenn Include verwendet wird.
    /// </summary>
    public Venue Venue { get; private set; } = null!;
    
    /// <summary>
    /// Name der Veranstaltung, z.B. "Rock am Ring 2025".
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    
    /// <summary>
    /// Datum und Uhrzeit der Veranstaltung.
    /// </summary>
    public DateTime DateTime { get; private set; }
    
    /// <summary>
    /// Maximale Anzahl der Tickets/Besucher für diese Veranstaltung.
    /// </summary>
    public int MaxAttendees { get; private set; }
    
    /// <summary>
    /// Sammlung aller verkauften Tickets für diese Veranstaltung.
    /// </summary>
    public ICollection<Ticket> Tickets { get; private set; } = default!;
    
    private Event() { } // Für EF Core

    /// <summary>
    /// Erstellt eine neue Event-Instanz.
    /// </summary>
    public static Event Create(Venue venue, string name, DateTime dateTime, int maxAttendees)
    {
        ArgumentNullException.ThrowIfNull(venue);
        
        var trimmedName = (name ?? string.Empty).Trim();
        
        ValidateEventProperties(venue.Id, trimmedName, dateTime, maxAttendees, venue.Capacity);
        
        return new Event 
        { 
            Venue = venue, 
            VenueId = venue.Id, 
            Name = trimmedName, 
            DateTime = dateTime, 
            MaxAttendees = maxAttendees 
        };
    }

    /// <summary>
    /// TODO: Implementiere die Validierung der Event-Eigenschaften auf Domain-Ebene.
    /// 
    /// HINWEIS: Diese Methode soll folgende Validierungen durchführen:
    /// - EventSpecifications.CheckVenueId(venueId)
    /// - EventSpecifications.CheckName(name)
    /// - EventSpecifications.CheckDateTime(dateTime)
    /// - EventSpecifications.CheckMaxAttendees(maxAttendees)
    /// - EventSpecifications.CheckMaxAttendeesNotExceedVenueCapacity(maxAttendees, venueCapacity)
    /// 
    /// Bei einem Fehler soll eine DomainValidationException geworfen werden.
    /// </summary>
    public static void ValidateEventProperties(int venueId, string name, DateTime dateTime, 
        int maxAttendees, int venueCapacity)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("ValidateEventProperties muss noch implementiert werden!");
    }
}
