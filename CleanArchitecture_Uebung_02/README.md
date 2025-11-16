# 🎓 WMC Test-Übung 02: Library Management System

## 📋 Übersicht

**Domäne:** Bibliotheksverwaltung mit drei Entitäten:

- 📚 **Book** (Buch) - *~75% implementiert! (GET, POST, DELETE fertig; UPDATE fehlt noch)*
- ✍️ **Author** (Autor) - *~95% implementiert! (GET All, GET ById fertig; UPDATE fehlt noch)*
- 📖 **Loan** (Ausleihe) - *Noch nicht implementiert (0%)*

**Aktueller Stand (2025-11-16):**
- ✅ **Phase 1 (Authors):** GET All, GET ById, Domain-Validierungen **FERTIG!**
- ✅ **Phase 2 (Books):** GET All, GET ById, POST, DELETE, Domain-Validierungen **FERTIG!**
- ⏳ **Phase 3 (Loans):** Noch nicht begonnen

---

## 📐 Domain Model - Entities & Properties

### 📚 Book (Buch)

| Property          | Typ                 | Beschreibung                          |
| ----------------- | ------------------- | ------------------------------------- |
| `Id`              | `int`               | Primary Key (von BaseEntity)          |
| `ISBN`            | `string`            | ISBN-Nummer (13 Zeichen, nur Ziffern) |
| `Title`           | `string`            | Buchtitel                             |
| `AuthorId`        | `int`               | Foreign Key zum Author                |
| `Author`          | `Author`            | Navigation Property zum Author        |
| `PublicationYear` | `int`               | Veröffentlichungsjahr                 |
| `AvailableCopies` | `int`               | Anzahl verfügbarer Exemplare          |
| `Loans`           | `ICollection<Loan>` | Navigation Property zu Ausleihen      |

**Factory-Methoden:**

```csharp
// ✅ FERTIG IMPLEMENTIERT!
static Task<Book> CreateAsync(string isbn, string title, Author author, 
    int publicationYear, int availableCopies, IBookUniquenessChecker uniquenessChecker, 
    CancellationToken ct = default)

// ⚠️ NOCH ZU IMPLEMENTIEREN:
Task UpdateAsync(string isbn, string title, int authorId, int publicationYear, 
    int availableCopies, IBookUniquenessChecker uniquenessChecker, CancellationToken ct = default)
```

**Fertige Methoden:**

- ✅ `CreateAsync()` - Erstellt ein neues Buch mit Validierungen
- ✅ `DecreaseCopies()` - Reduziert AvailableCopies um 1 (beim Ausleihen)
- ✅ `IncreaseCopies()` - Erhöht AvailableCopies um 1 (beim Zurückgeben)

---

### ✍️ Author (Autor)

| Property      | Typ                 | Beschreibung                   |
| ------------- | ------------------- | ------------------------------ |
| `Id`          | `int`               | Primary Key (von BaseEntity)   |
| `FirstName`   | `string`            | Vorname                        |
| `LastName`    | `string`            | Nachname                       |
| `DateOfBirth` | `DateTime`          | Geburtsdatum                   |
| `Books`       | `ICollection<Book>` | Navigation Property zu Büchern |

**Computed Property:**

- `FullName` → `$"{FirstName} {LastName}"` (bereits implementiert)

**Factory-Methode:**

```csharp
// ✅ FERTIG IMPLEMENTIERT!
static Author Create(string firstName, string lastName, DateTime dateOfBirth)
```

---

### 📖 Loan (Ausleihe)

| Property       | Typ         | Beschreibung                                          |
| -------------- | ----------- | ----------------------------------------------------- |
| `Id`           | `int`       | Primary Key (von BaseEntity)                          |
| `BookId`       | `int`       | Foreign Key zum Book                                  |
| `Book`         | `Book`      | Navigation Property zum Book                          |
| `BorrowerName` | `string`    | Name des Ausleihers                                   |
| `LoanDate`     | `DateTime`  | Ausleihdatum                                          |
| `DueDate`      | `DateTime`  | Rückgabedatum (LoanDate + 14 Tage)                    |
| `ReturnDate`   | `DateTime?` | Tatsächliches Rückgabedatum (null = noch ausgeliehen) |

