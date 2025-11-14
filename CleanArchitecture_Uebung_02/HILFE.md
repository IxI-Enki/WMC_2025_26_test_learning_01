# 📚 Hilfe & Nützliche Informationen - Übung 02

Ergänzung zur **[README.md](./README.md)** - Hier findest du alle nützlichen Infos, Tipps und Anleitungen!

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

## 🚀 Projekt ausführen

### 1. Datenbank erstellen (mit Migrationen)

```bash
cd CleanArchitecture_Uebung_02/Infrastructure
dotnet ef migrations add InitialMigration --startup-project ../Api --output-dir ./Persistence/Migrations
dotnet ef database update --startup-project ../Api
```

**ODER:** Einfach die API starten - der DataSeeder macht Migrationen automatisch!

### 2. API starten

```bash
cd CleanArchitecture_Uebung_02/Api
dotnet run
```

**Was passiert beim Start:**

1. ✅ DataSeeder läuft automatisch (IHostedService)
2. ✅ Migrationen werden angewendet
3. ✅ Sample-Daten werden geladen (5 Authors)
4. ✅ API ist bereit unter: `https://localhost:5101/swagger`

**Sample-Daten nach dem Start:**

- 5 Authors: J.K. Rowling, George R.R. Martin, J.R.R. Tolkien, Agatha Christie, Stephen King
- Du kannst sofort Books über die API anlegen!

### 3. Tests ausführen

```bash
# Domain Tests
cd CleanArchitecture_Uebung_02/Domain.Tests
dotnet test

# API Tests (Integration Tests)
cd CleanArchitecture_Uebung_02/Api.Tests
dotnet test
```

---

## 🎯 Arbeitsweise (empfohlen)

**Wie die Kollegin arbeitet:**
> "Ich acker mich so durch, dass ich zuerst die Domain und Infrastruktur aufbaue, bevor ich mich an die API mache. Ich finde, wenn man UniquenessChecker und Validation etc. später erst macht, hat man ja überhaupt keinen Überblick, wo dann nachträglich nochmal was ergänzt werden muss."

**Empfohlene Reihenfolge:**

1. **Domain:** Validierungen implementieren
2. **Domain:** Entity Create/Update Methoden
3. **Infrastructure:** Repository-Methoden (spezielle Abfragen)
4. **Application:** DTOs erstellen
5. **Application:** Services (BookUniquenessChecker)
6. **Application:** DependencyInjection
7. **Application:** Commands & Queries (Handler + Validators)
8. **API:** Controller implementieren
9. **Tests:** Domain- und API-Tests ausführen

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
8. **MediatR** - CQRS-Dispatcher mit ValidationBehavior Pipeline
9. **Exception-Handling** - Zentrale ValidationBehavior (ALLE Exceptions → Result<T>)
10. **Hosted Services** - Background Tasks (DataSeeder)
11. **Data Seeding** - Automatische Datenbefüllung beim Start

---

## 📚 Wichtige Dateien zum Nachschlagen

### ✅ Template als Referenz

- `../CleanArchitecture_Template/Domain/Specifications/SensorSpecifications.cs`
- `../CleanArchitecture_Template/Domain/Entities/Sensor.cs`
- `../CleanArchitecture_Template/Application/Features/Sensors/Commands/CreateSensor/`
- `../CleanArchitecture_Template/Application/Services/SensorUniquenessChecker.cs`
- `../CleanArchitecture_Template/Api/Controllers/SensorsController.cs`
- `../CleanArchitecture_Template/Application/Pipeline/ValidationBehavior.cs` ⭐

### 📖 Frühere Aufgabenstellungen

- `../FruehereAufgabenstellungen/` - zeigt den Stil der Lücken vom Professor
- `../FruehereAufgabenstellungen/Infrastructure/Services/StartupDataSeeder.cs` - Beispiel DataSeeder

### ✅ DataSeeder in dieser Übung (FERTIG!)

- `Infrastructure/Services/StartupDataSeeder.cs` - **VOLLSTÄNDIG implementiert**
- `Infrastructure/Services/StartupDataSeederOptions.cs` - **VOLLSTÄNDIG implementiert**
- `Infrastructure/Data/library-seed-data.json` - Sample-Daten
- **Du musst diese Dateien NICHT ändern oder implementieren!**

