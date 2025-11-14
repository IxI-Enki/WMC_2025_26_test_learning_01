# 🎓 WMC Test-Übung: Event Management System

## 📋 Übersicht

Willkommen zur Übungsaufgabe für den WMC-Test! Diese Aufgabe simuliert die Prüfungssituation und deckt alle wichtigen Aspekte der Clean Architecture ab, die du im Unterricht gelernt hast.

**Domäne:** Event Management System mit drei Entitäten:

- 🏛️ **Venue** (Veranstaltungsort)
- 🎭 **Event** (Veranstaltung)
- 🎫 **Ticket** (Eintrittskarte)

---

## 🎯 Lernziele

Nach dieser Übung verstehst du:

- ✅ Clean Architecture mit 4 Layern (Domain, Application, Infrastructure, API)
- ✅ CQRS Pattern (Commands & Queries mit MediatR)
- ✅ Repository Pattern & Unit of Work
- ✅ Drei Ebenen der Validierung (Domain, FluentValidation, Application)
- ✅ Domain-Driven Design Konzepte
- ✅ Dependency Injection
- ✅ Entity Framework Core
- ✅ Unit Tests & Integration Tests

---

## 🏗️ Architektur-Übersicht

```architecture
┌─────────────────────────────────────────────────────────┐
│                    API Layer (Presentation)             │
│  - Controllers (VenuesController, EventsController)     │
│  - Dependency Injection Configuration                   │
└───────────────────────┬─────────────────────────────────┘
                        │
┌───────────────────────▼─────────────────────────────────┐
│              Application Layer (Use Cases)              │
│  - Commands & Queries (CQRS)                            │
│  - Command/Query Handlers                               │
│  - FluentValidation Validators                          │
│  - DTOs (Data Transfer Objects)                         │
│  - Interfaces (IUnitOfWork, IRepositories)              │
└───────────────────────┬─────────────────────────────────┘
                        │
┌───────────────────────▼─────────────────────────────────┐
│               Infrastructure Layer                      │
│  - EF Core DbContext                                    │
│  - Repository Implementations                           │
│  - Unit of Work Implementation                          │
│  - Migrations                                           │
└───────────────────────┬─────────────────────────────────┘
                        │
┌───────────────────────▼─────────────────────────────────┐
│                 Domain Layer (Core)                     │
│  - Entities (Venue, Event, Ticket)                      │
│  - Domain Validations                                   │
│  - Domain Exceptions                                    │
│  - Domain Contracts (Interfaces)                        │
└─────────────────────────────────────────────────────────┘
```

---

## 🎯 Deine Aufgaben

### ✏️ Teil 1: Domain-Validierungen (Event-Entität)

#### 📍 Aufgabe 1.1: EventSpecifications implementieren

**Datei:** `Domain/ValidationSpecifications/EventSpecifications.cs`

Implementiere die folgenden drei Methoden:

##### a) `CheckVenueId(int venueId)`

- **Regel:** VenueId muss größer als 0 sein
- **Fehlermeldung:** "VenueId muss größer als 0 sein."

##### b) `CheckDateTime(DateTime dateTime)`

- **Regel:** DateTime darf nicht in der Vergangenheit liegen
- **Vergleich:** `dateTime < DateTime.Now`
- **Fehlermeldung:** "DateTime darf nicht in der Vergangenheit liegen."

##### c) `CheckMaxAttendeesNotExceedVenueCapacity(int maxAttendees, int venueCapacity)`

- **Regel:** MaxAttendees darf die Venue-Kapazität nicht überschreiten
- **Vergleich:** `maxAttendees > venueCapacity`
- **Fehlermeldung:** "MaxAttendees darf die Venue-Kapazität nicht überschreiten."

**💡 Tipp:** Schaue dir `VenueSpecifications.cs` als Beispiel an!

**🧪 Tests:** `Domain.Tests/EventSpecificationsTests.cs` - Diese Tests sollten grün werden.

---

#### 📍 Aufgabe 1.2: Event.ValidateEventProperties implementieren

**Datei:** `Domain/Entities/Event.cs`

Implementiere die Methode `ValidateEventProperties`:

```csharp
public static void ValidateEventProperties(int venueId, string name, DateTime dateTime, 
    int maxAttendees, int venueCapacity)
{
    // TODO: Implementiere diese Methode
}
```

**Anforderungen:**

