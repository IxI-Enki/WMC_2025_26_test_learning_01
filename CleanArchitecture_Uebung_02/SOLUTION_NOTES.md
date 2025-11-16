# 📚 Library Management System - Lösungshinweise

## ✅ Vollständige Implementierung (solution/uebung-02)

Diese Branch enthält die **vollständige, funktionierende Lösung** der Library Management System Übung.

---

## 🎯 Implementierte Features

### 1. **Domain Layer** ✅

#### Entities mit Factory-Methoden

```csharp
// Author.cs
public static async Task<Author> CreateAsync(
    string firstName,
    string lastName,
    DateTime dateOfBirth,
    IAuthorUniquenessChecker uc,
    CancellationToken ct = default)
{
    // 1. Trimmen
    var trimmedFirstName = (firstName ?? string.Empty).Trim();
    var trimmedLastName = (lastName ?? string.Empty).Trim();
    
    // 2. Domain Validation (Internal)
    AuthorSpecifications.ValidateAuthorInternal(
        trimmedFirstName, trimmedLastName, dateOfBirth);
    
    // 3. External Validation (Uniqueness)
    var fullName = $"{trimmedFirstName} {trimmedLastName}";
    await AuthorSpecifications.ValidateAuthorExternal(0, fullName, uc, ct);
    
    // 4. Objekt erstellen
    return new Author
    {
        FirstName = trimmedFirstName,
        LastName = trimmedLastName,
        DateOfBirth = dateOfBirth
    };
}
```

**💡 Wichtige Konzepte:**
- **Validation SOFORT in Factory-Methode**
- **Internal vor External Validation**
- **Keine ungültigen Objekte möglich**
- **Async wegen External Validation**

#### Book mit Navigation Property

```csharp
// Book.cs
public class Book : BaseEntity
{
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;  // ← Navigation Property
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }
    
    public static async Task<Book> CreateAsync(
        string isbn,
        string title,
        Author author,  // ← Author-Objekt, nicht nur ID!
        int publicationYear,
        int availableCopies,
        IBookUniquenessChecker uc,
        CancellationToken ct = default)
    {
        // Validation...
        
        return new Book
        {
            ISBN = trimmedIsbn,
            Title = trimmedTitle,
            Author = author,  // ← EF Core setzt AuthorId automatisch!
            PublicationYear = publicationYear,
            AvailableCopies = availableCopies
        };
    }
}
```

**💡 Wichtige Konzepte:**
- **Navigation Property mit `= null!`** (Required)
- **EF Core setzt Foreign Key automatisch**
- **Factory-Methode bekommt Author-Objekt**

#### ValidationSpecifications

