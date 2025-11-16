# 📚 Event Management System - Lösungshinweise

## ✅ Implementierungsstatus (solution/uebung-01)

Dieser Branch enthält eine **teilweise implementierte Lösung** der Event Management System Übung.

### 📊 Status-Übersicht

| Entity | Domain | Commands | Queries | Controller | Status |
|--------|--------|----------|---------|------------|--------|
| **Venue** | ✅ Komplett | ✅ Create, Update, Delete | ✅ GetAll, GetById | ✅ Alle Actions | ✅ **100%** |
| **Event** | ⚠️ Teilweise | ✅ Create | ✅ GetAll | ⚠️ Teilweise | ⚠️ **40%** |
| **Ticket** | ⚠️ Teilweise | ✅ Create | ❌ TODO | ⚠️ Teilweise | ⚠️ **30%** |

---

## 🎯 Vollständig implementiert: Venue

### Domain Layer

**Venue.cs** - Vollständige Factory-Methode:
```csharp
public static async Task<Venue> CreateAsync(
    string name,
    string location,
    int capacity,
    IVenueUniquenessChecker uc,
    CancellationToken ct = default)
{
    // 1. TRIM
    var trimmedName = (name ?? string.Empty).Trim();
    var trimmedLocation = (location ?? string.Empty).Trim();
    
    // 2. INTERNAL VALIDATION
    VenueSpecifications.ValidateVenueInternal(trimmedName, trimmedLocation, capacity);
    
    // 3. EXTERNAL VALIDATION
    await VenueSpecifications.ValidateVenueExternal(0, trimmedName, uc, ct);
    
    // 4. CREATE
    return new Venue
    {
        Name = trimmedName,
        Location = trimmedLocation,
        Capacity = capacity
    };
}
```

**VenueSpecifications.cs** - Vollständig:
- `CheckName` - Name muss min. 2 Zeichen haben
- `CheckLocation` - Location muss min. 2 Zeichen haben
- `CheckCapacity` - Capacity muss > 0 sein
- `ValidateVenueInternal` - Aggregiert alle Checks
- `ValidateVenueExternal` - Uniqueness-Check

### Application Layer

**Commands:**
- ✅ CreateVenueCommand + Handler + Validator
- ✅ UpdateVenueCommand + Handler + Validator
- ✅ DeleteVenueCommand + Handler

**Queries:**
- ✅ GetAllVenuesQuery + Handler
- ✅ GetVenueByIdQuery + Handler + Validator

**DTO:**
- ✅ GetVenueDto

**Service:**
- ✅ VenueUniquenessChecker

### API Layer

**VenuesController** - Alle CRUD-Actions:
```csharp
[HttpPost] Create       → 201 Created
[HttpGet] GetAll        → 200 OK
[HttpGet("{id}")] GetById → 200 OK / 404
[HttpPut("{id}")] Update  → 200 OK / 404
[HttpDelete("{id}")] Delete → 204 No Content / 404
```

---

## ⚠️ Teilweise implementiert: Event

### Was ist vorhanden?

#### Domain Layer
- ✅ Event.cs Entity (Gerüst)
- ⚠️ EventSpecifications.cs (TODO - siehe Hints!)
- ❌ Factory-Methode fehlt

#### Application Layer
- ✅ CreateEventCommand + Handler + Validator
- ✅ GetAllEventsQuery + Handler
- ❌ UpdateEventCommand fehlt
- ❌ DeleteEventCommand fehlt
- ❌ GetEventByIdQuery fehlt

#### API Layer
- ⚠️ EventsController (nur Create und GetAll)

### Was fehlt noch?

1. **EventSpecifications implementieren:**
   ```
   - CheckVenueId(int venueId)
   - CheckDateTime(DateTime dateTime)
   - CheckMaxAttendeesNotExceedVenueCapacity(int maxAttendees, int venueCapacity)
   - ValidateEventInternal(...)
   ```

2. **Event.CreateAsync Factory-Methode:**
   ```csharp
   public static async Task<Event> CreateAsync(
       int venueId,
       string title,
       DateTime dateTime,
       int maxAttendees,
       Venue venue,
       IEventUniquenessChecker uc,
       CancellationToken ct = default)
   {
       // TODO: Validation & Create
   }
   ```

3. **Fehlende Commands:**
   - UpdateEventCommand + Handler + Validator
   - DeleteEventCommand + Handler

