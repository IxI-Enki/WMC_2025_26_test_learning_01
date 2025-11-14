# 🎓 WMC Test-Übung 02: Library Management System

## 📋 Übersicht

**Domäne:** Bibliotheksverwaltung mit drei Entitäten:

- 📚 **Book** (Buch) - *ALLES mit NotImplementedException!*
- ✍️ **Author** (Autor) - *ALLES mit NotImplementedException!*
- 📖 **Loan** (Ausleihe) - *ALLES mit NotImplementedException!*

**Wichtig:** Diese Übung entspricht dem Professor-Stil! Fast ALLES ist mit `NotImplementedException` versehen und muss implementiert werden!

---

## 🎯 Was du beim Test implementieren musst (laut Kollegin)

### ✏️ 1. Domain-Validierungen

**Aufgabe:** Validations auf Domain-Ebene implementieren

📁 **Dateien:**

- `Domain/Specifications/BookSpecifications.cs`
- `Domain/Specifications/AuthorSpecifications.cs`
- `Domain/Specifications/LoanSpecifications.cs`

**Was zu tun ist:**

- Alle `Check...` Methoden implementieren (CheckISBN, CheckTitle, CheckAuthorId, usw.)
- `ValidateXXXInternal` Methoden implementieren
- `ValidateXXXExternal` Methoden implementieren (für Uniqueness)

**💡 Tipp:** Schaue dir `CleanArchitecture_Template/Domain/Specifications/SensorSpecifications.cs` an!

---

### ✏️ 2. Domain Entities (Create/Update Methoden)

**Aufgabe:** Factory-Methoden in Entitäten implementieren

📁 **Dateien:**

- `Domain/Entities/Book.cs` - `CreateAsync`, `UpdateAsync`
- `Domain/Entities/Author.cs` - `Create`
- `Domain/Entities/Loan.cs` - `Create`

**Was zu tun ist:**

- ArgumentNullException prüfen
- Trimmen von Strings
- Validierungen aufrufen
- Objekt erstellen und zurückgeben

---

### ✏️ 3. Commands & Queries mit Handlers und Validators

**Aufgabe:** Alle Commands/Queries mit Handler und Validator selbst erstellen

📁 **Book-Features (Beispiele vorhanden, aber mit NotImplementedException):**

- `Application/Features/Books/Commands/CreateBook/`
  - CreateBookCommandHandler ❌
  - CreateBookCommandValidator ❌
- `Application/Features/Books/Commands/DeleteBook/`
  - DeleteBookCommandHandler ❌
- `Application/Features/Books/Queries/GetAllBooks/`
  - GetAllBooksQueryHandler ❌
- `Application/Features/Books/Queries/GetBookById/`
  - GetBookByIdQueryHandler ❌

📁 **Loan-Features (nur Ordner vorhanden):**

- `Application/Features/Loans/Commands/CreateLoan/` - komplett erstellen!
- `Application/Features/Loans/Commands/ReturnLoan/` - komplett erstellen!
- `Application/Features/Loans/Queries/GetLoansByBook/` - komplett erstellen!
- `Application/Features/Loans/Queries/GetOverdueLoans/` - komplett erstellen!

**💡 Tipp:** Schaue dir das `CleanArchitecture_Template` an, wie Commands/Queries aufgebaut sind!

---

### ✏️ 4. Dependency Injection

**Aufgabe:** Services bei DI registrieren

📁 **Datei:** `Application/DependencyInjection.cs`

**Was zu tun ist:**

```csharp
// Diese Zeile ist auskommentiert - du musst sie aktivieren:
services.AddScoped<IBookUniquenessChecker, BookUniquenessChecker>();
```

**💡 Tipp:** Im Template ist `ISensorUniquenessChecker` registriert - schau dort!

---

### ✏️ 5. Services implementieren

**Aufgabe:** Uniqueness Checker implementieren

📁 **Datei:** `Application/Services/BookUniquenessChecker.cs`

**Was zu tun ist:**

- ISBN-Eindeutigkeit über Repository prüfen
- Bestehende Bücher mit gleicher ID ignorieren (bei Update)

---

### ✏️ 6. Controller implementieren

**Aufgabe:** Controller-Endpoints implementieren

📁 **Dateien:**

- `Api/Controllers/BooksController.cs` - alle Methoden ❌
- `Api/Controllers/LoansController.cs` - komplett leer

**Was zu tun ist:**

- MediatR verwenden: `await mediator.Send(...)`
- Result in ActionResult umwandeln: `.ToActionResult(this)`
- ProducesResponseType Attribute sind schon da

**Beispiel:**

```csharp
[HttpGet]
public async Task<IActionResult> GetAll(CancellationToken ct)
{
    var result = await mediator.Send(new GetAllBooksQuery(), ct);
    return result.ToActionResult(this);
}
```

---

### ✏️ 7. Repository-Methoden für spezielle Abfragen

**Aufgabe:** Spezifische Repository-Methoden implementieren

📁 **Dateien:**

- `Infrastructure/Persistence/Repositories/BookRepository.cs`
  - GetByISBNAsync ❌
  - GetBooksByAuthorAsync ❌
- `Infrastructure/Persistence/Repositories/AuthorRepository.cs`
  - GetAuthorsWithBooksAsync ❌
