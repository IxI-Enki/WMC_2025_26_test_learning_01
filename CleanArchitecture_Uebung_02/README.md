# 🎓 WMC Test-Übung 02: Library Management System

## 📋 Übersicht

**Domäne:** Bibliotheksverwaltung mit drei Entitäten:
- 📚 **Book** (Buch) - *vollständig implementiert als Referenz*
- ✍️ **Author** (Autor) - *teilweise implementiert*
- 📖 **Loan** (Ausleihe) - *als Lückentext - DU musst implementieren!*

---

## 🎯 Deine Aufgaben

### ✏️ Teil 1: Domain-Validierungen (Loan)

#### 📍 Aufgabe 1.1: LoanSpecifications implementieren

**Datei:** `Domain/Specifications/LoanSpecifications.cs`

Implementiere folgende Methoden:

##### a) `CheckBookId(int bookId)`
- **Regel:** BookId muss größer als 0 sein
- **Fehlermeldung:** "BookId muss größer als 0 sein."

##### b) `CheckBorrowerName(string borrowerName)`
- **Regeln:**
  - Darf nicht leer sein
  - Muss mindestens `BorrowerNameMinLength` (2) Zeichen haben
- **Fehlermeldungen:**
  - "BorrowerName darf nicht leer sein."
  - "BorrowerName muss mindestens 2 Zeichen haben."

##### c) `CheckLoanDate(DateTime loanDate)`
- **Regel:** LoanDate darf nicht in der Zukunft liegen
- **Vergleich:** `loanDate > DateTime.Now`
- **Fehlermeldung:** "LoanDate darf nicht in der Zukunft liegen."

##### d) `ValidateLoanInternal(...)`
Implementiere die vollständige Validierungsmethode:
1. Erstelle eine Liste von `DomainValidationResult`
2. Rufe alle drei Check-Methoden auf
3. Iteriere über die Ergebnisse
4. Werfe `DomainValidationException` bei Fehlern

**💡 Tipp:** Schaue dir `BookSpecifications.ValidateBookInternal` als Beispiel an!

**🧪 Tests:** `Domain.Tests/LoanSpecificationsTests.cs`

---

#### 📍 Aufgabe 1.2: Loan.Create implementieren

**Datei:** `Domain/Entities/Loan.cs`

Implementiere die `Create`-Methode:

```csharp
public static Loan Create(Book book, string borrowerName, DateTime loanDate)
{
    // TODO: Implementiere diese Methode
    
    // Schritte:
    // 1. ArgumentNullException.ThrowIfNull(book)
    // 2. borrowerName trimmen
    // 3. LoanSpecifications.ValidateLoanInternal aufrufen
    // 4. DueDate = loanDate + 14 Tage berechnen
    // 5. Neues Loan-Objekt erstellen und zurückgeben
}
```

**💡 Tipp:** Schaue dir `Book.CreateAsync` als Beispiel an!

---

### ✏️ Teil 2: Repository-Methoden (Infrastructure Layer)

#### 📍 Aufgabe 2.1: LoanRepository implementieren

**Datei:** `Infrastructure/Persistence/Repositories/LoanRepository.cs`

Implementiere folgende Methoden:

##### a) `GetLoansByBookIdAsync`
```csharp
public async Task<IReadOnlyCollection<Loan>> GetLoansByBookIdAsync(int bookId, CancellationToken ct = default)
{
    // Alle Ausleihen für ein Buch
    // AsNoTracking, Include(l => l.Book), Where, OrderBy(l => l.LoanDate), ToListAsync
}
```

##### b) `GetActiveLoansByBorrowerAsync`
```csharp
public async Task<IReadOnlyCollection<Loan>> GetActiveLoansByBorrowerAsync(string borrowerName, CancellationToken ct = default)
{
    // Aktive Ausleihen (ReturnDate == null) für einen Ausleiher
    // AsNoTracking, Include(l => l.Book), Where, OrderBy(l => l.DueDate), ToListAsync
}
```

##### c) `GetOverdueLoansAsync`
```csharp
public async Task<IReadOnlyCollection<Loan>> GetOverdueLoansAsync(CancellationToken ct = default)
{
    // Überfällige Ausleihen (ReturnDate == null && DueDate < DateTime.Now)
    // AsNoTracking, Include(l => l.Book), Where, OrderBy(l => l.DueDate), ToListAsync
}
```

**💡 Tipp:** Schaue dir `BookRepository` als Beispiel an!

---

### ✏️ Teil 3: Commands & Queries (Application Layer)

Die Ordnerstruktur ist bereits angelegt in:
- `Application/Features/Loans/Commands/CreateLoan/`
- `Application/Features/Loans/Commands/ReturnLoan/`
- `Application/Features/Loans/Queries/GetLoansByBook/`
- `Application/Features/Loans/Queries/GetOverdueLoans/`

#### 📍 Aufgabe 3.1: CreateLoanCommand erstellen

Erstelle folgende Dateien in `Commands/CreateLoan/`:

##### CreateLoanCommand.cs
```csharp
public readonly record struct CreateLoanCommand(int BookId, string BorrowerName) 
    : IRequest<Result<GetLoanDto>>;
```

##### CreateLoanCommandHandler.cs
```csharp
public sealed class CreateLoanCommandHandler(IUnitOfWork uow) 
    : IRequestHandler<CreateLoanCommand, Result<GetLoanDto>>
{
    public async Task<Result<GetLoanDto>> Handle(...)
    {
        // 1. Buch laden (GetByIdAsync)
        // 2. Prüfen ob vorhanden (NotFoundException)
        // 3. Prüfen ob verfügbar (AvailableCopies > 0)
        // 4. Loan erstellen (Loan.Create mit DateTime.Now)
        // 5. Book.DecreaseCopies() aufrufen
        // 6. Loan hinzufügen (AddAsync)
        // 7. Book updaten (Update)
        // 8. Speichern (SaveChangesAsync)
        // 9. Result.Created zurückgeben
    }
}
```

##### CreateLoanCommandValidator.cs
```csharp
public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.BorrowerName).NotEmpty().MinimumLength(2);
    }
}
```

**💡 Tipp:** Schaue dir `CreateBookCommandHandler` als Beispiel an!

---

#### 📍 Aufgabe 3.2: GetLoansByBookQuery erstellen

Erstelle in `Queries/GetLoansByBook/`:

##### GetLoansByBookQuery.cs
```csharp
public readonly record struct GetLoansByBookQuery(int BookId) 
    : IRequest<Result<IEnumerable<GetLoanDto>>>;
```

##### GetLoansByBookQueryHandler.cs
```csharp
public sealed class GetLoansByBookQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetLoansByBookQuery, Result<IEnumerable<GetLoanDto>>>
{
    public async Task<Result<IEnumerable<GetLoanDto>>> Handle(...)
    {
        // 1. Repository-Methode aufrufen
        // 2. Zu DTOs mappen (mit Mapster: entity.Adapt<GetLoanDto>())
        // 3. BookTitle und IsOverdue setzen
        // 4. Result.Success zurückgeben
    }
}
```

---

### ✏️ Teil 4: Controller Endpoints (API Layer)

#### 📍 Aufgabe 4.1: LoansController implementieren

**Datei:** `Api/Controllers/LoansController.cs`

Der Controller ist bereits angelegt, aber leer. Implementiere:

##### POST /api/loans - CreateLoan
```csharp
[HttpPost]
[ProducesResponseType(typeof(GetLoanDto), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> Create([FromBody] CreateLoanCommand command, CancellationToken ct)
{
    var result = await mediator.Send(command, ct);
    return result.ToActionResult(this);
}
```

##### GET /api/loans/book/{bookId}
```csharp
[HttpGet("book/{bookId:int}")]
[ProducesResponseType(typeof(IEnumerable<GetLoanDto>), StatusCodes.Status200OK)]
public async Task<IActionResult> GetByBook(int bookId, CancellationToken ct)
{
    var result = await mediator.Send(new GetLoansByBookQuery(bookId), ct);
    return result.ToActionResult(this);
}
```

**💡 Tipp:** Schaue dir `BooksController` als vollständiges Beispiel an!

---

### ✏️ Teil 5: Dependency Injection (Application Layer)

#### 📍 Aufgabe 5.1: Service registrieren

**Datei:** `Application/DependencyInjection.cs`

Falls du einen Uniqueness Checker für Loans brauchst, registriere ihn hier ähnlich wie `BookUniquenessChecker`.

**💡 Hinweis:** Für diese Übung ist kein Uniqueness Checker für Loans notwendig.

---

## 🧪 Tests ausführen

### Unit Tests
```bash
cd CleanArchitecture_Uebung_02
dotnet test Domain.Tests/Domain.Tests.csproj
```

**Erwartung:**
- ✅ `BookTests` - sollten grün sein (Beispiel)
- ❌ `LoanSpecificationsTests` - werden erst grün, wenn du LoanSpecifications implementiert hast

### Integration Tests
```bash
dotnet test Api.Tests/Api.Tests.csproj
```

---

## 🚀 Projekt ausführen

### 1. Datenbank erstellen
```bash
cd CleanArchitecture_Uebung_02/Infrastructure
dotnet ef migrations add Initial --startup-project ../Api
dotnet ef database update --startup-project ../Api
```

### 2. API starten
```bash
cd CleanArchitecture_Uebung_02/Api
dotnet run
```

Die API läuft auf: `https://localhost:5101/swagger`

---

## 📝 Validierungsregeln - Übersicht

### Book (vollständig implementiert)
| Property | Domain Validation | FluentValidation |
|----------|------------------|------------------|
| ISBN | NotEmpty, Length(13), OnlyDigits | NotEmpty, Length(13) |
| Title | NotEmpty, MinLength(1) | NotEmpty, MinLength(1) |
| AuthorId | > 0 | GreaterThan(0) |
| PublicationYear | >= 1450, <= Now+1 | GreaterThanOrEqualTo(1450) |
| AvailableCopies | >= 0 | GreaterThanOrEqualTo(0) |
| ISBN (Uniqueness) | Unique | - |