```csharp
// BookSpecifications.cs
public static void ValidateBookInternal(
    string isbn,
    string title,
    int authorId,
    int publicationYear,
    int availableCopies)
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

**💡 Wichtige Konzepte:**
- **Alle Validierungen sammeln**
- **Erste fehlerhafte wirft Exception**
- **DomainValidationResult Pattern**

---

### 2. **Application Layer** ✅

#### Commands mit Handler und Validator

**CreateBookCommand.cs:**
```csharp
public readonly record struct CreateBookCommand(
    string ISBN, 
    string Title, 
    int AuthorId,  // ← Nur ID, kein ganzes Objekt!
    int PublicationYear, 
    int AvailableCopies
) : IRequest<Result<GetBookDto>>;
```

**CreateBookCommandHandler.cs:**
```csharp
public sealed class CreateBookCommandHandler(
    IUnitOfWork uow, 
    IBookUniquenessChecker uniquenessChecker)
    : IRequestHandler<CreateBookCommand, Result<GetBookDto>>
{
    public async Task<Result<GetBookDto>> Handle(
        CreateBookCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Author aus DB laden (WICHTIG!)
        var author = await uow.Authors.GetByIdAsync(
            request.AuthorId, cancellationToken);
        
        if (author == null)
            return Result<GetBookDto>.NotFound(
                $"Author with ID {request.AuthorId} not found.");
        
        // 2. Book über Factory erstellen
        var entity = await Book.CreateAsync(
            request.ISBN, 
            request.Title, 
            author,  // ← Vollständiges Author-Objekt!
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
}
```

**💡 Wichtige Konzepte:**
- **Command hat nur ID, Handler lädt Objekt**
- **Factory-Methode bekommt vollständiges Author-Objekt**
- **Result Pattern für HTTP Status Codes**
- **Mapster für DTO-Mapping**

**CreateBookCommandValidator.cs:**
```csharp
public sealed class CreateBookCommandValidator 
    : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN ist erforderlich.")
            .Length(10, 17).WithMessage("ISBN muss zwischen 10 und 17 Zeichen lang sein.");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title ist erforderlich.")
            .MinimumLength(2).WithMessage("Title muss mindestens 2 Zeichen lang sein.");
        
        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("AuthorId muss größer als 0 sein.");
        
        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1450, DateTime.Now.Year + 1)
            .WithMessage($"PublicationYear muss zwischen 1450 und {DateTime.Now.Year + 1} liegen.");
        
        RuleFor(x => x.AvailableCopies)
            .GreaterThanOrEqualTo(0)
            .WithMessage("AvailableCopies muss >= 0 sein.");
    }
}
```

**💡 Wichtige Konzepte:**
- **FluentValidation für UseCase-Regeln**
- **Läuft in ValidationBehavior (MediatR Pipeline)**
- **NICHT für Domain-Regeln!**

#### DTOs mit Mapster

**GetBookDto.cs:**
```csharp
public sealed record GetBookDto(
    int Id,
    string ISBN,
    string Title,
    int AuthorId,
    string AuthorName,  // ← Computed Property!
    int PublicationYear,
    int AvailableCopies
);
```

**BookMappingConfig.cs:**
```csharp
public static class BookMappingConfig
{
    public static void ConfigureBookMappings()
    {
        TypeAdapterConfig<Book, GetBookDto>.NewConfig()
            .Map(dest => dest.ISBN, src => src.ISBN)
            .Map(dest => dest.AuthorId, src => src.AuthorId)
            .Map(dest => dest.AvailableCopies, src => src.AvailableCopies)
            .Map(dest => dest.AuthorName, src => src.Author != null 
                ? $"{src.Author.FirstName} {src.Author.LastName}" 
                : null);
    }
}
```

**💡 Wichtige Konzepte:**
- **AuthorName wird aus Navigation Property berechnet**
- **Explizite Mappings für alle Properties**
- **Registrierung in DependencyInjection.cs**

---

### 3. **Infrastructure Layer** ✅

#### Repository mit .Include()

**BookRepository.cs:**
```csharp
public class BookRepository(AppDbContext dbContext) 
    : GenericRepository<Book>(dbContext), IBookRepository
{
    // ⚠️ WICHTIG: Override für Navigation Properties!
    public override async Task<Book?> GetByIdAsync(
        int id, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)  // ← KRITISCH für Navigation!
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public override async Task<IReadOnlyCollection<Book>> GetAllAsync(
        Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null,
        Expression<Func<Book, bool>>? filter = null,
        CancellationToken ct = default)
    {
        IQueryable<Book> query = Set
            .Include(b => b.Author)  // ← KRITISCH für Navigation!
            .AsNoTracking();
        
        if (filter is not null)
            query = query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        
        return await query.ToListAsync(ct);
    }
    
    public async Task<Book?> GetByISBNAsync(
        string isbn, CancellationToken ct = default)
    {
        return await Set
            .Include(b => b.Author)
            .SingleOrDefaultAsync(b => b.ISBN == isbn, ct);
    }
}
```

**💡 Wichtige Konzepte:**
- **`.Include(b => b.Author)` lädt Navigation Property**
- **Ohne Include ist `Author` immer `null`!**
- **Override von GetByIdAsync und GetAllAsync nötig**
- **SingleOrDefaultAsync für Unique-Constraints**

#### CSV-Seeding

**StartupDataSeeder.cs:**
```csharp
public async Task StartAsync(CancellationToken cancellationToken)
{
    using var scope = _serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    await dbContext.Database.MigrateAsync(cancellationToken);

    if (await dbContext.Authors.AnyAsync(cancellationToken)) 
        return;  // Daten schon vorhanden

    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var uniquenessChecker = new SeedDataUniquenessChecker();

    var authors = new Dictionary<string, Author>();
    var books = new List<Book>();

    var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);
    
    for (int i = 1; i < lines.Length; i++) // Skip header
    {
        var parts = lines[i].Split(';');
        
        // Author erstellen (wenn noch nicht vorhanden)
        var firstName = parts[0].Trim();
        var lastName = parts[1].Trim();
        var dateOfBirth = DateTime.Parse(parts[2].Trim());
        var authorKey = $"{firstName} {lastName}";

        if (!authors.TryGetValue(authorKey, out var author))
        {
            author = await Author.CreateAsync(
                firstName, lastName, dateOfBirth, 
                uniquenessChecker, cancellationToken);
            authors[authorKey] = author;
            
            await uow.Authors.AddAsync(author, cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);  // ← SOFORT speichern!
        }

        // Book erstellen (mit Author-Referenz)
        var isbn = parts[3].Trim();
        var title = parts[4].Trim();
        var publicationYear = int.Parse(parts[5].Trim());
        var availableCopies = int.Parse(parts[6].Trim());

        var book = await Book.CreateAsync(
            isbn, title, author, publicationYear, availableCopies, 
            uniquenessChecker, cancellationToken);
        books.Add(book);
    }

    // Alle Books auf einmal speichern
    await uow.Books.AddRangeAsync(books, cancellationToken);
    await uow.SaveChangesAsync(cancellationToken);
}
```

**💡 Wichtige Konzepte:**
- **Authors SOFORT speichern → ID wird generiert**
- **Books nutzen gespeicherte Author-Objekte**
- **Dictionary verhindert Duplikate**
- **AddRangeAsync für Performance**

**library_seed_data.csv:**
```csv
FirstName;LastName;DateOfBirth;ISBN;Title;PublicationYear;AvailableCopies
J.K.;Rowling;1965-07-31;9780747532699;Harry Potter and the Philosopher's Stone;1997;5
J.K.;Rowling;1965-07-31;9780439064873;Harry Potter and the Chamber of Secrets;1998;3
George R.R.;Martin;1948-09-20;9780553103540;A Game of Thrones;1996;4
```

---

### 4. **API Layer** ✅

#### Controller mit Result Pattern

**BooksController.cs:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class BooksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(GetBookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookCommand command, 
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(
            this, 
            createdAtAction: nameof(GetById), 
            routeValues: new { id = result?.Value?.Id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetBookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetBookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllBooksQuery(), ct);
        return result.ToActionResult(this);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteBookCommand(id), ct);
        return result.ToActionResult(this);
    }
}
```

