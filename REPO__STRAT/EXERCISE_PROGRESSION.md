<!-- markdownlint-disable -->
# Exercise Progression Strategy

## Übersicht der Schwierigkeitsgrade

Dieses Dokument definiert die präzise Progression von einfachen zu komplexen Übungen im WMC Clean Architecture Repository.

---

## Aufbau-Prinzipien

```ascii
┌────────────────────────────────────────────────────────┐
│              SCAFFOLDING PRINCIPLE                     │
│         (Gerüst-Prinzip für Lern-Progression)          │
├────────────────────────────────────────────────────────┤
│                                                        │
│  LEVEL 1: Maximum Guidance (Guided Implementation)     │
│  ┌───────────────────────────────────────┐             │
│  │  Vollständige Struktur                │             │
│  │    1-2 komplette Beispiele           │             │
│  │    TODO: Methoden-Implementierungen  │             │
│  └───────────────────────────────────────┘             │
│            ↓ Gerüst wird reduziert                     │
│                                                        │
│  LEVEL 2: Structural Guidance                          │
│  ┌────────────────────────────────────────┐            │
│  │    Ordnerstruktur komplett            │            │
│  │    Klassen: Nur Gerüste               │            │
│  │    TODO: Komplette Implementierungen  │            │
│  └────────────────────────────────────────┘            │
│            ↓ Nur noch Konzept vorgegeben               │
│                                                        │
│  LEVEL 3: Conceptual Guidance                          │
│  ┌────────────────────────────────────────┐            │
│  │    README mit Anforderungen           │            │
│  │    Tests als Spezifikation            │            │
│  │    TODO: Alles andere                 │            │
│  └────────────────────────────────────────┘            │
│            ↓ Prüfungssituation                         │
│                                                        │
│  LEVEL 4: Real-World Scenario (Exam)                   │
│  ┌────────────────────────────────────────┐            │
│  │    Nur Anforderungsdokument           │            │
│  │    TODO: Komplette Implementierung    │            │
│  │    Zeitlimit: 3-4 Stunden             │            │
│  └────────────────────────────────────────┘            │
│                                                        │
└────────────────────────────────────────────────────────┘
```

---

##   Level 1: Guided Implementation

###  Ziel

**Student lernt:** Die MUSTER und STRUKTUREN von Clean Architecture

**Fokus:** Domain Validations, CQRS-Commands, Controller-Actions

<!--

             
    
             
             
            
    

-->

###   Beispiel: Event Management System

