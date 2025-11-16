# Hint 3: EventSpecifications - Vollständige Lösung

## Kompletter Code

```csharp
using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValidationSpecifications;

public static class EventSpecifications
{
    /// <summary>
    /// Prüft, ob VenueId größer als 0 ist.
    /// </summary>
    public static DomainValidationResult CheckVenueId(int venueId)
    {
        if (venueId <= 0)
            return DomainValidationResult.Invalid("VenueId", "VenueId muss größer als 0 sein.");
        
        return DomainValidationResult.Valid();
    }

    /// <summary>
    /// Prüft, ob DateTime nicht in der Vergangenheit liegt.
    /// </summary>
    public static DomainValidationResult CheckDateTime(DateTime dateTime)
    {
        if (dateTime < DateTime.Now)
            return DomainValidationResult.Invalid("DateTime", "DateTime darf nicht in der Vergangenheit liegen.");
        
        return DomainValidationResult.Valid();
    }

    /// <summary>
    /// Prüft, ob MaxAttendees die Venue-Kapazität nicht überschreitet.
    /// </summary>
    public static DomainValidationResult CheckMaxAttendeesNotExceedVenueCapacity(
        int maxAttendees, 
        int venueCapacity)
    {
        if (maxAttendees > venueCapacity)
            return DomainValidationResult.Invalid(
                "MaxAttendees", 
                "MaxAttendees darf die Venue-Kapazität nicht überschreiten.");
        
        return DomainValidationResult.Valid();
    }

    /// <summary>
    /// Validiert alle Event-Properties (Internal Validation).
    /// </summary>
    public static void ValidateEventInternal(
        int venueId,
        DateTime dateTime,
        int maxAttendees,
        int venueCapacity)
    {
        var validationResults = new List<DomainValidationResult>
        {
            CheckVenueId(venueId),
            CheckDateTime(dateTime),
            CheckMaxAttendeesNotExceedVenueCapacity(maxAttendees, venueCapacity)
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
```

---

## Erklärung

### Warum 3 separate Check-Methoden?

**Modularität:** Jede Methode prüft EINE Regel  
**Testbarkeit:** Jede kann einzeln getestet werden  
**Wiederverwendbarkeit:** Können in verschiedenen Kontexten genutzt werden

### Warum DomainValidationResult?

**Standardisiert:** Konsistentes Rückgabeformat  
**Klare Fehler:** Property + Message für genaue Fehlermeldung  
**Valid/Invalid:** Einfache Prüfung mit `.IsValid`

### Warum ValidateEventInternal?

**Aggregation:** Alle Checks an einem Ort  
**Fail-Fast:** Wirft Exception beim ERSTEN Fehler  
**Factory-Methode:** Wird in `Event.CreateAsync()` aufgerufen

---

## Verwendung in Factory-Methode

```csharp
// In Event.cs:
public static async Task<Event> CreateAsync(
    int venueId,
    DateTime dateTime,
    int maxAttendees,
    Venue venue,  // ← Brauchen wir für venueCapacity!
    IEventUniquenessChecker uc,
    CancellationToken ct = default)
{
    // HIER wird ValidateEventInternal aufgerufen:
    EventSpecifications.ValidateEventInternal(
        venueId, 
        dateTime, 
        maxAttendees, 
        venue.Capacity);  // ← Venue-Kapazität!
    
    // ... rest of factory method
}
```

---

## Häufige Fehler

### ❌ Falsch: venueId >= 0
```csharp
if (venueId >= 0)  // 0 ist NICHT gültig!
```

### ✅ Richtig: venueId > 0
```csharp
if (venueId <= 0)  // Nur > 0 ist gültig
```

---

### ❌ Falsch: DateTime.Now.Date
```csharp
if (dateTime < DateTime.Now.Date)  // Nur Datum, keine Zeit!
```

### ✅ Richtig: DateTime.Now
```csharp
if (dateTime < DateTime.Now)  // Mit Zeit!
```

---

### ❌ Falsch: maxAttendees >= venueCapacity
```csharp
if (maxAttendees >= venueCapacity)  // Gleich ist OK!
```

### ✅ Richtig: maxAttendees > venueCapacity
```csharp
if (maxAttendees > venueCapacity)  // Nur mehr ist falsch
```

---

## Testen

```powershell
cd Domain.Tests
dotnet test --filter "EventSpecifications"
```

**Erwartete Ergebnisse:**
- ✅ CheckVenueId_ValidId_ReturnsValid
- ✅ CheckVenueId_InvalidId_ReturnsInvalid
- ✅ CheckDateTime_FutureDate_ReturnsValid
- ✅ CheckDateTime_PastDate_ReturnsInvalid
- ✅ CheckMaxAttendees_WithinCapacity_ReturnsValid
- ✅ CheckMaxAttendees_ExceedsCapacity_ReturnsInvalid

---

## Warum ist das richtig?

1. **Domain-Validierung:** Grundregeln, immer gültig
2. **Fail-Fast:** Sofort Exception bei Fehler
3. **Clean:** Keine ungültigen Objekte möglich
4. **Testbar:** Jede Regel einzeln prüfbar
5. **Konsistent:** Gleiche Struktur wie VenueSpecifications

---

**✅ Fertig! Diese Implementierung folgt EXAKT dem Template-Pattern!**

---

## Nächster Schritt

Jetzt kannst du die **Factory-Methode** in `Event.cs` implementieren:
→ Siehe Hints im Ordner `02-commands-handlers/`