- `Infrastructure/Persistence/Repositories/LoanRepository.cs`
  - GetLoansByBookIdAsync ❌
  - GetActiveLoansByBorrowerAsync ❌
  - GetOverdueLoansAsync ❌

**💡 Tipp:**

- Verwende `Set.AsNoTracking()`
- Verwende `.Include()` für Navigation Properties
- Verwende `.Where()`, `.OrderBy()`, `.ToListAsync()`

**Laut Kollegin:** Die Repositories sind normalerweise fertig, aber spezielle Methoden müssen hinzugefügt werden!

---

## 🧪 Tests

### Domain-Tests

📁 `Domain.Tests/`

- `BookTests.cs` - Tests für Book (auskommentiert, aktiviere sie!)
- `LoanSpecificationsTests.cs` - Tests für Loan-Validierungen

**Wichtig:** Die Tests zeigen dir, welche Methodennamen erwartet werden!

### API-Tests

📁 `Api.Tests/`

- `Books/BooksEndpointTests.cs` - Integration Tests

**Laut Kollegin:** Tests für Domain- und API-Ebene sind vorhanden - Methodennamen abgleichen!

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

API: `https://localhost:5101/swagger`

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

- [ ] CreateBookCommandHandler
- [ ] CreateBookCommandValidator (FluentValidation Rules)
- [ ] DeleteBookCommandHandler
- [ ] GetAllBooksQueryHandler
- [ ] GetBookByIdQueryHandler
- [ ] BookUniquenessChecker implementieren
- [ ] CreateLoanCommand + Handler + Validator erstellen
- [ ] ReturnLoanCommand + Handler erstellen
- [ ] GetLoansByBookQuery + Handler erstellen
- [ ] GetOverdueLoansQuery + Handler erstellen
- [ ] DependencyInjection: IBookUniquenessChecker registrieren

### ☐ Infrastructure Layer

- [ ] BookRepository.GetByISBNAsync
- [ ] BookRepository.GetBooksByAuthorAsync
- [ ] AuthorRepository.GetAuthorsWithBooksAsync
- [ ] LoanRepository.GetLoansByBookIdAsync
- [ ] LoanRepository.GetActiveLoansByBorrowerAsync
- [ ] LoanRepository.GetOverdueLoansAsync

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

## 🎯 Arbeitsweise (empfohlen)

**Wie die Kollegin arbeitet:**
> "Ich acker mich so durch, dass ich zuerst die Domain und Infrastruktur aufbaue, bevor ich mich an die API mache. Ich finde, wenn man UniquenessChecker und Validation etc. später erst macht, hat man ja überhaupt keinen Überblick, wo dann nachträglich nochmal was ergänzt werden muss."

**Empfohlene Reihenfolge:**

1. **Domain:** Validierungen implementieren
2. **Domain:** Entity Create/Update Methoden
3. **Infrastructure:** Repository-Methoden (spezielle Abfragen)
4. **Application:** Services (BookUniquenessChecker)
5. **Application:** DependencyInjection
6. **Application:** Commands & Queries (Handler + Validators)
7. **API:** Controller implementieren
8. **Tests:** Domain- und API-Tests ausführen

---

## 🎓 Design Patterns & Konzepte

Diese Übung deckt ab:

1. **Clean Architecture** - 4 Layer Trennung
2. **CQRS** - Commands & Queries
3. **Repository Pattern** - Spezifische Methoden
4. **Domain-Driven Design** - Factory Methods, Validierungen
5. **Validation (3 Ebenen)**:
   - Domain: Specifications
   - Application: FluentValidation
   - Database: Foreign Keys, Unique Indexes
6. **Dependency Injection** - Service Registration
7. **Unit of Work** - Transaktionale Speicherung
8. **MediatR** - CQRS-Dispatcher

---

## 📚 Wichtige Dateien zum Nachschlagen

### ✅ Template als Referenz

- `../CleanArchitecture_Template/Domain/Specifications/SensorSpecifications.cs`
- `../CleanArchitecture_Template/Domain/Entities/Sensor.cs`
- `../CleanArchitecture_Template/Application/Features/Sensors/Commands/CreateSensor/`
- `../CleanArchitecture_Template/Application/Services/SensorUniquenessChecker.cs`
- `../CleanArchitecture_Template/Api/Controllers/SensorsController.cs`

### 📖 Frühere Aufgabenstellungen

- `../FruehereAufgabenstellungen/` - zeigt den Stil der Lücken vom Professor

---

## 🤔 Häufige Fragen

### Wie verwende ich Mapster?

```csharp
var dto = entity.Adapt<GetBookDto>();
// Oder mit Anpassungen:
var dto = entity.Adapt<GetBookDto>() with { AuthorName = entity.Author.FullName };
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

### Wie mappe ich Result zu ActionResult?

```csharp
var result = await mediator.Send(command, ct);
return result.ToActionResult(this, createdAtAction: nameof(GetById), 
    routeValues: new { id = result?.Value?.Id });
```

---

> **Viel Erfolg! Diese Übung ist näher am echten Test! 🚀**

---

**Erstellt für WMC Test-Vorbereitung 2025** 🎓