**Factory-Methode (zu implementieren):**

```csharp
static Loan Create(Book book, string borrowerName, DateTime loanDate)
// DueDate = LoanDate + 14 Tage
```

**Fertige Methoden:**

- `MarkAsReturned(DateTime returnDate)` - Setzt ReturnDate
- `IsOverdue()` - Prüft ob überfällig (ReturnDate == null && DateTime.Now > DueDate)

---

## 📦 DTOs - Was zu erstellen ist

### GetBookDto

📁 `Application/Dtos/GetBookDto.cs`

```csharp
namespace Application.Dtos;

public sealed record GetBookDto(
    int Id, 
    string ISBN, 
    string Title, 
    int AuthorId, 
    string AuthorName,           // ← Aus Author.FullName!
    int PublicationYear, 
    int AvailableCopies
);
```

---

### GetAuthorDto

📁 `Application/Dtos/GetAuthorDto.cs`

```csharp
namespace Application.Dtos;

public sealed record GetAuthorDto(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth
);
```

**💡 Tipp:** Im QueryHandler kannst du `author.Adapt<GetAuthorDto>()` verwenden!

---

### GetLoanDto

📁 `Application/Dtos/GetLoanDto.cs`

```csharp
namespace Application.Dtos;

public sealed record GetLoanDto(
    int Id,
    int BookId,
    string BookTitle,            // ← Aus Book.Title!
    string BorrowerName,
    DateTime LoanDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    bool IsOverdue               // ← Berechnet: ReturnDate == null && DateTime.Now > DueDate
);
```

---

## 📝 Commands & Queries - Signaturen

### CreateBookCommand ✅ FERTIG!

📁 `Application/Features/Books/Commands/CreateBook/CreateBookCommand.cs`

```csharp
// ✅ FERTIG IMPLEMENTIERT!
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

public readonly record struct CreateBookCommand(
    string ISBN,
    string Title,
    int AuthorId,
    int PublicationYear,
    int AvailableCopies
) : IRequest<Result<GetBookDto>>;
```

---

### DeleteBookCommand

📁 `Application/Features/Books/Commands/DeleteBook/DeleteBookCommand.cs`

```csharp
using Application.Common.Results;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

public readonly record struct DeleteBookCommand(int Id) : IRequest<Result<bool>>;
```

---

### GetAllBooksQuery

📁 `Application/Features/Books/Queries/GetAllBooks/GetAllBooksQuery.cs`

```csharp
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetAllBooks;

public readonly record struct GetAllBooksQuery : IRequest<Result<IReadOnlyCollection<GetBookDto>>>;
```

---

### GetBookByIdQuery

📁 `Application/Features/Books/Queries/GetBookById/GetBookByIdQuery.cs`

```csharp
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetBookById;

public readonly record struct GetBookByIdQuery(int Id) : IRequest<Result<GetBookDto>>;
```

---

### CreateLoanCommand

📁 `Application/Features/Loans/Commands/CreateLoan/CreateLoanCommand.cs`

```csharp
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Loans.Commands.CreateLoan;

public readonly record struct CreateLoanCommand(
    int BookId,
    string BorrowerName,
    DateTime LoanDate
) : IRequest<Result<GetLoanDto>>;
```

---

### ReturnLoanCommand

📁 `Application/Features/Loans/Commands/ReturnLoan/ReturnLoanCommand.cs`

```csharp
using Application.Common.Results;
using MediatR;

namespace Application.Features.Loans.Commands.ReturnLoan;

public readonly record struct ReturnLoanCommand(
    int LoanId,
    DateTime ReturnDate
) : IRequest<Result<bool>>;
```

---

### GetLoansByBookQuery

📁 `Application/Features/Loans/Queries/GetLoansByBook/GetLoansByBookQuery.cs`

```csharp
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Loans.Queries.GetLoansByBook;

public readonly record struct GetLoansByBookQuery(int BookId) 
    : IRequest<Result<IReadOnlyCollection<GetLoanDto>>>;
```

---

### GetOverdueLoansQuery

