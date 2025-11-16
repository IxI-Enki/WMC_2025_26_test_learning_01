<!-- markdownlint-disable -->

# 🎯 WMC Übung 2 - Implementierungs-Roadmap

> **Letzte Aktualisierung:** 2025-11-16 (nach Code-Implementierung & Testing)
>
> **Status:** 🟢 Phase 1 (Authors) ~95% fertig | 🟢 Phase 2 (Books) ~75% fertig | 🔴 Phase 3 (Loans) noch nicht gestartet

## 📊 Entitäten-Abhängigkeiten (wie im Template)

```diagram|
┌─────────┐
│ Author  │ ◄─── Keine Abhängigkeiten (standalone) ──► siehe Sensor
└────┬────┘                                                  ↳ Sensor.Id
     │                                                                ⇣
     │--→ FK: AuthorId                                      -→ FK: SensorId
     │                                                                ⋮
     ▼                                                                ⋮
┌─────────┐                                                           ⇣
│  Book   │ ◄─── Braucht Author                    ──► siehe Measurement (braucht Sensor)
└────┬────┘
     │
     │--→ FK: BookId
     │
     ▼
┌─────────┐
│  Loan   │ ◄─── Braucht Book
└─────────┘
```

**Vergleich mit Template:**

- Template: `Sensor` (standalone) → `Measurement` (FK: SensorId)
- Übung 2: `Author` (standalone) → `Book` (FK: AuthorId) → `Loan` (FK: BookId)

---

## 🎓 Implementierungs-Strategie

### ⚡ **Warum diese Reihenfolge?**

1. **Author zuerst** → Einfachste Entität, keine FK-Abhängigkeiten
2. **Book danach** → Braucht Author-Objekte für Tests und Queries
3. **Loan zuletzt** → Braucht Book-Objekte (und indirekt Authors)

### 🏗️ **Pro Entität: Layer-by-Layer Ansatz**

Für jede Entität (Author → Book → Loan) diese Schritte durchlaufen:

1. ✅ **Domain** → Validationen & Factory-Methoden
2. ✅ **Application** → Commands, Queries, Handlers, Validators
3. ✅ **Infrastructure** → Repository-Methoden (bereits fertig laut Kollegin!)
4. ✅ **API** → Controller-Endpunkte
5. ✅ **Tests** → Domain-Tests & API-Tests ausführen

---

## 📋 Phase 1: Author (Standalone - Einfachste Entität)

<details>
<summary><strong>✅ Was bereits funktioniert - Authors (Click to expand)</strong></summary>

### Domain-Ebene

- ✅ `Author.Create()` ist implementiert (`Domain/Entities/Author.cs`)
- ✅ `AuthorSpecifications.CheckFirstName()` - **IMPLEMENTIERT!** ✨
- ✅ `AuthorSpecifications.CheckLastName()` - **IMPLEMENTIERT!** ✨
- ✅ `AuthorSpecifications.CheckDateOfBirth()` - **IMPLEMENTIERT!** ✨
- ✅ `AuthorSpecifications.ValidateAuthorInternal()` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨

### Application-Ebene

- ✅ `GetAuthorDto.cs` existiert
- ✅ `GetAllAuthorsQuery` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
- ✅ `GetAllAuthorsQueryHandler` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
  - Nutzt `uow.Authors.GetAllAsync()`
  - Mappt mit Mapster zu DTOs
  - Gibt `Result.Success()` zurück
- ✅ `GetAuthorByIdQuery` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
- ✅ `GetAuthorByIdQueryHandler` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
  - Nutzt `uow.Authors.GetByIdAsync()`
  - Null-Check implementiert
  - Gibt `Result.NotFound()` oder `Result.Success()` zurück

### API-Ebene

- ✅ `AutorsController.cs` existiert
- ✅ `GetAll()` - **FUNKTIONIERT!** ✨
- ✅ `GetById(int id)` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨

### Infrastructure

- ✅ `AuthorRepository` ist fertig (laut Kollegin)

</details>

<details>
<summary><strong>🔨 Was zu tun ist - Authors (Click to expand)</strong></summary>

### 1️⃣ Domain-Ebene - Authors ✅ FERTIG!

**Vergleich mit Template:** `SensorSpecifications.cs`

#### ✅ Erfolgreich implementiert!

