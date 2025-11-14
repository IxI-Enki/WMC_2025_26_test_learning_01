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

### ✏️ 3. DTOs erstellen

**Aufgabe:** Data Transfer Objects für API-Responses erstellen

📁 **Datei:** `Application/Dtos/` (aktuell nur .gitkeep vorhanden)

**Was zu erstellen ist:**

- `GetBookDto.cs` - DTO für Book-Responses

  ```csharp
  public record GetBookDto(int Id, string ISBN, string Title, int AuthorId, 
      string AuthorName, int PublicationYear, int AvailableCopies);
  ```

- `GetAuthorDto.cs` - DTO für Author-Responses
- `GetLoanDto.cs` - DTO für Loan-Responses

**💡 Tipp:** DTOs sind einfache Records ohne Logik!

---

### ✏️ 4. Commands & Queries mit Handlers und Validators

**Aufgabe:** Alle Commands/Queries mit Handler und Validator selbst erstellen

📁 **Book-Features (nur Ordner mit .gitkeep vorhanden):**

- `Application/Features/Books/Commands/CreateBook/`
  - CreateBookCommand.cs ❌
  - CreateBookCommandHandler.cs ❌
  - CreateBookCommandValidator.cs ❌
- `Application/Features/Books/Commands/DeleteBook/`
  - DeleteBookCommand.cs ❌
  - DeleteBookCommandHandler.cs ❌
- `Application/Features/Books/Queries/GetAllBooks/`
  - GetAllBooksQuery.cs ❌
  - GetAllBooksQueryHandler.cs ❌
- `Application/Features/Books/Queries/GetBookById/`
  - GetBookByIdQuery.cs ❌
  - GetBookByIdQueryHandler.cs ❌

📁 **Loan-Features (nur Ordner vorhanden):**

- `Application/Features/Loans/Commands/CreateLoan/` - komplett erstellen!
- `Application/Features/Loans/Commands/ReturnLoan/` - komplett erstellen!
- `Application/Features/Loans/Queries/GetLoansByBook/` - komplett erstellen!
- `Application/Features/Loans/Queries/GetOverdueLoans/` - komplett erstellen!

**💡 Tipp:** Schaue dir das `CleanArchitecture_Template` an, wie Commands/Queries aufgebaut sind!

---

### ✏️ 5. Dependency Injection

**Aufgabe:** Services bei DI registrieren

📁 **Datei:** `Application/DependencyInjection.cs`

**Was zu tun ist:**

```csharp
// Diese Zeile ist auskommentiert - du musst sie aktivieren:
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

### ✏️ 7. Controller implementieren

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

### ✏️ 8. Repository-Methoden für spezielle Abfragen

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

**Repository-Methoden (spezielle Abfragen):**

- [ ] BookRepository.GetByISBNAsync
- [ ] BookRepository.GetBooksByAuthorAsync
- [ ] AuthorRepository.GetAuthorsWithBooksAsync
- [ ] LoanRepository.GetLoansByBookIdAsync
- [ ] LoanRepository.GetActiveLoansByBorrowerAsync
- [ ] LoanRepository.GetOverdueLoansAsync

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