📁 `Application/Features/Loans/Queries/GetOverdueLoans/GetOverdueLoansQuery.cs`

```csharp
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Loans.Queries.GetOverdueLoans;

public readonly record struct GetOverdueLoansQuery 
    : IRequest<Result<IReadOnlyCollection<GetLoanDto>>>;
```

---

## 🔌 Repository-Interfaces - Methodensignaturen

### IBookRepository

📁 `Application/Interfaces/Repositories/IBookRepository.cs`

```csharp
public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default);
    Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken ct = default);
}
```

---

### IAuthorRepository

📁 `Application/Interfaces/Repositories/IAuthorRepository.cs`

```csharp
public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<IReadOnlyCollection<Author>> GetAuthorsWithBooksAsync(CancellationToken ct = default);
}
```

**💡 Tipp:** Diese Signatur ist **bereits** im Code vorhanden!

---

### ILoanRepository

📁 `Application/Interfaces/Repositories/ILoanRepository.cs`

```csharp
public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<IReadOnlyCollection<Loan>> GetLoansByBookIdAsync(int bookId, CancellationToken ct = default);
    Task<IReadOnlyCollection<Loan>> GetActiveLoansByBorrowerAsync(string borrowerName, CancellationToken ct = default);
    Task<IReadOnlyCollection<Loan>> GetOverdueLoansAsync(CancellationToken ct = default);
}
```

---

## 🎯 Implementierungs-Status

### ✅ 1. Domain-Validierungen (FERTIG!)

**Status:** ✅ **Für Authors & Books vollständig implementiert!**

📁 **Dateien:**

- ✅ `Domain/Specifications/BookSpecifications.cs` - **FERTIG!**
  - ✅ Alle `Check...` Methoden (CheckISBN, CheckTitle, CheckAuthorId, etc.)
  - ✅ `ValidateBookInternal` implementiert
  - ✅ `ValidateBookExternal` implementiert
- ✅ `Domain/Specifications/AuthorSpecifications.cs` - **FERTIG!**
  - ✅ Alle `Check...` Methoden (CheckFirstName, CheckLastName, CheckDateOfBirth)
  - ✅ `ValidateAuthorInternal` implementiert
- ⏳ `Domain/Specifications/LoanSpecifications.cs` - **Noch nicht gestartet**

**💡 Tipp:** Schaue dir `CleanArchitecture_Template/Domain/Specifications/SensorSpecifications.cs` an!

---

### ✅ 2. Domain Entities (Create/Update Methoden)

**Status:** ✅ **Authors & Books Create-Methoden fertig!**

📁 **Dateien:**

- ✅ `Domain/Entities/Book.cs` - `CreateAsync` **FERTIG!**, `UpdateAsync` ⚠️ noch offen
- ✅ `Domain/Entities/Author.cs` - `Create` **FERTIG!**
- ⏳ `Domain/Entities/Loan.cs` - `Create` noch nicht implementiert

**Was implementiert ist:**

- ✅ ArgumentNullException prüfen
- ✅ Trimmen von Strings
- ✅ Validierungen aufrufen
- ✅ Objekt erstellen und zurückgeben

---

### ✅ 3. DTOs erstellen (FERTIG für Authors & Books!)

**Status:** ✅ **GetAuthorDto & GetBookDto fertig implementiert!**

📁 **Datei:** `Application/Dtos/`

**Was bereits existiert:**

- ✅ `GetBookDto.cs` - **FERTIG IMPLEMENTIERT!**
- ✅ `GetAuthorDto.cs` - **FERTIG IMPLEMENTIERT!**
- ⏳ `GetLoanDto.cs` - Noch zu erstellen (siehe Abschnitt "📦 DTOs" oben für Definition!)

**💡 Tipp:** DTOs sind einfache Records ohne Logik! Die vollständigen Definitionen findest du im Abschnitt "📦 DTOs" oben.

---

### ✅ 4. Commands & Queries mit Handlers und Validators

**Status:** ✅ **Authors & Books Queries/Commands größtenteils fertig!**

📁 **Author-Features:**