```ascii
CleanArchitecture_Uebung_01/
│
├─   Domain/
│  ├─ Entities/
│  │  ├─ Venue.cs                        KOMPLETT (als Beispiel)
│  │  ├─ Event.cs                        Factory-Methode TODO
│  │  └─ Ticket.cs                       Factory-Methode TODO
│  │
│  ├─ ValidationSpecifications/
│  │  ├─ VenueSpecifications.cs          KOMPLETT (als Beispiel)
│  │  ├─ EventSpecifications.cs          3 Check-Methoden TODO
│  │  └─ TicketSpecifications.cs         2 Check-Methoden TODO
│  │
│  └─ Contracts/
│     ├─ IVenueUniquenessChecker.cs      VORHANDEN
│     ├─ IEventUniquenessChecker.cs      TODO: Interface erstellen
│     └─ ITicketUniquenessChecker.cs     TODO: Interface erstellen
│
├─   Application/
│  ├─ Features/
│  │  ├─ Venues/                         KOMPLETT (als Beispiel)
│  │  │  ├─ Commands/
│  │  │  │  ├─ CreateVenue/
│  │  │  │  │  ├─ CreateVenueCommand.cs            
│  │  │  │  │  ├─ CreateVenueCommandHandler.cs     
│  │  │  │  │  └─ CreateVenueCommandValidator.cs   
│  │  │  │  ├─ UpdateVenue/                          KOMPLETT
│  │  │  │  └─ DeleteVenue/                          KOMPLETT
│  │  │  └─ Queries/
│  │  │     ├─ GetAllVenues/                         KOMPLETT
│  │  │     └─ GetVenueById/                         KOMPLETT
│  │  │
│  │  ├─ Events/                                     NUR Ordner-Struktur
│  │  │  ├─ Commands/
│  │  │  │  ├─ CreateEvent/
│  │  │  │  │  ├─ CreateEventCommand.cs              TODO
│  │  │  │  │  ├─ CreateEventCommandHandler.cs       TODO
│  │  │  │  │  └─ CreateEventCommandValidator.cs     TODO
│  │  │  │  └─ DeleteEvent/            TODO
│  │  │  └─ Queries/
│  │  │     ├─ GetAllEvents/           Query TODO, Handler TODO
│  │  │     └─ GetEventById/           TODO
│  │  │
│  │  └─ Tickets/                      NUR Ordner-Struktur
│  │     ├─ Commands/CreateTicket/     TODO
│  │     └─ Queries/GetAllTickets/     TODO
│  │
│  └─ Services/
│     ├─ VenueUniquenessChecker.cs     KOMPLETT
│     ├─ EventUniquenessChecker.cs     TODO
│     └─ TicketUniquenessChecker.cs    TODO
│
├─   Infrastructure/                  KOMPLETT VORGEGEBEN
│  ├─ Persistence/
│  │  ├─ AppDbContext.cs               Alle Entities konfiguriert
│  │  ├─ Repositories/                 Alle Repos vorhanden
│  │  └─ UnitOfWork.cs                 Fertig
│  └─ Services/
│     └─ StartupDataSeeder.cs          CSV-Import fertig
│
├─   Api/
│  └─ Controllers/
│     ├─ VenuesController.cs           KOMPLETT (als Beispiel)
│     ├─ EventsController.cs           5 Actions TODO
│     └─ TicketsController.cs          3 Actions TODO
│
├─   Domain.Tests/
│  ├─ VenueTests.cs                    KOMPLETT
│  ├─ EventTests.cs                    Tests vorhanden (rot)
│  └─ TicketTests.cs                   Tests vorhanden (rot)
│
├─   Api.Tests/
│  └─ VenuesEndpointTests.cs           Integration Tests
│
└─   README.md                        SEHR AUSFÜHRLICH
   ├─ Lernziele
   ├─ Architektur-Übersicht
   ├─ Aufgaben mit Beispielen
   ├─ Schritt-für-Schritt Anleitung
   └─ Testing & Submission
```

###  Was Student implementiert

| Bereich            | Was TODO                          | Beispiel vorhanden        | Tests vorhanden |
| ------------------ | --------------------------------- | ------------------------- | --------------- |
| Domain Validations |   3 EventSpecifications-Methoden |   VenueSpecifications    |   Domain.Tests |
| Entity Factory     |   Event.CreateAsync()            |   Venue.CreateAsync()    |   Domain.Tests |
| Commands           |   CreateEvent, DeleteEvent       |   Venue-Commands         |   Zu erstellen |
| Queries            |   GetAllEvents, GetEventById     |   Venue-Queries          |   Zu erstellen |
| Controller         |   5 Actions in EventsController  |   VenuesController       |   Api.Tests    |
| UniquenessChecker  |   EventUniquenessChecker         |   VenueUniquenessChecker | -               |
| DI Registration    |   In DependencyInjection.cs      |   Beispiel vorhanden     | -               |

###   README-Struktur für Level 1