1. Erstelle eine Liste von `DomainValidationResult`
2. Füge folgende Validierungen hinzu:
   - `EventSpecifications.CheckVenueId(venueId)`
   - `EventSpecifications.CheckName(name)`
   - `EventSpecifications.CheckDateTime(dateTime)`
   - `EventSpecifications.CheckMaxAttendees(maxAttendees)`
   - `EventSpecifications.CheckMaxAttendeesNotExceedVenueCapacity(maxAttendees, venueCapacity)`
3. Iteriere über die Ergebnisse
4. Wirf eine `DomainValidationException` bei Fehlern

**💡 Tipp:** Schaue dir `Venue.ValidateVenueProperties` als Beispiel an!

**🧪 Tests:** `Domain.Tests/EventTests.cs` - Diese Tests sollten grün werden.

---

### ✏️ Teil 2: FluentValidation (Application Layer)

#### 📍 Aufgabe 2.1: CreateVenueCommandValidator implementieren

**Datei:** `Application/Features/Venues/Commands/CreateVenue/CreateVenueCommandValidator.cs`

Implementiere die Validierungsregeln:

```csharp
public CreateVenueCommandValidator()
{
    // TODO: Implementiere die Validierungsregeln
    
    // Name: NotEmpty, MinimumLength(3)
    // Address: NotEmpty, MinimumLength(5)
    // Capacity: GreaterThan(0)
}
```

**💡 Beispiel aus dem Projekt:**

```csharp
RuleFor(x => x.Name)
    .NotEmpty()
    .MinimumLength(3);
```

**💡 Tipp:** Schaue dir `UpdateVenueCommandValidator.cs` als vollständiges Beispiel an!

---

#### 📍 Aufgabe 2.2: CreateTicketCommandValidator implementieren

**Datei:** `Application/Features/Tickets/Commands/CreateTicket/CreateTicketCommandValidator.cs`

Implementiere die Validierungsregeln:

```csharp
public CreateTicketCommandValidator()
{
    // TODO: Implementiere die Validierungsregeln
    
    // EventId: GreaterThan(0)
    // BuyerName: NotEmpty, MinimumLength(2)
    // Price: GreaterThan(0)
}
```

---

### ✏️ Teil 3: Command & Query Handler (Application Layer)

#### 📍 Aufgabe 3.1: UpdateVenueCommandHandler implementieren

**Datei:** `Application/Features/Venues/Commands/UpdateVenue/UpdateVenueCommandHandler.cs`

Implementiere die `Handle` Methode:

```csharp
public async Task<Result<GetVenueDto>> Handle(UpdateVenueCommand request, 
    CancellationToken cancellationToken)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Hole die Venue aus dem Repository (GetByIdAsync)
    // 2. Prüfe, ob die Venue existiert (NotFoundException werfen falls nicht)
    // 3. Rufe entity.UpdateAsync(...) auf
    // 4. Rufe uow.Venues.Update(entity) auf
    // 5. Rufe uow.SaveChangesAsync auf
    // 6. Gib Result<GetVenueDto>.Success zurück (mit entity.Adapt<GetVenueDto>())
}
```

**💡 Tipp:** Schaue dir `CreateVenueCommandHandler.cs` als Beispiel an!

---

#### 📍 Aufgabe 3.2: GetVenueByIdQueryHandler implementieren

**Datei:** `Application/Features/Venues/Queries/GetVenueById/GetVenueByIdQueryHandler.cs`

Implementiere die `Handle` Methode:

```csharp
public async Task<Result<GetVenueDto>> Handle(GetVenueByIdQuery request, 
    CancellationToken cancellationToken)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Hole die Venue aus dem Repository (GetByIdAsync)
    // 2. Wenn null, gib Result<GetVenueDto>.NotFound zurück
    // 3. Sonst gib Result<GetVenueDto>.Success zurück (mit entity.Adapt<GetVenueDto>())
}
```

**💡 Tipp:** Schaue dir `GetAllVenuesQueryHandler.cs` als Beispiel an!

---

#### 📍 Aufgabe 3.3: CreateEventCommandHandler implementieren

**Datei:** `Application/Features/Events/Commands/CreateEvent/CreateEventCommandHandler.cs`

Implementiere die `Handle` Methode:

```csharp
public async Task<Result<GetEventDto>> Handle(CreateEventCommand request, 
    CancellationToken cancellationToken)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Hole die Venue aus dem Repository (GetByIdAsync)
    // 2. Prüfe, ob die Venue existiert (NotFoundException werfen falls nicht)
    // 3. Erstelle das Event über Event.Create(venue, request.Name, request.DateTime, request.MaxAttendees)
    // 4. Füge das Event zum Repository hinzu (AddAsync)
    // 5. Speichere die Änderungen (SaveChangesAsync)
    // 6. Gib Result<GetEventDto>.Created zurück (mit entity.Adapt<GetEventDto>())
}
```

**💡 Tipp:** Schaue dir `CreateVenueCommandHandler.cs` als Beispiel an!

---

### ✏️ Teil 4: Repository Implementation (Infrastructure Layer)

#### 📍 Aufgabe 4.1: TicketRepository implementieren

**Datei:** `Infrastructure/Persistence/Repositories/TicketRepository.cs`

Implementiere die beiden Methoden:

```csharp
public async Task<IReadOnlyCollection<Ticket>> GetByEventIdAsync(int eventId, CancellationToken ct = default)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Verwende Set.AsNoTracking()
    // 2. Filtere mit Where(t => t.EventId == eventId)
    // 3. Sortiere mit OrderBy(t => t.PurchaseDate)
    // 4. Führe aus mit ToListAsync(ct)
}

public async Task<int> GetTicketCountForEventAsync(int eventId, CancellationToken ct = default)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Verwende Set
    // 2. Zähle mit CountAsync(t => t.EventId == eventId, ct)
}
```

**💡 Tipp:** Schaue dir `EventRepository.cs` als Beispiel an!

---

### ✏️ Teil 5: Controller Endpoints (API Layer)

#### 📍 Aufgabe 5.1: VenuesController.GetById implementieren

**Datei:** `Api/Controllers/VenuesController.cs`

Implementiere die GetById Methode:

```csharp
/// <summary>
/// Liefert eine Venue per ID.
/// </summary>
[HttpGet("{id:int}")]
[ProducesResponseType(typeof(GetVenueDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetById(int id, CancellationToken ct)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Sende GetVenueByIdQuery mit mediator.Send(new GetVenueByIdQuery(id), ct)
    // 2. Gib result.ToActionResult(this) zurück
}
```

**💡 Tipp:** Schaue dir die `GetAll` Methode im selben Controller an!

---

#### 📍 Aufgabe 5.2: EventsController.Create implementieren

**Datei:** `Api/Controllers/EventsController.cs`

Implementiere die Create Methode:

```csharp
/// <summary>
/// Erstellt ein neues Event.
/// </summary>
[HttpPost]
[ProducesResponseType(typeof(GetEventDto), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> Create([FromBody] CreateEventCommand command, CancellationToken ct)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. Sende CreateEventCommand mit mediator.Send(command, ct)
    // 2. Gib result.ToActionResult(this) zurück
}
```

**💡 Tipp:** Schaue dir die `VenuesController.Create` Methode an!

---

## 🧪 Tests ausführen

### Unit Tests (Domain Layer)

```bash
cd CleanArchitecture_Uebung_01
dotnet test Domain.Tests/Domain.Tests.csproj
```

**Erwartete Tests:**

- ✅ `VenueTests` - sollten alle grün sein (Beispiel-Implementierung)
- ❌ `EventTests` - werden erst grün, wenn du Event-Validierungen implementiert hast
- ❌ `EventSpecificationsTests` - werden erst grün, wenn du EventSpecifications implementiert hast

### Integration Tests (API Layer)

```bash
dotnet test Api.Tests/Api.Tests.csproj
```

**Erwartete Tests:**

- ✅ `VenuesEndpointTests` - sollten alle grün sein

---

## 🚀 Projekt ausführen

### 1. Datenbank erstellen

```bash
cd CleanArchitecture_Uebung_01/Infrastructure
dotnet ef migrations add Initial --startup-project ../Api
dotnet ef database update --startup-project ../Api
```

### 2. API starten

```bash
cd CleanArchitecture_Uebung_01/Api
dotnet run
```

Die API läuft auf: `https://localhost:5001/swagger`

### 3. Swagger UI verwenden

Öffne deinen Browser und gehe zu `https://localhost:5001/swagger`

**Teste folgende Endpoints:**