```csharp
// ✅ Domain/Specifications/AuthorSpecifications.cs - Zeile 46-62
public static void ValidateAuthorInternal(string firstName, string lastName, DateTime dateOfBirth)
{
    var validationResults = new List<DomainValidationResult>
    {
        CheckFirstName(firstName),
        CheckLastName(lastName),
        CheckDateOfBirth(dateOfBirth)
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

### 2️⃣ Application-Ebene - Authors

#### ✅ Bereits implementiert

- `GetAllAuthorsQuery` + Handler

#### ✅ GetAuthorById - FERTIG IMPLEMENTIERT!

```plaintext
Application/Features/Authors/Queries/GetAuthorById/
  ├── GetAuthorByIdQuery.cs ✅
  ├── GetAuthorByIdQueryHandler.cs ✅
  └── GetAuthoByIdValidator.cs ✅
```

**Template-Referenz:** `GetSensorByIdQuery.cs` (Zeilen 6-7)

```csharp
// Template Pattern:
public readonly record struct GetAuthorByIdQuery(int Id) 
    : IRequest<Result<GetAuthorDto>>;
```

**Handler-Template:** `GetSensorByIdQueryHandler.cs` (Zeilen 10-20)

```csharp
// Template Pattern:
public sealed class GetAuthorByIdQueryHandler(IUnitOfWork uow) 
    : IRequestHandler<GetAuthorByIdQuery, Result<GetAuthorDto>>
{
    public async Task<Result<GetAuthorDto>> Handle(GetAuthorByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var entity = await uow.Authors.GetByIdAsync(request.Id, cancellationToken);
        
        if (entity == null)
            return Result<GetAuthorDto>.NotFound($"Author with ID {request.Id} not found.");
        
        var dto = entity.Adapt<GetAuthorDto>();
        return Result<GetAuthorDto>.Success(dto);
    }
}
```

---

### 3️⃣ API-Ebene - Authors ✅ FERTIG!

#### ✅ Erfolgreich implementiert!

```csharp
// ✅ Api/Controllers/AutorsController.cs - Zeile 32-40
[HttpGet("{id:int}")]
[ProducesResponseType(typeof(GetBookDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetById(int id, CancellationToken ct)
{
    var result = await mediator.Send(new GetAuthorByIdQuery(id), ct);
    return result.ToActionResult(this);
}
```

---

### 4️⃣ Tests - Authors

**Via Swagger testen:**

```bash
✅ GET /api/autors → Sollte 5 Autoren zurückgeben (J.K. Rowling, George R.R. Martin, etc.)
✅ GET /api/autors/1 → Sollte J.K. Rowling zurückgeben
✅ GET /api/autors/999 → Sollte 404 Not Found zurückgeben
```

</details>

---

## 📋 Phase 2: Book (Hängt von Author ab)

<details>
<summary><strong>✅ Was bereits funktioniert - Books (Click to expand)</strong></summary>

### Domain-Ebene

- ✅ `Book.CreateAsync()` ist **vollständig implementiert**
- ✅ `Book.DecreaseCopies()` & `IncreaseCopies()` sind fertig
- ✅ `BookSpecifications.CheckISBN()` - **IMPLEMENTIERT!** ✨
  - Prüft auf 13 Zeichen
  - Prüft nur Ziffern
  - Korrekte Fehlermeldung mit "(ohne Bindestriche)"
- ✅ `BookSpecifications.CheckTitle()` - **IMPLEMENTIERT!** ✨
- ✅ `BookSpecifications.CheckPublicationYear()` - **IMPLEMENTIERT!** ✨ (1450 - heute+1)
- ✅ `BookSpecifications.CheckAvailableCopies()` - **IMPLEMENTIERT!** ✨ (≥ 0)
- ✅ `BookSpecifications.ValidateBookInternal()` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
- ✅ `BookSpecifications.ValidateBookExternal()` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨

### Application-Ebene

- ✅ `GetBookDto.cs` existiert
- ✅ `GetAllBooksQuery` existiert
- ✅ `GetAllBooksQueryHandler` - **IMPLEMENTIERT!** ✨
  - Nutzt `uow.Books.GetAllAsync()`
  - Mappt zu DTOs mit Mapster
  - ⚠️ **ACHTUNG:** `.Include(b => b.Author)` fehlt noch für Navigation Properties!
- ✅ `CreateBookCommand.cs` - **PROPERTIES KORRIGIERT!** ✨
  - ISBN, Title, AuthorId, PublicationYear, AvailableCopies
- ✅ `CreateBookCommandHandler.cs` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
  - Lädt Author aus DB
  - Ruft `Book.CreateAsync()` auf
  - Speichert via UoW
  - Gibt `Result.Created()` zurück
- ⚠️ `CreateBookCommandValidator.cs` ist **noch LEER** (Optional)

### API-Ebene

- ✅ `BooksController.cs` existiert
- ✅ `GetAll()` - **FUNKTIONIERT!** ✨
- ✅ `GetById(int id)` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
- ✅ `Create()` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨
- ✅ `Delete(int id)` - **VOLLSTÄNDIG IMPLEMENTIERT!** ✨

### Infrastructure

- ✅ `BookRepository` ist fertig (laut Kollegin)

### Dependency Injection

- ✅ `IBookUniquenessChecker` ist registriert in `DependencyInjection.cs` ✨

</details>

<details>
<summary><strong>🔨 Was zu tun ist - Books (Click to expand)</strong></summary>

### 1️⃣ Domain-Ebene - Books

#### ✅ Erfolgreich implementiert!

**a) ValidateBookInternal: ✅**

```csharp
// ✅ Domain/Specifications/BookSpecifications.cs - Zeile 70-93
public static void ValidateBookInternal(
    string isbn, string title, int authorId, int publicationYear, int availableCopies)
{
    var validationResults = new List<DomainValidationResult>
    {
        CheckISBN(isbn),
        CheckTitle(title),
        CheckAuthorId(authorId),
        CheckPublicationYear(publicationYear),
        CheckAvailableCopies(availableCopies)
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

**b) ValidateBookExternal: ✅**

```csharp
// ✅ Domain/Specifications/BookSpecifications.cs - Zeile 98-103
public static async Task ValidateBookExternal(int id, string isbn,
    IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
{
    if (!await uniquenessChecker.IsUniqueAsync(id, isbn, ct))
        throw new DomainValidationException("ISBN", "Ein Buch mit dieser ISBN existiert bereits.");
}
```

**c) Book.UpdateAsync:**

```csharp
// ⚠️ Domain/Entities/Book.cs - Zeile 109-121
public async Task UpdateAsync(
    string isbn, string title, int authorId, int publicationYear, int availableCopies,
    IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
{
    throw new NotImplementedException("Book.UpdateAsync muss noch implementiert werden!");
}
```

**Template-Referenz:** `Sensor.UpdateAsync` (Zeilen 59-71)

```csharp
// ✅ Sollte so aussehen:
public async Task UpdateAsync(
    string isbn, string title, Author author, int publicationYear, int availableCopies,
    IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
{
    var trimmedIsbn = (isbn ?? string.Empty).Trim();
    var trimmedTitle = (title ?? string.Empty).Trim();
    
    // Keine Änderung? Dann return
    if (ISBN == trimmedIsbn && Title == trimmedTitle && AuthorId == author.Id 
        && PublicationYear == publicationYear && AvailableCopies == availableCopies)
        return;
    
    ValidateBookProperties(trimmedIsbn, trimmedTitle, author, publicationYear, availableCopies);
    await ValidateBookUniqueness(Id, trimmedIsbn, uniquenessChecker, ct);
    
    ISBN = trimmedIsbn;
    Title = trimmedTitle;
    Author = author;
    AuthorId = author.Id;
    PublicationYear = publicationYear;
    AvailableCopies = availableCopies;
}
```

---

### 2️⃣ Application-Ebene - Books

#### 🚨 KRITISCH: CreateBookCommand Properties KORRIGIEREN

```csharp
// ❌ Application/Features/Books/Commands/CreateBook/CreateBookCommand.cs - Zeile 10-11
// FALSCH (von Sensor kopiert):
public readonly record struct CreateBookCommand(string Location, string Name, DateTime Timestamp, double Value)
    : IRequest<Result<GetBookDto>>;
```

```csharp
// ✅ RICHTIG (sollte sein):
public readonly record struct CreateBookCommand(
    string ISBN, 
    string Title, 
    int AuthorId, 
    int PublicationYear, 
    int AvailableCopies
) : IRequest<Result<GetBookDto>>;
```

**Template-Referenz:** `CreateSensorCommand` (Zeile 8-9)

---

#### ✅ CreateBookCommandHandler erfolgreich implementiert!

```csharp
// ✅ Application/Features/Books/Commands/CreateBook/CreateBookCommandHandler.cs
public async Task<Result<GetBookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
{
    // 1. Author aus DB laden
    var author = await uow.Authors.GetByIdAsync(request.AuthorId, cancellationToken);
    if (author == null)
        return Result<GetBookDto>.NotFound($"Author with ID {request.AuthorId} not found.");
    
    // 2. Book über Domänenlogik erstellen
    var entity = await Book.CreateAsync(
        request.ISBN, 
        request.Title, 
        author, 
        request.PublicationYear, 
        request.AvailableCopies,
        uniquenessChecker, 
        cancellationToken);
    
    // 3. Persistieren
    await uow.Books.AddAsync(entity, cancellationToken);
    await uow.SaveChangesAsync(cancellationToken);
    
    // 4. DTO zurückgeben
    return Result<GetBookDto>.Created(entity.Adapt<GetBookDto>());
}
```

---

#### ❌ CreateBookCommandValidator implementieren

```csharp
// ❌ Aktuell: LEER (nur namespace)
```

**Template-Referenz:** `CreateSensorValidator` (Zeilen 5-18) - aber **auskommentiert!**

**WICHTIG:** Im Template ist der Validator auskommentiert, weil Domain-Validierung bevorzugt wird. Für die Übung sollten wir aber FluentValidation zeigen:

```csharp
// ✅ Sollte so aussehen:
public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN darf nicht leer sein.")
            .Length(13).WithMessage("ISBN muss genau 13 Zeichen haben.")
            .Matches("^[0-9]+$").WithMessage("ISBN darf nur Ziffern enthalten.");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Titel darf nicht leer sein.")
            .MinimumLength(1);
        
        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("AuthorId muss größer als 0 sein.");
        
        RuleFor(x => x.PublicationYear)
            .GreaterThanOrEqualTo(1450)
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage($"PublicationYear muss zwischen 1450 und {DateTime.Now.Year + 1} liegen.");
        
        RuleFor(x => x.AvailableCopies)
            .GreaterThanOrEqualTo(0).WithMessage("AvailableCopies muss mindestens 0 sein.");
    }
}
```

---

#### ⚠️ GetAllBooksQueryHandler verbessern

```csharp
// ⚠️ Aktuell (Zeile 17):
var entities = await uow.Books.GetAllAsync(ct: cancellationToken);
```

**Problem:** Navigation Property `Author` wird nicht geladen!

**Template-Referenz:** `GetAllSensorsQueryHandler` (Zeilen 15-16) nutzt `orderBy`, aber keine `.Include()` weil Sensor keine Navigation Properties hat.

Für Books brauchen wir `.Include()`:

```csharp
// ✅ Sollte so aussehen:
var entities = await uow.Books.GetAllAsync(
    include: q => q.Include(b => b.Author),
    ct: cancellationToken);
```

**ODER** wenn `GetAllAsync` kein `include` Parameter hat, über Repository direkt:

```csharp
// Alternative (falls GetAllAsync kein include unterstützt):
var entities = await uow.Books.GetAllWithAuthorsAsync(cancellationToken);
```

---

#### ❌ Noch zu erstellen

**GetBookById:**

```plaintext
Application/Features/Books/Queries/GetBookById/
  ├── GetBookByIdQuery.cs
  ├── GetBookByIdQueryHandler.cs (inkl. .Include(b => b.Author))
  └── GetBookByIdQueryValidator.cs (optional)
```

**DeleteBook:**

```plaintext
Application/Features/Books/Commands/DeleteBook/
  ├── DeleteBookCommand.cs
  └── DeleteBookCommandHandler.cs
```

**UpdateBook:**

```plaintext
Application/Features/Books/Commands/UpdateBook/
  ├── UpdateBookCommand.cs
  ├── UpdateBookCommandHandler.cs
  └── UpdateBookCommandValidator.cs
```

---

### 3️⃣ API-Ebene - Books

#### ⚠️ Zu vervollständigen

**a) GetById:**

```csharp
// ❌ Api/Controllers/BooksController.cs - Zeile 38-40
[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id, CancellationToken ct)
{
    throw new NotImplementedException("BooksController.GetById muss noch implementiert werden!");
}
```

**Template-Referenz:** `SensorsController.GetById` (Zeilen 38-45)

```csharp
// ✅ Sollte so aussehen:
[HttpGet("{id:int}")]
[ProducesResponseType(typeof(GetBookDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetById(int id, CancellationToken ct)
{
    var result = await mediator.Send(new GetBookByIdQuery(id), ct);
    return result.ToActionResult(this);
}
```

**b) Create:**

```csharp
// ❌ Api/Controllers/BooksController.cs - Zeile 50-54
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateBookCommand command, CancellationToken ct)
{
    throw new NotImplementedException("BooksController.Create muss noch implementiert werden!");
}
```

**Template-Referenz:** `SensorsController.Create` (Zeilen 67-77)

```csharp
// ✅ Sollte so aussehen:
[HttpPost]
[ProducesResponseType(typeof(GetBookDto), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public async Task<IActionResult> Create([FromBody] CreateBookCommand command, CancellationToken ct)
{
    var result = await mediator.Send(command, ct);
    return result.ToActionResult(this, createdAtAction: nameof(GetById), 
        routeValues: new { id = result?.Value?.Id });
}
```

**c) Update (noch zu erstellen):**