- ✅ `Application/Features/Authors/Queries/GetAllAuthors/` - **FERTIG!**
  - ✅ GetAllAuthorsQuery.cs
  - ✅ GetAllAuthorsQueryHandler.cs
- ✅ `Application/Features/Authors/Queries/GetAuthorById/` - **FERTIG!**
  - ✅ GetAuthorByIdQuery.cs
  - ✅ GetAuthorByIdQueryHandler.cs (mit Null-Check!)

📁 **Book-Features:**

- ✅ `Application/Features/Books/Commands/CreateBook/` - **FERTIG!**
  - ✅ CreateBookCommand.cs (mit korrekten Properties!)
  - ✅ CreateBookCommandHandler.cs (vollständig implementiert!)
  - ⚠️ CreateBookCommandValidator.cs (noch leer, optional)
- ✅ `Application/Features/Books/Commands/DeleteBook/` - **FERTIG!**
  - ✅ DeleteBookCommand.cs
  - ✅ DeleteBookCommandHandler.cs
- ✅ `Application/Features/Books/Queries/GetAllBooks/` - **FERTIG!**
  - ✅ GetAllBooksQuery.cs
  - ✅ GetAllBooksQueryHandler.cs
- ✅ `Application/Features/Books/Queries/GetBookById/` - **FERTIG!**
  - ✅ GetBookByIdQuery.cs
  - ✅ GetBookByIdQueryHandler.cs (mit Null-Check!)

📁 **Loan-Features (nur Ordner vorhanden):**

- `Application/Features/Loans/Commands/CreateLoan/` - komplett erstellen! (Signaturen siehe oben)
- `Application/Features/Loans/Commands/ReturnLoan/` - komplett erstellen! (Signaturen siehe oben)
- `Application/Features/Loans/Queries/GetLoansByBook/` - komplett erstellen! (Signaturen siehe oben)
- `Application/Features/Loans/Queries/GetOverdueLoans/` - komplett erstellen! (Signaturen siehe oben)

**💡 Tipp:**

- Schaue dir das `CleanArchitecture_Template` an, wie Commands/Queries aufgebaut sind!
- Die **exakten Signaturen** findest du im Abschnitt "📝 Commands & Queries - Signaturen" oben!

---

### ✅ 5. Dependency Injection (FERTIG!)

**Status:** ✅ **IBookUniquenessChecker ist registriert!**

📁 **Datei:** `Application/DependencyInjection.cs`

**Was bereits implementiert ist:**

```csharp
// ✅ FERTIG - IBookUniquenessChecker ist registriert!
services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();
```

**💡 Tipp:** Im Template ist `ISensorUniquenessChecker` registriert - schau dort!

---

### ✏️ 6. Services implementieren

**Aufgabe:** Uniqueness Checker implementieren

📁 **Datei:** `Application/Services/BookUniquenessChecker.cs`

**Was zu tun ist:**

- ISBN-Eindeutigkeit über Repository prüfen
- Bestehende Bücher mit gleicher ID ignorieren (bei Update)

---

### ✅ 7. Controller implementieren

**Status:** ✅ **Authors & Books Controller größtenteils fertig!**

📁 **Dateien:**

- ✅ `Api/Controllers/AutorsController.cs` - **FERTIG!**
  - ✅ GetAll() - FERTIG
  - ✅ GetById(int id) - FERTIG
- ✅ `Api/Controllers/BooksController.cs` - **~80% FERTIG!**
  - ✅ GetAll() - FERTIG
  - ✅ GetById(int id) - FERTIG
  - ✅ Create() - FERTIG
  - ✅ Delete(int id) - FERTIG
  - ⚠️ Update(int id) - Noch nicht implementiert
- ⏳ `Api/Controllers/LoansController.cs` - Noch nicht gestartet

**Implementiert mit:**

- ✅ MediatR: `await mediator.Send(...)`
- ✅ Result in ActionResult: `.ToActionResult(this)`
- ✅ ProducesResponseType Attribute

**Beispiel:**

```csharp
// ✅ FERTIG IMPLEMENTIERT!
[HttpGet]
public async Task<IActionResult> GetAll(CancellationToken ct)
{
    var result = await mediator.Send(new GetAllBooksQuery(), ct);
    return result.ToActionResult(this);
}
```