- ✅ `POST /api/venues` - Erstelle einen Veranstaltungsort
- ✅ `GET /api/venues` - Hole alle Veranstaltungsorte
- ❌ `GET /api/venues/{id}` - Hole einen Veranstaltungsort (TODO)
- ✅ `PUT /api/venues/{id}` - Aktualisiere einen Veranstaltungsort (TODO: Handler)
- ✅ `DELETE /api/venues/{id}` - Lösche einen Veranstaltungsort
- ✅ `GET /api/events` - Hole alle Events
- ❌ `POST /api/events` - Erstelle ein Event (TODO)
- ✅ `POST /api/tickets` - Erstelle ein Ticket

---

## 📝 Validierungsregeln - Übersicht

### Venue

| Property          | Domain Validation      | FluentValidation           |
| ----------------- | ---------------------- | -------------------------- |
| Name              | NotEmpty, MinLength(3) | NotEmpty, MinimumLength(3) |
| Address           | NotEmpty, MinLength(5) | NotEmpty, MinimumLength(5) |
| Capacity          | >= 1                   | GreaterThan(0)             |
| Name (Uniqueness) | Unique                 | -                          |

### Event

| Property                 | Domain Validation      | FluentValidation           |
| ------------------------ | ---------------------- | -------------------------- |
| VenueId                  | > 0                    | GreaterThan(0)             |
| Name                     | NotEmpty, MinLength(3) | NotEmpty, MinimumLength(3) |
| DateTime                 | Not in past            | GreaterThan(DateTime.Now)  |
| MaxAttendees             | >= 1                   | GreaterThan(0)             |
| MaxAttendees vs Capacity | <= Venue.Capacity      | -                          |

### Ticket

| Property     | Domain Validation      | FluentValidation           |
| ------------ | ---------------------- | -------------------------- |
| EventId      | > 0                    | GreaterThan(0)             |
| BuyerName    | NotEmpty, MinLength(2) | NotEmpty, MinimumLength(2) |
| PurchaseDate | Not in future          | -                          |
| Price        | >= 0.01                | GreaterThan(0)             |

---

## 🎓 Design Patterns & Konzepte

### 1. Clean Architecture

- **4 Layer:** Domain, Application, Infrastructure, API
- **Dependency Rule:** Abhängigkeiten zeigen immer nach innen
- **Domain ist Core:** Keine Abhängigkeiten zu anderen Layern

### 2. CQRS (Command Query Responsibility Segregation)

- **Commands:** Ändern den State (Create, Update, Delete)
- **Queries:** Lesen den State (GetAll, GetById)
- **MediatR:** Entkopplung zwischen Controller und Business Logic

### 3. Repository Pattern

- **Abstraction:** `IVenueRepository`, `IEventRepository`, `ITicketRepository`
- **Implementation:** In Infrastructure Layer
- **GenericRepository:** Basis-CRUD-Operationen

### 4. Unit of Work Pattern

- **IUnitOfWork:** Aggregiert Repositories
- **SaveChanges:** Transaktionale Speicherung

### 5. Dependency Injection

- **Constructor Injection:** Alle Dependencies über Constructor
- **DI Container:** Microsoft.Extensions.DependencyInjection

### 6. Validation (3 Ebenen)

- **Domain Validation:** Business Rules in Entities
- **FluentValidation:** Input-Validierung in Application Layer
- **Database Constraints:** Unique Indexes, Foreign Keys

### 7. Result Pattern

- **`Result<T>`:** Wrapping von Success/Error States
- **ResultExtensions:** Mapping zu HTTP Status Codes

---

## 📚 Wichtige Dateien zum Lernen

### Vollständig implementiert (als Beispiel)

1. ✅ `Domain/Entities/Venue.cs` - Entity mit Domain Validations
2. ✅ `Domain/ValidationSpecifications/VenueSpecifications.cs` - Domain Validation Rules
3. ✅ `Application/Features/Venues/Commands/CreateVenue/` - Kompletter CQRS Flow
4. ✅ `Application/Features/Venues/Commands/UpdateVenue/UpdateVenueCommandValidator.cs` - FluentValidation
5. ✅ `Infrastructure/Persistence/Repositories/VenueRepository.cs` - Repository Implementation
6. ✅ `Api/Controllers/VenuesController.cs` - Controller mit allen Endpoints (außer GetById)

### Mit Lücken (deine Aufgabe)

