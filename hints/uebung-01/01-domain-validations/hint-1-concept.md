# Hint 1: EventSpecifications - Konzept

## Problem

Du sollst `EventSpecifications.cs` implementieren, aber weißt nicht genau wie?

## Was sind ValidationSpecifications?

ValidationSpecifications enthalten die **Domain-Validierungsregeln** für eine Entity. Sie prüfen, ob Properties **grundsätzlich gültig** sind - unabhängig vom UseCase.

## Pattern

Schaue dir das **vollständig implementierte Beispiel** an:

```
Domain/ValidationSpecifications/VenueSpecifications.cs
```

Dieses File zeigt dir **EXAKT** das Pattern, das du für Event verwenden sollst!

## Was musst du implementieren?

Für `EventSpecifications.cs` brauchst du **3 Check-Methoden**:

1. **CheckVenueId** - Ist die VenueId gültig?
2. **CheckDateTime** - Ist das DateTime valide?
3. **CheckMaxAttendeesNotExceedVenueCapacity** - Passt MaxAttendees zur Venue-Kapazität?

## Grundstruktur

Jede Check-Methode:
- Bekommt einen **Wert** als Parameter
- Prüft eine **Regel**
- Gibt ein `DomainValidationResult` zurück

```csharp
public static DomainValidationResult CheckXXX(TYPE value)
{
    if (/* Regel verletzt? */)
        return DomainValidationResult.Invalid("PropertyName", "Error Message");
    
    return DomainValidationResult.Valid();
}
```

## Nächste Schritte

1. Öffne `VenueSpecifications.cs` und studiere `CheckCapacity`
2. Verstehe das Pattern
3. Wende es auf Event an

## Weiter mit Hint 2?

Hint 2 zeigt dir die **genauen Signaturen** und **Validierungslogik**.

---

**Tipp:** IMMER zuerst das Beispiel anschauen! VenueSpecifications ist KOMPLETT implementiert.