```markdown
# Event Management System - Clean Architecture Übung

## Lernziele
(Was lerne ich hier?)

## Architektur-Übersicht
(Diagramm mit allen Layers)

## Teil 1: Domain-Validierungen
### Aufgabe 1.1: EventSpecifications implementieren
- Schritt-für-Schritt Anleitung
- Code-Beispiel von VenueSpecifications
- Expected Result
- Tests zum Überprüfen

### Aufgabe 1.2: Event.CreateAsync() implementieren
- Factory-Methode Pattern erklärt
- Venue.CreateAsync() als Beispiel
- Validierungs-Reihenfolge
- Tests zum Überprüfen

## Teil 2: Application Layer - Commands
### Aufgabe 2.1: CreateEventCommand erstellen
- CQRS Pattern erklärt
- CreateVenueCommand als Beispiel
- Properties definieren
- IRequest<Result<T>> Interface

### Aufgabe 2.2: CreateEventCommandHandler implementieren
- Handler-Pattern erklärt
- CreateVenueCommandHandler als Beispiel
- UnitOfWork verwenden
- Error Handling

### Aufgabe 2.3: CreateEventCommandValidator
- FluentValidation erklärt
- CreateVenueCommandValidator als Beispiel
- Validation Rules

## Teil 3: API Layer - Controller
### Aufgabe 3.1: EventsController.Create()
- REST Conventions
- VenuesController.Create() als Beispiel
- Result Pattern
- HTTP Status Codes

## Testing
- Wie starte ich die Tests?
- Wie teste ich über Swagger?
- Expected Results

## Submission Checklist
□ Alle Tests grün
□ Swagger funktioniert
□ Code kompiliert ohne Warnings
```

###  Erfolgskriterien Level 1

- [ ] Student kann Domain Validations nach Muster implementieren
- [ ] Student versteht Factory-Methoden Pattern
- [ ] Student kann Commands/Handlers nach Vorlage erstellen
- [ ] Student versteht CQRS-Separation
- [ ] Student kann Controller-Actions nach REST implementieren
- [ ] Student versteht Result-Pattern
- [ ] **Alle Unit Tests grün**
- [ ] **API über Swagger testbar**

---

##   Level 2: Structural Guidance

###  Ziel - Level 2

**Student lernt:** Selbstständig Commands/Queries anlegen, Repository-Methoden erweitern

**Fokus:** Navigation Properties, CSV-Seeding, Custom Repository Methods

###   Beispiel: Library Management System

```ascii
CleanArchitecture_Uebung_02/
│
├─   Domain/
│  ├─ Entities/
│  │  ├─ Author.cs                     NUR Gerüst:
│  │  │                                  public class Author : BaseEntity
│  │  │                                  {
│  │  │                                      // Properties TODO
│  │  │                                      // Factory-Methode TODO
│  │  │                                  }
│  │  │
│  │  ├─ Book.cs                       NUR Gerüst + Navigation Property:
│  │  │                                  public class Book : BaseEntity
│  │  │                                  {
│  │  │                                      // Properties TODO
│  │  │                                      public Author Author { get; set; } = null!;
│  │  │                                      // Factory-Methode TODO
│  │  │                                  }
│  │  │
│  │  └─ Loan.cs                       NUR Gerüst + Navigation Properties:
│  │                                     public class Loan : BaseEntity
│  │                                     {
│  │                                         public Book Book { get; set; } = null!;
│  │                                         // Weitere Properties TODO
│  │                                     }
│  │
│  ├─ ValidationSpecifications/
│  │  ├─ AuthorSpecifications.cs       LEER (nur using-Statements)
│  │  ├─ BookSpecifications.cs         LEER
│  │  └─ LoanSpecifications.cs         LEER
│  │
│  └─ Contracts/
│     ├─ IAuthorUniquenessChecker.cs  Interface vorgegeben
│     ├─ IBookUniquenessChecker.cs    Interface vorgegeben
│     └─ ILoanUniquenessChecker.cs    Interface vorgegeben
│
├─   Application/
│  ├─ Features/
│  │  ├─ Authors/                      NUR Ordner-Struktur:
│  │  │  ├─ Commands/
│  │  │  │  ├─ CreateAuthor/           (Ordner leer)
│  │  │  │  ├─ UpdateAuthor/           (Ordner leer)
│  │  │  │  └─ DeleteAuthor/           (Ordner leer)
│  │  │  └─ Queries/
│  │  │     ├─ GetAllAuthors/          (Ordner leer)
│  │  │     └─ GetAuthorById/          (Ordner leer)
│  │  │
│  │  ├─ Books/                        NUR Ordner-Struktur
│  │  └─ Loans/                        NUR Ordner-Struktur
│  │
│  ├─ Dtos/                            LEER (Student muss DTOs erstellen)
│  │
│  └─ Services/                        LEER
│
├─   Infrastructure/                  FAST KOMPLETT
│  ├─ Persistence/
│  │  ├─ AppDbContext.cs               OnModelCreating fertig
│  │  ├─ Repositories/
│  │  │  ├─ GenericRepository.cs       Fertig
│  │  │  ├─ AuthorRepository.cs        TODO: Custom-Methoden
│  │  │  ├─ BookRepository.cs          TODO: .Include() für Navigation
│  │  │  └─ LoanRepository.cs          TODO: Custom-Methoden
│  │  └─ UnitOfWork.cs                 Fertig
│  │
│  └─ Services/
│     └─ StartupDataSeeder.cs          CSV-Import fertig
│
├─   Api/
│  └─ Controllers/
│     ├─ AuthorsController.cs          NUR Gerüst:
│     │                                  [ApiController]
│     │                                  [Route("api/[controller]")]
│     │                                  public class AuthorsController(IMediator mediator) : ControllerBase
│     │                                  {
│     │                                      // Actions TODO
│     │                                  }
│     │
│     ├─ BooksController.cs            NUR Gerüst
│     └─ LoansController.cs            NUR Gerüst
│
├─   library_seed_data.csv            CSV-Daten vorgegeben
│
└─   README.md                        Aufgabenstellung (weniger detailliert)
   ├─ Aufgabe beschreiben
   ├─ Hinweis auf Template
   ├─ Keine Schritt-für-Schritt Anleitung!
   └─ Erfolgskriterien
```

