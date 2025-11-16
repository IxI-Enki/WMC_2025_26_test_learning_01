# Hint 2: EventSpecifications - Struktur & Pattern

## Schritt-für-Schritt

### 1. CheckVenueId

**Signatur:**
```csharp
public static DomainValidationResult CheckVenueId(int venueId)
```

**Regel:**
- VenueId MUSS größer als 0 sein

**Logik:**
```csharp
if (venueId <= 0)
    return DomainValidationResult.Invalid("VenueId", "VenueId muss größer als 0 sein.");

return DomainValidationResult.Valid();
```

---

### 2. CheckDateTime

**Signatur:**
```csharp
public static DomainValidationResult CheckDateTime(DateTime dateTime)
```

**Regel:**
- DateTime darf NICHT in der Vergangenheit liegen

**Logik:**
```csharp
if (dateTime < DateTime.Now)
    return DomainValidationResult.Invalid("DateTime", "DateTime darf nicht in der Vergangenheit liegen.");

return DomainValidationResult.Valid();
```

---

### 3. CheckMaxAttendeesNotExceedVenueCapacity

**Signatur:**
```csharp
public static DomainValidationResult CheckMaxAttendeesNotExceedVenueCapacity(
    int maxAttendees, 
    int venueCapacity)
```

**Regel:**
- MaxAttendees darf die Venue-Kapazität NICHT überschreiten

**Logik:**
```csharp
if (maxAttendees > venueCapacity)
    return DomainValidationResult.Invalid(
        "MaxAttendees", 
        "MaxAttendees darf die Venue-Kapazität nicht überschreiten.");

return DomainValidationResult.Valid();
```

---

## ValidateEventInternal

Diese Methode **aggregiert alle Check-Methoden**:

```csharp
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
```

---

## Wichtige Punkte

⚠️ **Property-Namen müssen exakt übereinstimmen** (z.B. "VenueId", nicht "venueId")

⚠️ **Error Messages sind klar und hilfreich**

⚠️ **ValidateEventInternal wirft Exception bei ERSTEM Fehler**

---

## Testen

Nach der Implementierung:

```powershell
cd Domain.Tests
dotnet test --filter "EventSpecifications"
```

Alle Tests sollten grün werden! ✅

---

## Weiter mit Hint 3?

Hint 3 zeigt dir den **vollständigen Code** mit Erklärungen.