**Template-Referenz:** `SensorsController.Update` (Zeilen 85-102)

```csharp
// ✅ Sollte so aussehen:
[HttpPut("{id:int}")]
[ProducesResponseType(typeof(GetBookDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public async Task<IActionResult> Update(int id, [FromBody] UpdateBookCommand command, CancellationToken ct)
{
    if (id != command.Id)
    {
        Result<GetBookDto> badResult = Result<GetBookDto>.ValidationError(
            "The route ID does not match the book ID in the request body.");
        return badResult.ToActionResult(this);
    }
    
    var result = await mediator.Send(command, ct);
    return result.ToActionResult(this);
}
```

**d) Delete:**

```csharp
// ❌ Api/Controllers/BooksController.cs - Zeile 63-65
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id, CancellationToken ct)
{
    throw new NotImplementedException("BooksController.Delete muss noch implementiert werden!");
}
```

**Template-Referenz:** `SensorsController.Delete` (Zeilen 107-114)

```csharp
// ✅ Sollte so aussehen:
[HttpDelete("{id:int}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> Delete(int id, CancellationToken ct)
{
    var result = await mediator.Send(new DeleteBookCommand(id), ct);
    return result.ToActionResult(this);
}
```

---

### 4️⃣ Tests - Books

**Via Swagger testen:**