### ✅ ValidationBehavior & Exception-Handling (FERTIG!)

- `Application/Pipeline/ValidationBehavior.cs` - **VOLLSTÄNDIG implementiert**
- `Application/Common/Exceptions/ConcurrencyException.cs` - **Hinzugefügt**
- **Diese Dateien sind 100% Template-kompatibel!**
- **Du musst diese Dateien NICHT ändern oder implementieren!**

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

### Wo finde ich den DataSeeder?

Der DataSeeder ist **VOLLSTÄNDIG implementiert** in:

- `Infrastructure/Services/StartupDataSeeder.cs`
- `Infrastructure/Services/StartupDataSeederOptions.cs`

**Du musst ihn NICHT implementieren!** Er läuft automatisch beim Start.

### Wie mappe ich Result zu ActionResult?

```csharp
var result = await mediator.Send(command, ct);
return result.ToActionResult(this, createdAtAction: nameof(GetById), 
    routeValues: new { id = result?.Value?.Id });
```

### Was ist der Unterschied zwischen Domain- und Application-Validation?

**Domain-Validation:**
- Grundlegende Geschäftsregeln
- Unabhängig vom UseCase
- In `Domain/Specifications/`
- Beispiel: "ISBN muss 13 Zeichen haben"

**Application-Validation (FluentValidation):**
- UseCase-spezifische Regeln
- In Command/Query Validators
- Kann Domain-Validation ergänzen
- Beispiel: "CreateBookCommand.ISBN NotEmpty"

### Wie implementiere ich einen Repository-Query mit Navigation Properties?

```csharp
public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken ct = default)
{
    return await Set
        .AsNoTracking()
        .Include(b => b.Author)  // Navigation Property laden
        .Where(b => b.AuthorId == authorId)
        .OrderBy(b => b.Title)
        .ToListAsync(ct);
}
```

### Was ist der Unterschied zwischen Internal und External Validation?

**Internal Validation:**
- Prüft Properties der Entität selbst
- Benötigt keine externe Dependencies
- Beispiel: `CheckISBN(string isbn)`

**External Validation:**
- Prüft gegen Datenbank oder externe Services
- Benötigt Dependencies (z.B. Repository)
- Beispiel: `ValidateBookExternal()` mit `IBookUniquenessChecker`

### Wie erstelle ich einen Command mit Handler?

**1. Command (Record):**

```csharp
public readonly record struct CreateBookCommand(
    string ISBN, 
    string Title, 
    int AuthorId, 
    int PublicationYear, 
    int AvailableCopies
) : IRequest<Result<GetBookDto>>;
```

**2. Handler (Class):**

```csharp
public sealed class CreateBookCommandHandler(
    IUnitOfWork uow, 
    IBookUniquenessChecker uniquenessChecker) 
    : IRequestHandler<CreateBookCommand, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle(
        CreateBookCommand request, 
        CancellationToken ct)
    {
        var author = await uow.Authors.GetByIdAsync(request.AuthorId, ct);
        if (author is null)
            return Result<GetBookDto>.NotFound("Author not found");

        var book = await Book.CreateAsync(
            request.ISBN, 
            request.Title, 
            author, 
            request.PublicationYear, 
            request.AvailableCopies, 
            uniquenessChecker, 
            ct);

        await uow.Books.AddAsync(book, ct);
        await uow.SaveChangesAsync(ct);

        return Result<GetBookDto>.Created(book.Adapt<GetBookDto>());
    }
}
```

**3. Validator (Class):**

```csharp
public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty()
            .Length(13);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1);

        RuleFor(x => x.AuthorId)
            .GreaterThan(0);

        RuleFor(x => x.PublicationYear)
            .GreaterThanOrEqualTo(1450)
            .LessThanOrEqualTo(DateTime.Now.Year + 1);

        RuleFor(x => x.AvailableCopies)
            .GreaterThanOrEqualTo(0);
    }
}
```

### Wie debugge ich ValidationBehavior?