---

### ✏️ 8. Repository-Methoden für spezielle Abfragen

**Aufgabe:** Spezifische Repository-Methoden implementieren

📁 **Dateien:**

- `Infrastructure/Persistence/Repositories/BookRepository.cs`
  - GetByISBNAsync ❌ (Signatur siehe Abschnitt "🔌 Repository-Interfaces" oben!)
  - GetBooksByAuthorAsync ❌ (Signatur siehe Abschnitt "🔌 Repository-Interfaces" oben!)
- `Infrastructure/Persistence/Repositories/AuthorRepository.cs`
  - GetAuthorsWithBooksAsync ❌ (Signatur siehe Abschnitt "🔌 Repository-Interfaces" oben!)
- `Infrastructure/Persistence/Repositories/LoanRepository.cs`
  - GetLoansByBookIdAsync ❌ (Signatur siehe Abschnitt "🔌 Repository-Interfaces" oben!)
  - GetActiveLoansByBorrowerAsync ❌ (Signatur siehe Abschnitt "🔌 Repository-Interfaces" oben!)
  - GetOverdueLoansAsync ❌ (Signatur siehe Abschnitt "🔌 Repository-Interfaces" oben!)

**💡 Tipp:**

- Verwende `Set.AsNoTracking()`
- Verwende `.Include()` für Navigation Properties
- Verwende `.Where()`, `.OrderBy()`, `.ToListAsync()`
- Die **exakten Methodensignaturen** findest du im Abschnitt "🔌 Repository-Interfaces" oben!

**Laut Kollegin:** Die Repositories sind normalerweise fertig, aber spezielle Methoden müssen hinzugefügt werden!

**⚠️ WICHTIG:** Du musst die Methodensignaturen auch in den **Interfaces** (`IBookRepository`, `ILoanRepository`) hinzufügen! (Siehe Abschnitt "🔌 Repository-Interfaces" oben)

---

## ✅ WICHTIGER HINWEIS: DataSeeder (FERTIG IMPLEMENTIERT!)

**Laut Kollegin (Zeile 33 im Prompt):**
> "In der Infrastruktur wird DataSeeder und Repositories fertig sein."

**Status in dieser Übung:**

- ✅ **DataSeeder ist VOLLSTÄNDIG implementiert!**
- ✅ Repositories mit NotImplementedException (nur spezielle Methoden)

**Der DataSeeder:**

- 📁 `Infrastructure/Services/StartupDataSeeder.cs` - **FERTIG!**
- 📁 `Infrastructure/Services/StartupDataSeederOptions.cs` - **FERTIG!**
- 📁 `Infrastructure/Data/library-seed-data.json` - Sample-Daten
- ✅ Ist als IHostedService registriert
- ✅ Läuft beim Start automatisch
- ✅ Lädt Authors aus JSON
- ✅ Ist idempotent (seeded nur einmal)

**Du musst den DataSeeder NICHT implementieren oder verstehen!**

Er läuft automatisch beim ersten Start und befüllt die Datenbank mit:

- 5 Authors (J.K. Rowling, George R.R. Martin, J.R.R. Tolkien, Agatha Christie, Stephen King)
- Die JSON-Datei kann erweitert werden mit Books

**Beim echten Test:**

- Der DataSeeder wird genau so **vollständig implementiert** sein
- Du musst ihn **NICHT** anfassen oder ändern
- Er gibt dir sofort Testdaten zum Arbeiten

---

## ✅ WICHTIGER HINWEIS: ValidationBehavior & Exception-Handling (FERTIG IMPLEMENTIERT!)

**Status in dieser Übung:**

- ✅ **ValidationBehavior ist VOLLSTÄNDIG implementiert!** (100% Template-kompatibel)
- ✅ **ConcurrencyException wurde hinzugefügt!**

**Was bedeutet das?**

Die `ValidationBehavior` (MediatR Pipeline) ist das **zentrale Exception-Handling** der Anwendung:

📁 **Dateien (alle FERTIG!):**