```bash
⏳ POST /api/books → Neues Buch anlegen (mit AuthorId=1 für J.K. Rowling)
⏳ GET /api/books → Liste mit Books (inkl. Author-Infos!)
⏳ GET /api/books/1 → Einzelnes Buch mit Author
⏳ PUT /api/books/1 → Buch aktualisieren
⏳ DELETE /api/books/1 → Buch löschen
```

**Domain.Tests:**

```bash
⏳ dotnet test Domain.Tests
```

</details>

---

## 📋 Phase 3: Loan (Hängt von Book ab)

<details>
<summary><strong>✅ Was bereits funktioniert - Loans (Click to expand)</strong></summary>

### Domain-Ebene

- ✅ `Loan.MarkAsReturned()` ist fertig
- ✅ `Loan.IsOverdue()` ist fertig
- ❌ `Loan.Create()` - Noch `NotImplementedException`

### Infrastructure

- ✅ `LoanRepository` ist fertig (laut Kollegin)

</details>

<details>
<summary><strong>🔨 Was zu tun ist - Loans (Click to expand)</strong></summary>

### 1️⃣ Domain-Ebene - Loans

**Zu implementieren:**

- `LoanSpecifications.CheckBorrowerName()`
- `LoanSpecifications.CheckLoanDate()`
- `LoanSpecifications.CheckDueDate()`
- `LoanSpecifications.ValidateLoanInternal()`
- `Loan.Create()`