**Setze Breakpoints in:**
1. `ValidationBehavior.Handle()` - Zeile mit `await next()`
2. Deinem CommandHandler - `Handle()`-Methode
3. Entity Factory-Methode - z.B. `Book.CreateAsync()`

**Prüfe:**
- Werden Validatoren ausgeführt?
- Welche Exception wird geworfen?
- Wird sie korrekt in `Result<T>` umgewandelt?

### Wie teste ich meine Domain-Validierungen?

```csharp
[Fact]
public void CheckISBN_WithInvalidLength_ShouldReturnError()
{
    // Arrange
    var isbn = "12345"; // zu kurz

    // Act
    var result = BookSpecifications.CheckISBN(isbn);

    // Assert
    result.IsValid.Should().BeFalse();
    result.ErrorMessage.Should().Contain("13 Zeichen");
}
```

---

## 💡 Tipps & Tricks

### Performance-Tipp: AsNoTracking()

Verwende `AsNoTracking()` bei Abfragen, die NICHT geändert werden:

```csharp
// Gut für Read-Only
var books = await Set.AsNoTracking().ToListAsync();

// Schlecht - ChangeTracker läuft umsonst
var books = await Set.ToListAsync();
```

### Tipp: Result Pattern konsequent verwenden

```csharp
// ✅ Gut - Keine Exceptions im Controller
public async Task<Result<GetBookDto>> Handle(...)
{
    if (author is null)
        return Result<GetBookDto>.NotFound("Author not found");
    
    // ...
}

// ❌ Schlecht - Exception im Controller
public async Task<GetBookDto> Handle(...)
{
    if (author is null)
        throw new NotFoundException("Author not found");
}
```

### Tipp: Navigation Properties laden

```csharp
// ✅ Explizit laden wenn benötigt
var books = await Set
    .Include(b => b.Author)
    .ToListAsync();

// ❌ Lazy Loading vermeiden (N+1 Problem)
var books = await Set.ToListAsync();
// books[0].Author.FirstName  // Extra DB-Query!
```

### Tipp: CancellationToken weitergeben

```csharp
// ✅ Immer weitergeben
public async Task<Result<GetBookDto>> Handle(
    CreateBookCommand request, 
    CancellationToken ct)
{
    await uow.Books.AddAsync(book, ct);
    await uow.SaveChangesAsync(ct);
}
```

---

## 🔍 Debugging-Checklist

Wenn etwas nicht funktioniert:

### Domain Layer
- [ ] Sind alle Check-Methoden implementiert?
- [ ] Werfen Validierungen die richtige `DomainValidationException`?
- [ ] Sind Factory-Methoden `static`?
- [ ] Wird `ArgumentNullException` geprüft?

### Application Layer
- [ ] Sind alle DTOs als `record` definiert?
- [ ] Implementieren Commands/Queries `IRequest<Result<T>>`?
- [ ] Sind Handler als `sealed class` definiert?
- [ ] Ist `IBookUniquenessChecker` in DI registriert?

### Infrastructure Layer
- [ ] Verwenden Queries `AsNoTracking()`?
- [ ] Sind Navigation Properties mit `.Include()` geladen?
- [ ] Gibt Repository `IReadOnlyCollection<T>` zurück?

### API Layer
- [ ] Verwenden Controller `mediator.Send()`?
- [ ] Wird `.ToActionResult(this)` aufgerufen?
- [ ] Sind `ProducesResponseType` Attribute vorhanden?

### Tests
- [ ] Sind Domain-Tests aktiviert (nicht auskommentiert)?
- [ ] Verwenden Tests `FluentAssertions`?
- [ ] Ist `TestWebApplicationFactory` korrekt konfiguriert?

---

## 📖 Weiterführende Links

- **[README.md](./README.md)** - Zurück zur Aufgabenstellung
- **[../CleanArchitecture_Template/](../CleanArchitecture_Template/)** - Template als Referenz
- **[../FruehereAufgabenstellungen/](../FruehereAufgabenstellungen/)** - Frühere Prüfungen

---

> **Viel Erfolg beim Üben! 🚀**

**Erstellt für WMC Test-Vorbereitung 2025** 🎓