1. ❌ `Domain/ValidationSpecifications/EventSpecifications.cs`
2. ❌ `Domain/Entities/Event.cs` (ValidateEventProperties)
3. ❌ `Application/Features/Venues/Commands/CreateVenue/CreateVenueCommandValidator.cs`
4. ❌ `Application/Features/Tickets/Commands/CreateTicket/CreateTicketCommandValidator.cs`
5. ❌ `Application/Features/Venues/Commands/UpdateVenue/UpdateVenueCommandHandler.cs`
6. ❌ `Application/Features/Venues/Queries/GetVenueById/GetVenueByIdQueryHandler.cs`
7. ❌ `Application/Features/Events/Commands/CreateEvent/CreateEventCommandHandler.cs`
8. ❌ `Infrastructure/Persistence/Repositories/TicketRepository.cs`
9. ❌ `Api/Controllers/VenuesController.cs` (GetById)
10. ❌ `Api/Controllers/EventsController.cs` (Create)

---

## ✅ Checkliste

Verwende diese Checkliste, um deinen Fortschritt zu verfolgen:

### Domain Layer

- [ ] EventSpecifications.CheckVenueId implementiert
- [ ] EventSpecifications.CheckDateTime implementiert
- [ ] EventSpecifications.CheckMaxAttendeesNotExceedVenueCapacity implementiert
- [ ] Event.ValidateEventProperties implementiert
- [ ] Domain.Tests: EventSpecificationsTests grün
- [ ] Domain.Tests: EventTests grün

### Application Layer (Validators)

- [ ] CreateVenueCommandValidator implementiert
- [ ] CreateTicketCommandValidator implementiert

### Application Layer (Handlers)

- [ ] UpdateVenueCommandHandler.Handle implementiert
- [ ] GetVenueByIdQueryHandler.Handle implementiert
- [ ] CreateEventCommandHandler.Handle implementiert

### Infrastructure Layer

- [ ] TicketRepository.GetByEventIdAsync implementiert
- [ ] TicketRepository.GetTicketCountForEventAsync implementiert

### API Layer

- [ ] VenuesController.GetById implementiert
- [ ] EventsController.Create implementiert

### Tests & Ausführung

- [ ] Alle Unit Tests grün
- [ ] Alle Integration Tests grün
- [ ] Projekt kompiliert ohne Fehler
- [ ] Datenbank Migration erfolgreich
- [ ] API läuft und Swagger ist erreichbar
- [ ] Alle Endpoints in Swagger getestet

---

## 🎯 Tipps für den Test

1. **Template zur Hilfe:** Du darfst das Original-Template (`CleanArchitecture_Template`) verwenden!
2. **Tests first:** Schaue dir zuerst die Tests an, um zu verstehen, was erwartet wird
3. **Patterns erkennen:** Viele Implementierungen folgen dem gleichen Pattern
4. **Beispiele nutzen:** Schaue dir die vollständig implementierten Teile als Beispiel an
5. **Compiler Errors:** Starte mit den Compiler Errors - sie zeigen dir, was fehlt
6. **Schritt für Schritt:** Arbeite die Aufgaben der Reihe nach ab

---

## 🤔 Häufige Fragen

### Wie verwende ich Mapster?

```csharp
var dto = entity.Adapt<GetVenueDto>();
```

### Wie werfe ich eine NotFoundException?

```csharp
if (entity == null)
    throw new NotFoundException($"Venue with ID {id} not found.");
```

### Wie verwende ich den UnitOfWork?

```csharp
var entity = await uow.Venues.GetByIdAsync(id, ct);
await uow.Venues.AddAsync(entity, ct);
uow.Venues.Update(entity);
await uow.SaveChangesAsync(ct);
```

### Wie erstelle ich ein Result?

```csharp
return Result<GetVenueDto>.Success(dto);
return Result<GetVenueDto>.NotFound("Not found");
return Result<GetVenueDto>.Created(dto);
```

---

## 🎉 Viel Erfolg 🎉

Diese Übung bereitet dich optimal auf den Test vor. Alle Patterns und Konzepte, die hier vorkommen, sind relevant für die Prüfung.

> **Viel Erfolg! 🚀**

---

## 📞 Kontakt

Bei Fragen zur Aufgabenstellung, schaue dir das Template-Projekt an oder frage deinen Lehrer.

---

**Erstellt für WMC Test-Vorbereitung 2025** 🎓