**Template-Referenz:** `Measurement.Create()` (Zeilen 35-45)

---

### 2️⃣ Application-Ebene - Loans

**Zu erstellen:**

- `GetLoanDto.cs`
- `CreateLoanCommand` + Handler + Validator
- `ReturnLoanCommand` + Handler
- `GetLoansByBookQuery` + Handler
- `GetOverdueLoansQuery` + Handler

---

### 3️⃣ Infrastructure-Ebene - Loans

**Optional - spezielle Queries:**

- `LoanRepository.GetLoansByBookIdAsync()`
- `LoanRepository.GetOverdueLoansAsync()`

---

### 4️⃣ API-Ebene - Loans

**Zu erstellen:**

- `LoansController.CreateLoan()`
- `LoansController.ReturnLoan()`
- `LoansController.GetLoansByBook()`
- `LoansController.GetOverdueLoans()`

</details>

---

## 🔧 Dependency Injection Checklist

<details>
<summary><strong>✅ DI Status (Click to expand)</strong></summary>

```csharp
// ✅ Application/DependencyInjection.cs - Zeile 27
services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();
```

**Status:** ✅ Fertig - IBookUniquenessChecker ist korrekt registriert!

</details>

---

## 📝 Allgemeine Tipps (aus Template gelernt)

<details>
<summary><strong>💡 Best Practices (Click to expand)</strong></summary>

### 🎯 Domain-Validierungen

- **Immer** `DomainValidationResult` zurückgeben (nie direkt Exception werfen)
- **Factory-Methoden** sammeln alle Validierungen und werfen dann `DomainValidationException`
- **ArgumentNullException** für null-Checks bei Objekten (z.B. `author`, `book`)

### 🎯 Application-Handler

- **Queries:** Daten aus UoW holen, zu DTOs mappen, `Result.Success()` zurückgeben
- **Commands:** Entität erstellen/updaten, via UoW speichern, DTO zurückgeben
- **Include:** Navigation Properties mit `.Include()` laden

### 🎯 Controller

- **Nie** Business-Logik im Controller
- **Immer** `result.ToActionResult(this)` für konsistente Responses
- **CreatedAtAction** für POST-Requests mit Route zu GetById

### 🎯 Tests