###   Was Student implementiert

| Bereich           | Was TODO                                     | Hinweis                    | Schwierigkeit |
| ----------------- | -------------------------------------------- | -------------------------- | ------------- |
| Entities          |  Properties + Factory-Methoden              | Nur Gerüst vorhanden       |             |
| Validations       |  Alle Specifications                        | Komplett leer              |             |
| DTOs              |  GetAuthorDto, GetBookDto, GetLoanDto       | Müssen erstellt werden     |              |
| Commands          |  Create, Update, Delete für alle 3          | Ordner-Struktur vorgegeben |            |
| Queries           |  GetAll, GetById für alle 3                 | Ordner-Struktur vorgegeben |             |
| Handlers          |  Alle Command/Query Handler                 | Müssen erstellt werden     |            |
| Validators        |  Alle FluentValidation Validators           | Müssen erstellt werden     |             |
| Repository        |  BookRepository.GetByIdAsync mit .Include() | Navigation Property laden  |            |
| UniquenessChecker |  Alle 3 Checker                             | Interface vorgegeben       |             |
| Controller        |  Alle CRUD-Actions                          | Nur Gerüst vorhanden       |             |
| DI Registration   |  Alle Services registrieren                 | -                          |              |

###   README-Struktur für Level 2

```markdown
# Library Management System - Clean Architecture Übung

## Aufgabenstellung

Implementiere ein vollständiges Library Management System mit:
- Authors (Autoren)
- Books (Bücher mit ISBN-Validation)
- Loans (Ausleihen mit Overlap-Check)

## Anforderungen

### Domain Layer
- [ ] Entities mit Properties und Factory-Methoden
- [ ] ValidationSpecifications für alle Entities
- [ ] ISBN-10/13 Validation für Books
- [ ] Loan-Overlap Validation

### Application Layer
- [ ] CRUD Commands/Queries für alle Entities
- [ ] DTOs erstellen
- [ ] FluentValidation Validators
- [ ] UniquenessChecker Services

### Infrastructure
- [ ] BookRepository mit .Include(b => b.Author)
- [ ] Custom Repository-Methoden

### API
- [ ] Controller mit allen CRUD-Endpoints
- [ ] Swagger-Testing

## Hinweise
- Orientiere dich am CleanArchitecture_Template!
- BookRepository braucht .Include() für Navigation Properties
- ISBN-Validation: CheckDigit-Algorithmus
- Loan-Overlap: Datumsbereichs-Validation

## Testing
- Unit Tests für Domain
- Integration Tests für API
- Swagger: http://localhost:5100/swagger

## Erfolgskriterien
□ Alle Domain Tests grün
□ CSV-Seeding funktioniert (10 Books)
□ API über Swagger testbar
□ Navigation Properties korrekt geladen
```