- `Application/Pipeline/ValidationBehavior.cs` - **VOLLSTÄNDIG implementiert!**
- `Application/Common/Exceptions/ConcurrencyException.cs` - **NEU hinzugefügt!**
- `Application/Common/Exceptions/NotFoundException.cs` - Bereits vorhanden

**Was die ValidationBehavior macht:**

1. ✅ **FluentValidation** - Fängt Validierungsfehler ab → `ValidationError`
2. ✅ **DomainValidationException** - Fängt Domain-Fehler ab → `ValidationError`
3. ✅ **NotFoundException** - Entity nicht gefunden → `NotFound`
4. ✅ **ConcurrencyException** - Konkurrierende Updates → `Conflict`
5. ✅ **Exception** (Generic) - Alle anderen Fehler → `Error`

**Wichtig:**

- ❌ **KEINE** separate ValidationExceptionMiddleware!
- ✅ **ValidationBehavior** macht das komplette Exception-Handling in der MediatR-Pipeline
- ✅ Wandelt alle Exceptions automatisch in `Result<T>` um
- ✅ Controller verwenden nur `ResultExtensions.ToActionResult()`

**Beim echten Test:**

- ValidationBehavior ist wahrscheinlich **NICHT vollständig** implementiert
- Du musst möglicherweise Exception-Handler hinzufügen
- **Schaue dir das Template an!** → `CleanArchitecture_Template/Application/Pipeline/ValidationBehavior.cs`

**Du musst ValidationBehavior und ConcurrencyException NICHT implementieren!** (In dieser Übung sind sie fertig)

---

## 📝 Validierungsregeln - Was zu implementieren ist

### Book (Domain Validation)

| Property        | Regel                           | Fehlermeldung                                                       |
| --------------- | ------------------------------- | ------------------------------------------------------------------- |
| ISBN            | NotEmpty                        | "ISBN darf nicht leer sein."                                        |
| ISBN            | Length = 13 (ohne Bindestriche) | "ISBN muss genau 13 Zeichen haben (ohne Bindestriche)."             |
| ISBN            | OnlyDigits                      | "ISBN darf nur Ziffern enthalten."                                  |
| ISBN            | Unique                          | "Ein Buch mit dieser ISBN existiert bereits."                       |
| Title           | NotEmpty                        | "Title darf nicht leer sein."                                       |
| Title           | MinLength(1)                    | "Title muss mindestens 1 Zeichen haben."                            |
| AuthorId        | > 0                             | "AuthorId muss größer als 0 sein."                                  |
| PublicationYear | >= 1450                         | "PublicationYear muss mindestens 1450 sein."                        |
| PublicationYear | <= Now + 1 Jahr                 | "PublicationYear darf nicht mehr als 1 Jahr in der Zukunft liegen." |
| AvailableCopies | >= 0                            | "AvailableCopies muss mindestens 0 sein."                           |

### Book (FluentValidation)

**Aufgabe:** Implementiere die Rules in `CreateBookCommandValidator`

- ISBN: NotEmpty, Length(13)
- Title: NotEmpty, MinimumLength(1)
- AuthorId: GreaterThan(0)
- PublicationYear: GreaterThanOrEqualTo(1450), LessThanOrEqualTo(DateTime.Now.Year + 1)
- AvailableCopies: GreaterThanOrEqualTo(0)

### Loan (Domain Validation)

| Property     | Regel                     | Fehlermeldung                                   |
| ------------ | ------------------------- | ----------------------------------------------- |
| BookId       | > 0                       | "BookId muss größer als 0 sein."                |
| BorrowerName | NotEmpty                  | "BorrowerName darf nicht leer sein."            |
| BorrowerName | MinLength(2)              | "BorrowerName muss mindestens 2 Zeichen haben." |
| LoanDate     | Not in future             | "LoanDate darf nicht in der Zukunft liegen."    |
| DueDate      | Auto (LoanDate + 14 Tage) | -                                               |

---

## ✅ Checkliste (laut Kollegin)

### ☐ Domain Layer