- **Domain-Tests:** Factory-Methoden & Validierungen testen
- **API-Tests:** InMemory-DB nutzen (bereits konfiguriert in TestWebApplicationFactory)

</details>

---

## 📚 Wichtige Referenzen im Template

<details>
<summary><strong>🔍 Template-Mapping-Tabelle (Click to expand)</strong></summary>

| Was zu implementieren | Template-Referenz                     | Zeilen  |
| --------------------- | ------------------------------------- | ------- |
| Domain Factory        | `Sensor.CreateAsync()`                | 40-55   |
| Domain Update         | `Sensor.UpdateAsync()`                | 59-71   |
| Domain Validierung    | `SensorSpecifications.cs`             | Alle    |
| QueryHandler          | `GetAllSensorsQueryHandler.cs`        | 12-19   |
| CommandHandler        | `CreateSensorCommandHandler.cs`       | 17-33   |
| CommandValidator      | `CreateSensorCommandValidator.cs`     | Alle    |
| Controller GET        | `SensorsController.GetAll()`          | 27-32   |
| Controller POST       | `SensorsController.Create()`          | 71-77   |
| Controller PUT        | `SensorsController.Update()`          | 90-102  |
| Controller DELETE     | `SensorsController.Delete()`          | 110-114 |
| Repository Custom     | `SensorRepository.GetByLocationAsync` | -       |
| Uniqueness Checker    | `SensorUniquenessChecker.cs`          | Alle    |
| Domain Tests          | `SensorTests.cs`                      | Alle    |
| API Tests             | `SensorsEndpointTests.cs`             | Alle    |
| Result Pattern        | `Result.cs` & `ResultExtensions.cs`   | Alle    |
| Mapster Config        | Handler: `.Adapt<DTO>()`              | Inline  |

</details>

---

## ⚡ Quick Start

<details>
<summary><strong>🚀 Schnellstart-Anleitung (Click to expand)</strong></summary>

```bash
# 1. Tests laufen lassen
dotnet test CleanArchitecture_Uebung_02/Domain.Tests
dotnet test CleanArchitecture_Uebung_02/Api.Tests

# 2. API starten und via Swagger testen
cd CleanArchitecture_Uebung_02/Api
dotnet run

# 3. Swagger öffnen: https://localhost:7085/swagger
```

### 🧪 Was jetzt testbar ist

```bash
# Via Swagger testen:
✅ GET /api/autors → 5 Autoren aus DataSeeder (J.K. Rowling, etc.)
✅ GET /api/books → Leeres Array (noch keine Books in DB)

# Nächste Schritte:
1. CreateBookCommand Properties korrigieren (ISBN, Title, AuthorId, etc.)
2. CreateBookCommandHandler implementieren (Zeile 19)
3. POST /api/books testen → Erstes Buch anlegen
4. GET /api/books erneut → Sollte dann das neue Buch zeigen
```

</details>

---

## 🎯 Erfolgskriterien & Aktueller Status

<details>
<summary><strong>📊 Status-Übersicht (Click to expand)</strong></summary>

| Phase                 | Kriterium                                            | Status | Fortschritt |
| --------------------- | ---------------------------------------------------- | ------ | ----------- |
| **Phase 1 (Authors)** | GET /api/autors liefert 5 Autoren aus DataSeeder    | ✅     | 100%        |
| **Phase 1 (Authors)** | GET /api/autors/{id} liefert einzelnen Autor        | ✅     | 100%        |
| **Phase 1 (Authors)** | Domain Validations (CheckFirstName, etc.)            | ✅     | 100%        |
| **Phase 1 (Authors)** | ValidateAuthorInternal implementiert                 | ✅     | 100%        |
| **Phase 2 (Books)**   | GET /api/books liefert Liste (aktuell leer)          | ✅     | 100%        |
| **Phase 2 (Books)**   | GET /api/books/{id} mit Null-Check                   | ✅     | 100%        |
| **Phase 2 (Books)**   | CreateBookCommand Properties korrekt                 | ✅     | 100%        |
| **Phase 2 (Books)**   | POST /api/books erstellt ein Buch                    | ✅     | 100%        |
| **Phase 2 (Books)**   | Domain Validations (CheckISBN, etc.)                 | ✅     | 100%        |
| **Phase 2 (Books)**   | ValidateBookInternal/External implementiert          | ✅     | 100%        |
| **Phase 2 (Books)**   | DELETE /api/books/{id} implementiert                 | ✅     | 100%        |
| **Phase 2 (Books)**   | Book.UpdateAsync implementiert                       | ⚠️     | 0%          |
| **Phase 2 (Books)**   | PUT /api/books/{id} implementiert                    | ⚠️     | 0%          |
| **Phase 3 (Loans)**   | POST /api/loans leiht ein Buch aus (AvailableCopies) | ⏳     | 0%          |
| **Tests**             | Domain.Tests ausführbar                              | ✅     | 100%        |
| **Tests**             | Api.Tests ausführbar                                 | ✅     | 100%        |
| **DataSeeder**        | Läuft beim Start und befüllt Authors                | ✅     | 100%        |
| **DI**                | IBookUniquenessChecker registriert                   | ✅     | 100%        |