###   Erfolgskriterien Level 2

- [ ] Student kann vollständige Entities selbstständig erstellen
- [ ] Student versteht Navigation Properties + EF Core .Include()
- [ ] Student kann komplette Feature-Folders aufbauen
- [ ] Student kann DTOs selbst definieren
- [ ] Student versteht Repository-Pattern Erweiterungen
- [ ] Student kann CSV-Seeding nutzen
- [ ] **Alle Tests grün**
- [ ] **Komplexe Validierungen (ISBN, Overlap) korrekt**

---

##   Level 3: Conceptual Guidance

###  Ziel

**Student lernt:** Komplette Anwendung von Grund auf aufbauen

**Fokus:** Architektur-Entscheidungen, Advanced Patterns

###   Beispiel: Device Management System

```ascii
CleanArchitecture_Uebung_03/
│
├─   README.md                         NUR Anforderungsdokument
│  ├─ Business-Requirements
│  ├─ Entities beschrieben
│  ├─ Validation-Rules
│  ├─ API-Endpoints definiert
│  └─ Erfolgskriterien
│
├─   DeviceManagement.sln              Leere Solution
│
└─   Tests/                            Tests als Spezifikation
   ├─ Domain.Tests/
   │  └─ DeviceSpecificationsTests.cs (Tests definieren Verhalten!)
   │
   └─ Api.Tests/
      └─ DevicesEndpointTests.cs

(Alles andere muss Student erstellen!)
```

###   README für Level 3 (devices.md Style)

```markdown
# Device Management System

## Lehrziele
- Fullstack-Anwendung mit REST-Interface
- Komplexe Validierungen (Buchungs-Overlaps)
- Advanced Repository Queries

## Anforderungen

Es ist eine Verwaltung von technischen Endgeräten zu implementieren.

### Entities
- **Device** (Name, SerialNumber, DeviceType)
- **Person** (FirstName, LastName, Email)
- **Usage** (Device, Person, FromDate, ToDate, ReturnDate)

### DeviceType Enum
- Smartphone
- Notebook
- Tablet

### Validierungen

#### Usage Validation
- Usages dürfen sich nicht überlappen (pro Device)
- ToDate >= FromDate
- Keine Buchung am gleichen Tag wie Rückgabe
- Person und Device müssen existieren
- Nur Zukunfts-Buchungen
- ReturnDate muss > ToDate sein

#### Device Validation
- Name: min. 2 Buchstaben
- SerialNumber: min. 3 Zeichen

#### Person Validation
- Alle Felder required
- Email syntaktisch korrekt
- Email unique (Uniqueness-Check)

### API Endpoints

#### Devices
- GET /api/devices
- GET /api/devices/{id}
- GET /api/devices/with-counts  ← Geräte mit Anzahl Buchungen
- POST /api/devices
- PUT /api/devices/{id}
- DELETE /api/devices/{id}

#### Persons
- (Standard CRUD)

#### Usages
- (Standard CRUD + Overlap-Validation)

### CSV-Import
- StartupDataSeeder mit usages.csv
- CLI beim Start: "Datenbank anlegen? (y/n)"

## Architektur
- Clean Architecture (4 Layer)
- CQRS Pattern
- Repository Pattern
- Domain Validations
- FluentValidation
- Swagger UI

## Testing
- Domain Tests
- Integration Tests
- Swagger: http://localhost:5100/swagger

## Hinweise
- Orientiere dich am Template
- Overlap-Check im Repository
- GetDevicesWithCounts: Custom Query mit .Include()
```

###   Was Student implementiert

**ALLES!**

- [ ] Projekt-Struktur (4 Layer-Projects)
- [ ] Entities von Grund auf
- [ ] Validations
- [ ] Commands/Queries/Handlers
- [ ] DTOs
- [ ] Repositories
- [ ] UniquenessChecker
- [ ] AppDbContext
- [ ] Migrations
- [ ] StartupDataSeeder
- [ ] Controller
- [ ] Exception Middleware
- [ ] Result Extensions
- [ ] DI Configuration
- [ ] Tests