- [ ] BookSpecifications: alle Check-Methoden
- [ ] BookSpecifications: ValidateBookInternal
- [ ] BookSpecifications: ValidateBookExternal
- [ ] AuthorSpecifications: alle Check-Methoden + Validate
- [ ] LoanSpecifications: alle Check-Methoden + Validate
- [ ] Book.CreateAsync
- [ ] Book.UpdateAsync
- [ ] Author.Create
- [ ] Loan.Create

### ☐ Application Layer

**DTOs:**

- [ ] GetBookDto erstellen
- [ ] GetAuthorDto erstellen
- [ ] GetLoanDto erstellen

**Book Commands/Queries:**

- [ ] CreateBookCommand + Handler + Validator erstellen
- [ ] DeleteBookCommand + Handler erstellen
- [ ] GetAllBooksQuery + Handler erstellen
- [ ] GetBookByIdQuery + Handler erstellen

**Loan Commands/Queries:**

- [ ] CreateLoanCommand + Handler + Validator erstellen
- [ ] ReturnLoanCommand + Handler erstellen
- [ ] GetLoansByBookQuery + Handler erstellen
- [ ] GetOverdueLoansQuery + Handler erstellen

**Services & DI:**

- [ ] BookUniquenessChecker implementieren
- [ ] DependencyInjection: IBookUniquenessChecker registrieren

### ☐ Infrastructure Layer

**Repository-Interfaces (Methodensignaturen hinzufügen):**

- [ ] IBookRepository: GetByISBNAsync, GetBooksByAuthorAsync hinzufügen
- [ ] ILoanRepository: GetLoansByBookIdAsync, GetActiveLoansByBorrowerAsync, GetOverdueLoansAsync hinzufügen
- [ ] IAuthorRepository: GetAuthorsWithBooksAsync (bereits vorhanden ✅)

**Repository-Methoden (Implementierungen):**

- [ ] BookRepository.GetByISBNAsync implementieren
- [ ] BookRepository.GetBooksByAuthorAsync implementieren
- [ ] AuthorRepository.GetAuthorsWithBooksAsync implementieren
- [ ] LoanRepository.GetLoansByBookIdAsync implementieren
- [ ] LoanRepository.GetActiveLoansByBorrowerAsync implementieren
- [ ] LoanRepository.GetOverdueLoansAsync implementieren

**DataSeeder (FERTIG - musst du NICHT machen!):**

- ✅ StartupDataSeeder.cs - VOLLSTÄNDIG implementiert
- ✅ StartupDataSeederOptions.cs - VOLLSTÄNDIG implementiert
- ✅ library-seed-data.json - Sample-Daten vorhanden
- ✅ Als IHostedService registriert

**ValidationBehavior & Exception-Handling (FERTIG - musst du NICHT machen!):**

- ✅ ValidationBehavior.cs - VOLLSTÄNDIG implementiert (100% Template-kompatibel)
- ✅ ConcurrencyException.cs - Hinzugefügt
- ✅ Alle Exception-Handler implementiert (ValidationException, DomainValidationException, NotFoundException, ConcurrencyException, Exception)

### ☐ API Layer

- [ ] BooksController.GetAll
- [ ] BooksController.GetById
- [ ] BooksController.Create
- [ ] BooksController.Delete
- [ ] LoansController: alle Endpoints erstellen

### ☐ Tests & Ausführung

- [ ] Domain.Tests: BookTests aktivieren (auskommentierte Tests)
- [ ] Domain.Tests: LoanSpecificationsTests grün machen
- [ ] Projekt kompiliert ohne Fehler
- [ ] Datenbank Migration erfolgreich
- [ ] API läuft und Swagger ist erreichbar

---

## 📚 Weitere Hilfe

Für weitere Informationen siehe **[HILFE.md](./HILFE.md)**:

- 🧪 Tests
- 🚀 Projekt ausführen
- 🎯 Empfohlene Arbeitsweise
- 🎓 Design Patterns & Konzepte
- 📖 Wichtige Dateien zum Nachschlagen
- 🤔 Häufige Fragen

---

> **Viel Erfolg! Diese Übung ist näher am echten Test! 🚀**

**Erstellt für WMC Test-Vorbereitung 2025** 🎓