### Loan (DU musst implementieren!)
| Property | Domain Validation | FluentValidation |
|----------|------------------|------------------|
| BookId | > 0 | GreaterThan(0) |
| BorrowerName | NotEmpty, MinLength(2) | NotEmpty, MinimumLength(2) |
| LoanDate | Not in future | - |
| DueDate | Auto-calculated (LoanDate + 14 days) | - |

---

## 🎓 Design Patterns & Konzepte

Diese Übung deckt ab:
1. **Clean Architecture** - 4 Layer Trennung
2. **CQRS** - Commands (CreateLoan) & Queries (GetLoans)
3. **Repository Pattern** - ILoanRepository mit spezifischen Methoden
4. **Domain-Driven Design** - Loan.Create Factory Method
5. **Validation (3 Ebenen)**:
   - Domain: LoanSpecifications
   - Application: FluentValidation
   - Database: Foreign Keys, Indexes
6. **Dependency Injection** - Constructor Injection
7. **Unit of Work** - Transaktionale Speicherung

---

## 📚 Wichtige Dateien

### Vollständig implementiert (als Referenz):
- ✅ `Domain/Entities/Book.cs`
- ✅ `Domain/Specifications/BookSpecifications.cs`
- ✅ `Application/Features/Books/` - alle Commands & Queries
- ✅ `Infrastructure/Persistence/Repositories/BookRepository.cs`
- ✅ `Api/Controllers/BooksController.cs`

### Mit Lücken (deine Aufgabe):
- ❌ `Domain/Specifications/LoanSpecifications.cs`
- ❌ `Domain/Entities/Loan.cs` (Create-Methode)
- ❌ `Infrastructure/Persistence/Repositories/LoanRepository.cs`
- ❌ `Application/Features/Loans/Commands/CreateLoan/` (alle Dateien)
- ❌ `Application/Features/Loans/Queries/GetLoansByBook/` (alle Dateien)
- ❌ `Api/Controllers/LoansController.cs`

---

## ✅ Checkliste

### Domain Layer
- [ ] LoanSpecifications.CheckBookId implementiert
- [ ] LoanSpecifications.CheckBorrowerName implementiert
- [ ] LoanSpecifications.CheckLoanDate implementiert
- [ ] LoanSpecifications.ValidateLoanInternal implementiert
- [ ] Loan.Create implementiert
- [ ] Domain.Tests: LoanSpecificationsTests grün

### Infrastructure Layer
- [ ] LoanRepository.GetLoansByBookIdAsync implementiert
- [ ] LoanRepository.GetActiveLoansByBorrowerAsync implementiert
- [ ] LoanRepository.GetOverdueLoansAsync implementiert

### Application Layer
- [ ] CreateLoanCommand erstellt
- [ ] CreateLoanCommandHandler erstellt
- [ ] CreateLoanCommandValidator erstellt
- [ ] GetLoansByBookQuery erstellt
- [ ] GetLoansByBookQueryHandler erstellt

### API Layer
- [ ] LoansController.Create implementiert
- [ ] LoansController.GetByBook implementiert

### Tests & Ausführung
- [ ] Alle Unit Tests grün
- [ ] Projekt kompiliert ohne Fehler
- [ ] Datenbank Migration erfolgreich
- [ ] API läuft und Swagger ist erreichbar
- [ ] Loan-Endpoints in Swagger getestet

---

## 🎯 Tipps

1. **Starte mit Domain:** Implementiere zuerst die Validierungen
2. **Tests nutzen:** Die Tests zeigen dir, was erwartet wird
3. **Referenz nutzen:** Book ist vollständig implementiert - nutze es als Vorlage!
4. **Schritt für Schritt:** Arbeite die Checkliste der Reihe nach ab
5. **Template verwenden:** Du darfst das `CleanArchitecture_Template` zur Hilfe nehmen!

---

## 🤔 Häufige Fragen

### Wie verwende ich Mapster?
```csharp
var dto = entity.Adapt<GetLoanDto>();
// Oder mit Anpassungen:
var dto = entity.Adapt<GetLoanDto>() with { BookTitle = entity.Book.Title };
```

### Wie prüfe ich auf null?
```csharp
if (book == null)
    throw new NotFoundException($"Book with ID {bookId} not found.");
```

### Wie verwende ich das UnitOfWork?
```csharp
var book = await uow.Books.GetByIdAsync(bookId, ct);
var loan = Loan.Create(book, borrowerName, DateTime.Now);
await uow.Loans.AddAsync(loan, ct);
book.DecreaseCopies();
uow.Books.Update(book);
await uow.SaveChangesAsync(ct);
```

---

**Viel Erfolg! 🚀**

---

**Erstellt für WMC Test-Vorbereitung 2025** 🎓

