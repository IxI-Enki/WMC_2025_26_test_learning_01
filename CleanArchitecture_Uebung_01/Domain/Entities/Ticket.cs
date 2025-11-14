using Domain.Common;
using Domain.Exceptions;
using Domain.ValidationSpecifications;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert ein verkauftes Ticket für eine Veranstaltung.
/// </summary>
public class Ticket : BaseEntity
{
    /// <summary>
    /// Fremdschlüssel auf das Event.
    /// </summary>
    public int EventId { get; private set; }
    
    /// <summary>
    /// Navigation zum Event. Wird von EF geladen, wenn Include verwendet wird.
    /// </summary>
    public Event Event { get; private set; } = null!;
    
    /// <summary>
    /// Name des Käufers.
    /// </summary>
    public string BuyerName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Kaufdatum des Tickets.
    /// </summary>
    public DateTime PurchaseDate { get; private set; }
    
    /// <summary>
    /// Preis des Tickets in Euro.
    /// </summary>
    public decimal Price { get; private set; }
    
    private Ticket() { } // Für EF Core

    /// <summary>
    /// Erstellt ein neues Ticket.
    /// </summary>
    public static Ticket Create(Event eventEntity, string buyerName, DateTime purchaseDate, decimal price)
    {
        ArgumentNullException.ThrowIfNull(eventEntity);
        
        var trimmedBuyerName = (buyerName ?? string.Empty).Trim();
        
        ValidateTicketProperties(eventEntity.Id, trimmedBuyerName, purchaseDate, price);
        
        return new Ticket 
        { 
            Event = eventEntity, 
            EventId = eventEntity.Id, 
            BuyerName = trimmedBuyerName, 
            PurchaseDate = purchaseDate,
            Price = price
        };
    }

    /// <summary>
    /// Validiert die Ticket-Eigenschaften auf Domain-Ebene.
    /// </summary>
    public static void ValidateTicketProperties(int eventId, string buyerName, DateTime purchaseDate, decimal price)
    {
        var validationResults = new List<DomainValidationResult>
        {
            TicketSpecifications.CheckEventId(eventId),
            TicketSpecifications.CheckBuyerName(buyerName),
            TicketSpecifications.CheckPurchaseDate(purchaseDate),
            TicketSpecifications.CheckPrice(price)
        };
        
        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }
}