**💡 Wichtige Konzepte:**
- **MediatR Pattern - Controller nur Koordination**
- **Result.ToActionResult() wandelt in HTTP Status**
- **ProducesResponseType für Swagger**
- **CreatedAtAction für 201 Created**

---

## 🎓 Lern-Schwerpunkte

### 1. **Navigation Properties richtig nutzen**

```csharp
// ❌ FALSCH: Author ist null
var book = await dbContext.Books.FindAsync(1);
var authorName = book.Author.FirstName;  // NullReferenceException!

// ✅ RICHTIG: Mit .Include()
var book = await dbContext.Books
    .Include(b => b.Author)
    .FirstOrDefaultAsync(b => b.Id == 1);
var authorName = book.Author.FirstName;  // Funktioniert!
```

### 2. **Factory-Methoden mit Validation**

```csharp
// ✅ Validation SOFORT in CreateAsync
public static async Task<Entity> CreateAsync(...)
{
    ValidateProperties(...);      // Domain Validation
    await ValidateExternal(...);   // Uniqueness Check
    return new Entity { ... };     // Nur wenn valid!
}
```

### 3. **Foreign Keys automatisch durch EF Core**

```csharp
// ❌ FALSCH: Manuell setzen
return new Book
{
    AuthorId = author.Id,  // Kann 0 sein wenn Author noch nicht gespeichert!
    Author = author
};

// ✅ RICHTIG: EF Core setzt automatisch
return new Book
{
    Author = author  // EF Core setzt AuthorId beim Speichern!
};
```