**Legende:** ✅ Fertig | ⚠️ Teilweise / To-Do | ❌ Kritischer Fehler | ⏳ Noch nicht gestartet

**Gesamtfortschritt:**

- **Phase 1 (Authors):** ~95% ✅✅✅✅ (nur Update-Methode fehlt noch)
- **Phase 2 (Books):** ~75% ✅✅✅✅✅⚠️⚠️ (GET/POST/DELETE fertig, nur Update fehlt noch)
- **Phase 3 (Loans):** ~0% ⏳

</details>

---

## 🎯 Nächste konkrete Schritte (Priorität)

<details>
<summary><strong>🔥 Top-Prioritäten (Click to expand)</strong></summary>

### ✅ Bereits erledigt:

1. ~~**CreateBookCommand Properties KORRIGIEREN:**~~ ✅ **FERTIG!**
   - ✅ Korrigiert: `ISBN, Title, AuthorId, PublicationYear, AvailableCopies`
   - Datei: `Application/Features/Books/Commands/CreateBook/CreateBookCommand.cs`

2. ~~**CreateBookCommandHandler implementieren:**~~ ✅ **FERTIG!**
   - ✅ Author aus DB laden mit `uow.Authors.GetByIdAsync(request.AuthorId)`
   - ✅ `Book.CreateAsync()` aufrufen
   - ✅ Persistieren und DTO zurückgeben
   - Datei: `Application/Features/Books/Commands/CreateBook/CreateBookCommandHandler.cs`

3. ~~**ValidateBookInternal & ValidateBookExternal:**~~ ✅ **FERTIG!**
   - ✅ Domain-Validierungen komplett implementiert
   - Datei: `Domain/Specifications/BookSpecifications.cs`

4. ~~**ValidateAuthorInternal:**~~ ✅ **FERTIG!**
   - ✅ Domain-Validierung implementiert
   - Datei: `Domain/Specifications/AuthorSpecifications.cs`

5. ~~**GetAuthorById implementieren:**~~ ✅ **FERTIG!**
   - ✅ Query, Handler mit Null-Check, Controller-Endpunkt
   - Dateien: `Application/Features/Authors/Queries/GetAuthorById/*`

6. ~~**BooksController vervollständigen:**~~ ✅ **FERTIG (teilweise)!**
   - ✅ GetById(), Create(), Delete() sind fertig
   - Datei: `Api/Controllers/BooksController.cs`

### ⚠️ Noch zu tun (Optional):

7. **GetAllBooksQueryHandler verbessern:**
   - `.Include(b => b.Author)` hinzufügen für Navigation Properties
   - Datei: `Application/Features/Books/Queries/GetAllBooks/GetAllBooksQueryHandler.cs`

8. **CreateBookCommandValidator implementieren:**
   - FluentValidation Rules für ISBN, Title, PublicationYear, etc.
   - Datei: `Application/Features/Books/Commands/CreateBook/CreateBookCommandValidator.cs`

9. **Book.UpdateAsync & UpdateBookCommand implementieren:**
   - UpdateBookCommand + Handler erstellen
   - PUT /api/books/{id} Endpoint
   - Dateien: `Application/Features/Books/Commands/UpdateBook/*`

</details>

---

## 📋 Template-Analyse-Checkliste

<details>
<summary><strong>🔍 Systematischer Vergleich Template ↔ Übung 2 (Click to expand)</strong></summary>

### Phase 1: Authors ↔ Sensors