4. **Fehlende Queries:**
   - GetEventByIdQuery + Handler

5. **Controller vervollständigen:**
   - GetById Action
   - Update Action
   - Delete Action

---

## ⚠️ Minimal implementiert: Ticket

### Was ist vorhanden?

- ✅ Ticket.cs Entity (Gerüst)
- ⚠️ TicketSpecifications.cs (teilweise)
- ✅ CreateTicketCommand + Handler + Validator

### Was fehlt?

1. **Ticket Factory-Methode**
2. **TicketSpecifications vervollständigen**
3. **Queries** (GetAll, GetById)
4. **Commands** (Update, Delete)
5. **Controller** vervollständigen

---

## 💡 Lern-Schwerpunkte (basierend auf Venue)

### 1. Factory Method Pattern

**Warum Factory-Methode statt Constructor?**

```csharp
// ❌ FALSCH (kein Schutz gegen ungültige Objekte):
var venue = new Venue 
{ 
    Name = "", // ← Ungültig! 
    Capacity = -5 // ← Ungültig!
};

// ✅ RICHTIG (Validation erzwingt gültige Objekte):
var venue = await Venue.CreateAsync(
    name: "",      // ← Wirft DomainValidationException!
    location: "...",
    capacity: -5,  // ← Wirft DomainValidationException!
    uc, ct);
```

**Vorteile:**
- **Keine ungültigen Objekte** möglich
- **Validation sofort** bei Erstellung
- **Async** für External Validation
- **Explizit** und klar

### 2. Domain Validations (3-Ebenen)

```ascii
┌─────────────────────────────────────────────────────────┐
│              VALIDATION LAYERS                          │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  1. DOMAIN (Internal)                                  │
│     └─ VenueSpecifications.ValidateVenueInternal       │
│        ├─ Grundregeln (Länge, Bereich)                 │
│        └─ Immer gültig, unabhängig vom UseCase         │
│                                                         │
│  2. DOMAIN (External)                                  │
│     └─ VenueSpecifications.ValidateVenueExternal       │
│        ├─ Datenbank-Checks (Uniqueness)                │
│        └─ Via IVenueUniquenessChecker Interface        │
│                                                         │
│  3. APPLICATION (FluentValidation)                     │
│     └─ CreateVenueCommandValidator                     │
│        ├─ UseCase-spezifische Regeln                   │
│        └─ In MediatR Pipeline                          │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### 3. CQRS Pattern

**Commands** (ändern Daten):
```csharp
CreateVenueCommand  → Result<GetVenueDto>
UpdateVenueCommand  → Result<GetVenueDto>
DeleteVenueCommand  → Result<bool>
```

**Queries** (lesen Daten):
```csharp
GetAllVenuesQuery   → Result<IReadOnlyCollection<GetVenueDto>>
GetVenueByIdQuery   → Result<GetVenueDto>
```

**Strikte Trennung!**

### 4. Result Pattern

```csharp
// In Handler:
return Result<GetVenueDto>.Created(dto);  // → 201
return Result<GetVenueDto>.Success(dto);  // → 200
return Result<bool>.NoContent();          // → 204
return Result<T>.NotFound("...");         // → 404
return Result<T>.Conflict("...");         // → 409
```

**In Controller:**
```csharp
var result = await mediator.Send(command, ct);
return result.ToActionResult(this);  // ← Automatische HTTP Status Codes!
```

---

## 🧪 Testing (Venue als Beispiel)

### Domain Tests

```csharp
// In Domain.Tests/VenueTests.cs:

[Fact]
public async Task CreateAsync_WithValidData_CreatesVenue()
{
    // Arrange
    var name = "TestVenue";
    var location = "TestLocation";
    var capacity = 100;
    var uc = new Mock<IVenueUniquenessChecker>();
    uc.Setup(x => x.IsUniqueAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(true);
    
    // Act
    var venue = await Venue.CreateAsync(name, location, capacity, uc.Object, CancellationToken.None);
    
    // Assert
    Assert.Equal(name, venue.Name);
    Assert.Equal(location, venue.Location);
    Assert.Equal(capacity, venue.Capacity);
}
```

### API Tests (Integration)

```powershell
cd Api
dotnet run
# Swagger: http://localhost:5100/swagger
```

**Test-Szenarien:**
1. ✅ POST /api/Venues → 201 Created
2. ✅ GET /api/Venues → 200 OK mit Liste
3. ✅ GET /api/Venues/{id} → 200 OK
4. ✅ PUT /api/Venues/{id} → 200 OK
5. ✅ DELETE /api/Venues/{id} → 204 No Content

---

## 🔍 Häufige Fehler (basierend auf Venue-Implementierung)

### 1. Validation nach Factory

```csharp
// ❌ FALSCH:
var venue = new Venue { Name = name };
ValidateVenue(venue);  // Zu spät! Objekt existiert schon!

// ✅ RICHTIG:
var venue = await Venue.CreateAsync(name, location, capacity, uc, ct);
// Validation ist TEIL der Factory!
```

### 2. Keine External Validation

```csharp
// ❌ FALSCH:
public static Venue Create(string name, ...)
{
    ValidateVenueInternal(name, ...);
    // ← Uniqueness-Check fehlt!
    return new Venue { ... };
}

// ✅ RICHTIG:
public static async Task<Venue> CreateAsync(string name, ..., IVenueUniquenessChecker uc, ...)
{
    ValidateVenueInternal(name, ...);
    await ValidateVenueExternal(0, name, uc, ct);  // ← WICHTIG!
    return new Venue { ... };
}
```

### 3. Entity statt DTO zurückgeben

```csharp
// ❌ FALSCH (Controller):
return Ok(venue);  // ← Entity nach außen!

// ✅ RICHTIG:
return Ok(venue.Adapt<GetVenueDto>());  // ← DTO nach außen!
```

---

## 📝 TODO für vollständige Lösung

### Event vervollständigen

- [ ] EventSpecifications implementieren (siehe Hints!)
- [ ] Event.CreateAsync Factory-Methode
- [ ] UpdateEventCommand + Handler + Validator
- [ ] DeleteEventCommand + Handler
- [ ] GetEventByIdQuery + Handler
- [ ] EventsController vervollständigen
- [ ] Tests schreiben

### Ticket vervollständigen

- [ ] Ticket.CreateAsync Factory-Methode
- [ ] TicketSpecifications vervollständigen
- [ ] UpdateTicketCommand + Handler + Validator
- [ ] DeleteTicketCommand + Handler
- [ ] GetAllTicketsQuery + Handler
- [ ] GetTicketByIdQuery + Handler
- [ ] TicketsController vervollständigen
- [ ] Tests schreiben

---

## 🎓 Als Student nutzen

**Wenn du mit Übung 01 arbeitest:**

1. **Schaue dir Venue an** - Das ist das vollständige Beispiel!
2. **Verstehe das Pattern** - Alle Entities folgen dem gleichen Muster
3. **Implementiere Event** - Nutze Venue als Vorlage
4. **Implementiere Ticket** - Gleiches Pattern
5. **Teste alles** - Domain Tests + Swagger

**Hints verfügbar:**
- `support/hints` Branch → `hints/uebung-01/`

---

## 🏗️ Architektur-Highlights

### Clean Architecture Layers

```ascii
API Layer (VenuesController)
    ↓ (MediatR)
Application Layer (Commands/Queries/Handlers)
    ↓ (IUnitOfWork)
Infrastructure Layer (Repositories)
    ↓ (EF Core)
Database
```

### Dependency Inversion

```
Api → Application → Domain
  ↓
Infrastructure → Domain
```

**Domain hat KEINE Abhängigkeiten!**

---

## ✅ Erfolgskriterien

**Für vollständige Implementierung (100%):**

- [x] Venue: Domain + Application + API komplett ✅
- [ ] Event: Domain + Application + API komplett ⚠️
- [ ] Ticket: Domain + Application + API komplett ⚠️
- [x] Alle Validations (3 Ebenen) ✅ (für Venue)
- [x] Factory-Methoden mit Validation ✅ (für Venue)
- [x] CQRS Pattern ✅ (für Venue)
- [x] Result Pattern ✅
- [x] Repository Pattern ✅
- [ ] Alle Tests grün ⚠️
- [x] Swagger funktioniert ✅

**Aktueller Stand:** ~60% komplett

---

**Branch:** solution/uebung-01  
**Status:** ⚠️ Teilweise implementiert (Venue komplett, Event/Ticket teilweise)  
**Verwendung:** Als Referenz für Venue-Implementierung  
**Version:** 1.0  
**Last Updated:** 2025-11-16