### 4. **CSV-Seeding mit Foreign Keys**

```csharp
// ✅ Authors SOFORT speichern:
await uow.Authors.AddAsync(author, cancellationToken);
await uow.SaveChangesAsync(cancellationToken);  // ID wird generiert!

// Jetzt hat author.Id einen Wert!
var book = await Book.CreateAsync(..., author, ...);
```

---

## 🧪 Testing

### Domain Tests
```bash
cd Domain.Tests
dotnet test
```

### API Tests (via Swagger)
```bash
cd Api
dotnet run
# Browser: http://localhost:5100/swagger
```

**Test-Szenarien:**
1. ✅ **Create Book** mit validem AuthorId
2. ✅ **Create Book** mit ungültigem AuthorId → 404
3. ✅ **Get Book** → AuthorName ist gefüllt
4. ✅ **Get All Books** → Alle 10 Books mit AuthorName
5. ✅ **Delete Book** → 204 No Content

---

## 📊 Architektur-Highlights

### CQRS Pattern
- Commands ändern Daten
- Queries lesen Daten
- Strikte Trennung

### Result Pattern
- Keine Exceptions für Business-Logic
- HTTP Status Codes als Enum
- Typsichere Fehlerbehandlung

### Repository Pattern
- IGenericRepository für Standard-CRUD
- Spezifische Repositories für Custom-Queries
- UnitOfWork für Transaktionen

### Domain-Driven Design
- Factory-Methoden für Objekt-Erstellung
- Domain Validations verhindern ungültige Objekte
- Rich Domain Model (keine Anemic Models)

---

## 🔍 Häufige Fehler (und wie sie gelöst wurden)

### 1. **Author ist immer null**
**Problem:** Navigation Property nicht geladen  
**Lösung:** `.Include(b => b.Author)` in Repository

### 2. **ISBN/AuthorId sind 0 oder null**
**Problem:** Tippfehler in DTO (`AuhorId` statt `AuthorId`)  
**Lösung:** Property-Namen korrigiert + Mapster-Config

### 3. **Books werden nicht geseedet**
**Problem:** AuthorId war 0 weil Author noch nicht gespeichert  
**Lösung:** Authors SOFORT speichern vor Book-Erstellung

### 4. **Validation-Fehler beim Seeding**
**Problem:** UniquenessChecker prüft auch beim Seeding  
**Lösung:** SeedDataUniquenessChecker gibt immer `true` zurück

---

## ✅ Erfolgskriterien (alle erfüllt)

- [x] Domain Layer mit Factory-Methoden
- [x] Alle ValidationSpecifications implementiert
- [x] Application Layer mit CQRS
- [x] Navigation Properties korrekt geladen
- [x] CSV-Seeding funktioniert (10 Books)
- [x] API über Swagger testbar
- [x] Alle DTO-Properties gefüllt
- [x] Result Pattern verwendet
- [x] FluentValidation in Pipeline
- [x] UnitOfWork Pattern
- [x] Mapster für DTO-Mapping

---

**Version:** 1.0 (Vollständige Lösung)  
**Branch:** solution/uebung-02  
**Status:** ✅ Alle Features implementiert und getestet