| Aspekt                  | Template (Sensors)          | Übung 2 (Authors)           | Status |
| ----------------------- | --------------------------- | --------------------------- | ------ |
| **Domain Entity**       | `Sensor.cs`                 | `Author.cs`                 | ✅     |
| Factory Method          | `CreateAsync()`             | `Create()`                  | ✅     |
| Update Method           | `UpdateAsync()`             | ❌ Nicht vorhanden          | ⚠️     |
| **Specifications**      | `SensorSpecifications.cs`   | `AuthorSpecifications.cs`   | ✅     |
| Individual Checks       | `CheckLocation()`, etc.     | `CheckFirstName()`, etc.    | ✅     |
| Internal Validation     | (in Entity)                 | `ValidateAuthorInternal()`  | ⚠️     |
| External Validation     | `ValidateSensorUniqueness()` | ❌ Nicht benötigt           | ✅     |
| **Application - Query** | `GetAllSensorsQuery`        | `GetAllAuthorsQuery`        | ✅     |
| Query Handler           | `GetAllSensorsQueryHandler` | `GetAllAuthorsQueryHandler` | ✅     |
| GetById Query           | `GetSensorByIdQuery`        | ❌ Fehlt                    | ⚠️     |
| GetById Handler         | `GetSensorByIdQueryHandler` | ❌ Fehlt                    | ⚠️     |
| **API Controller**      | `SensorsController`         | `AutorsController`          | ✅     |
| GetAll()                | ✅ Implementiert            | ✅ Implementiert            | ✅     |
| GetById()               | ✅ Implementiert            | ⚠️ NotImplementedException  | ⚠️     |

### Phase 2: Books ↔ Sensors

| Aspekt                   | Template (Sensors)           | Übung 2 (Books)              | Status |
| ------------------------ | ---------------------------- | ---------------------------- | ------ |
| **Domain Entity**        | `Sensor.cs`                  | `Book.cs`                    | ✅     |
| Factory Method           | `CreateAsync()`              | `CreateAsync()`              | ✅     |
| Update Method            | `UpdateAsync()`              | ⚠️ NotImplementedException   | ⚠️     |
| **Specifications**       | `SensorSpecifications.cs`    | `BookSpecifications.cs`      | ✅     |
| Individual Checks        | `CheckLocation()`, etc.      | `CheckISBN()`, etc.          | ✅     |
| Internal Validation      | `ValidateSensorProperties()` | ⚠️ NotImplementedException   | ⚠️     |
| External Validation      | `ValidateSensorUniqueness()` | ⚠️ NotImplementedException   | ⚠️     |
| **Uniqueness Checker**   | `SensorUniquenessChecker`    | `BookUniquenessChecker`      | ⚠️     |
| DI Registration          | ✅ Registered                | ✅ Registered                | ✅     |
| Implementation           | ✅ Implemented               | ⚠️ NotImplementedException   | ⚠️     |
| **Command - Create**     | `CreateSensorCommand`        | `CreateBookCommand`          | ❌     |
| Command Properties       | `Location, Name`             | ❌ `Location, Name, ...`     | ❌     |
| Command Handler          | ✅ Implemented               | ⚠️ NotImplementedException   | ⚠️     |
| Command Validator        | (auskommentiert)             | ❌ Leer                      | ⚠️     |
| **Query - GetAll**       | `GetAllSensorsQuery`         | `GetAllBooksQuery`           | ✅     |
| GetAll Handler           | ✅ With orderBy              | ⚠️ Without Include           | ⚠️     |
| GetById                  | ✅ Implemented               | ❌ Fehlt                     | ⚠️     |
| **API Controller**       | `SensorsController`          | `BooksController`            | ✅     |
| GetAll()                 | ✅ Implementiert             | ✅ Implementiert             | ✅     |
| GetById()                | ✅ Implementiert             | ⚠️ NotImplementedException   | ⚠️     |
| Create()                 | ✅ Implementiert             | ⚠️ NotImplementedException   | ⚠️     |
| Update()                 | ✅ Implementiert             | ❌ Fehlt komplett            | ⚠️     |
| Delete()                 | ✅ Implementiert             | ⚠️ NotImplementedException   | ⚠️     |

### Phase 3: Loans ↔ Measurements

| Aspekt                 | Template (Measurements)     | Übung 2 (Loans)            | Status |
| ---------------------- | --------------------------- | -------------------------- | ------ |
| **Domain Entity**      | `Measurement.cs`            | `Loan.cs`                  | ✅     |
| Factory Method         | `Create()`                  | ⚠️ NotImplementedException | ⚠️     |
| **Specifications**     | `MeasurementSpecifications` | `LoanSpecifications`       | ⚠️     |
| Individual Checks      | `CheckSensorId()`           | ❌ Alle fehlen             | ⚠️     |
| **Application**        | (Commands/Queries fehlen)   | ❌ Alle fehlen             | ⚠️     |
| **API Controller**     | `MeasurementsController`    | `LoansController`          | ⚠️     |
| Endpoints              | ✅ Implementiert            | ❌ Leer                    | ⚠️     |

</details>

---

> **Viel Erfolg! 🤓🤜🏻🤛🏻🤖**
>
> _Erstellt am: 2025-11-15_  
> _Aktualisiert am: 2025-11-16 (nach Code-Implementierung: Authors ~95%, Books ~75% fertig!)_  
> _Basierend auf: CleanArchitecture_Template & Kollegin-Notizen_