###   Erfolgskriterien Level 3

- [ ] Student kann Solution-Struktur selbst aufbauen
- [ ] Student versteht alle Architektur-Entscheidungen
- [ ] Student kann komplexe Business-Logic implementieren
- [ ] Student kann Custom Repository Queries schreiben
- [ ] Student kann CLI-Interaktion implementieren
- [ ] **Overlap-Validation korrekt**
- [ ] **GetDevicesWithCounts Query funktioniert**
- [ ] **Alle Tests grün**

---

##   Level 4: Exam Scenario

###  Ziel

**Prüfungssituation:** Realistische Zeitbedingungen, vollständige Eigenleistung

**Zeitlimit:** 3-4 Stunden

###   Beispiel: Real-World Scenario

```markdown
# Inventory Management System - WMC Test

## Aufgabenstellung

(Ähnlich wie devices.md, aber neue Domain)

## Zeitrahmen
- 3,5 Stunden
- Keine Hilfen
- Nur Template als Referenz

## Bewertungskriterien
- Domain Validations (30%)
- CQRS Implementation (30%)
- API Endpoints (20%)
- Tests (20%)

## Abgabe
- ZIP-File mit kompletter Solution
- Alle Tests grün
- Swagger funktionsfähig
```

---

##   Progression-Matrix

| Aspect           | Level 1             | Level 2      | Level 3          | Level 4              |
| ---------------- | ------------------- | ------------ | ---------------- | -------------------- |
| **Entities**     |   Beispiel         |   Gerüst    |   Selbst        |   Selbst            |
| **Validations**  |   Teilweise        |   Leer      |   Selbst        |   Selbst            |
| **Commands**     |   TODO             |   Struktur  |   Selbst        |   Selbst            |
| **Handlers**     |   TODO             |   Selbst    |   Selbst        |   Selbst            |
| **Repositories** |   Fertig           |   Custom    |   Komplett      |   Komplett          |
| **Controller**   |   TODO             |   Gerüst    |   Selbst        |   Selbst            |
| **Tests**        |   Vorhanden        |   Vorhanden |   Spezifikation |   Bewertung         |
| **README**       |   Sehr detailliert |   Mittel    |   Minimal       |   Nur Anforderungen |
| **Zeitaufwand**  | ~4-6h               | ~6-8h        | ~10-12h          | ~3-4h (Prüfung)      |

---

##  Themen-Zuordnung

### Level 1: Event Management

-  Domain Validations (Basics)
-  CQRS Pattern
-  Factory-Methoden
-  REST APIs
-  Result Pattern
-  FluentValidation

### Level 2: Library Management

-  Navigation Properties
-  EF Core .Include()
-  CSV-Seeding
-  ISBN-Validation
-  Custom Repository Methods
-  Mapster Configuration

### Level 3: Device Management

-  Overlap-Validation
-  Complex Queries (with-counts)
-  Enum Types
-  CLI-Interaction
-  Complete Scaffolding
-  Email Validation

### Level 4: Exam

-  Time Pressure
-  Everything from Scratch
-  Real-World Scenario
-  No Guidance

---

##  Quality Gates pro Level

### Level 1

```checklist
□ dotnet build erfolg reich
□ Domain Tests grün
□ API Tests grün (für Venues)
□ Swagger läuft
□ README vollständig
```

### Level 2

```checklist
□ dotnet build erfolgreich
□ Alle Domain Tests grün
□ CSV-Seeding funktioniert
□ Navigation Properties geladen
□ Swagger läuft
```

### Level 3

```checklist
□ dotnet build erfolgreich
□ Solution-Struktur korrekt
□ Alle Tests grün
□ Complex Queries funktionieren
□ Swagger läuft
```

### Level 4

```checklist
□ Zeitlimit eingehalten
□ Build erfolgreich
□ Mind. 80% Tests grün
□ API funktionsfähig
```

---

**Version:** 1.0  
**Status:**   Bereit für Implementierung
